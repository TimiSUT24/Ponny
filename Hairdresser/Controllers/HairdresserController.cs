using Hairdresser.Data;
using Hairdresser.DTOs;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HairdresserController(ApplicationDBContext context, IGenericRepository<ApplicationUser> repository) : ControllerBase
    {
        private readonly IGenericRepository<ApplicationUser> _repository = repository;
        private readonly ApplicationDBContext _context = context;


        [AllowAnonymous]
        [HttpGet(Name = "GetAllHairdressers")]
        public async Task<IActionResult> GetAll()
        {
            var hairdressers = await _repository.GetAllAsync();
            return Ok(hairdressers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddNewHairdresser")]
        public async Task<IActionResult> Create(string firstName, string lastName, string email, string phone)
        {
            var hairdresser = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phone,
            };
            await _repository.AddAsync(hairdresser);
            await _repository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), hairdresser);
        }

        // Get week schedule for hairdresser
        [Authorize(Roles = "Hairdresser")]
        [HttpGet("schedule")]
        public async Task<IActionResult> GetSchedule([FromQuery] string hairdresserId, [FromQuery] DateTime weekStart)
        {
            var weekEnd = weekStart.AddDays(7);

            var bookings = await _context.Bookings
                .Where(b => b.HairdresserId == hairdresserId && b.Start >= weekStart && b.Start < weekEnd)
                .Include(b => b.Customer)
                .Include(b => b.Treatment)
                .ToListAsync();

            return Ok(bookings);
        }

        [Authorize(Roles = "Hairdresser")]
        [HttpGet("monthly-schedule")]
        public async Task<IActionResult> GetMonthlySchedule([FromQuery] string hairdresserId, [FromQuery] int year, [FromQuery] int month)
        {
            var monthStart = new DateTime(year, month, 1);
            var monthEnd = monthStart.AddMonths(1);

            var bookings = await _context.Bookings
                .Where(b => b.HairdresserId == hairdresserId && b.Start >= monthStart && b.Start < monthEnd)
                .Include(b => b.Customer)
                .Include(b => b.Treatment)
                .Include(b => b.Hairdresser)
                .OrderBy(b => b.Start)
                .ToListAsync();

            var result = bookings.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                Start = b.Start,
                End = b.End,
                Treatment = b.Treatment,
                UserDto = new UserDto
                {
                    UserName = b.Customer.UserName,
                    Email = b.Customer.Email,
                    PhoneNumber = b.Customer.PhoneNumber
                },
                Hairdresser = b.Hairdresser
            });

            return Ok(result);
        }

        [Authorize(Roles = "Hairdresser,Admin")]
        [HttpGet("booking/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookingResponseDto>> GetBookingDetails(int id)
        {
            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.Customer)
                    .Include(b => b.Treatment)
                    .Select(b => new BookingResponseDto()
                    {
                        Id = b.Id,
                        Start = b.Start,
                        End = b.End,
                        Treatment = new Treatment
                        {
                            Name = b.Treatment.Name,
                            Price = b.Treatment.Price
                        },
                        Customer = new ApplicationUser
                        {
                            FirstName = b.Customer.FirstName,
                            LastName = b.Customer.LastName,
                            Email = b.Customer.Email,
                            PhoneNumber = b.Customer.PhoneNumber
                        },
                    })
                    .FirstOrDefaultAsync(booking => booking.Id == id);

                if (booking == null)
                    return NotFound();

                return Ok(booking);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Invalid booking ID");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
