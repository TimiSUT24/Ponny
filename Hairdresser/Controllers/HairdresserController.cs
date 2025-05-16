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
    public class HairdresserController : ControllerBase
    {
        private readonly IGenericRepository<ApplicationUser> _repository;
        private readonly ApplicationDBContext _context;


        public HairdresserController(IGenericRepository<ApplicationUser> repository)
        {
            _repository = repository;
        }
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

        [Authorize(Roles = "Hairdresser,Admin")]
        [HttpGet("booking/{id}")]
        public async Task<ActionResult<BookingRespondDto>> GetBookingDetails(int id)
        {
            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.Customer)
                    .Include(b => b.Treatment)
                    .FirstOrDefaultAsync(booking => booking.Id == id);

                if (booking == null)
                    return NotFound();

                var bookingRespond = new Booking
                {
                    Id = booking.Id,
                    Start = booking.Start,
                    End = booking.End,
                    Treatment = booking.Treatment,
                    Customer = booking.Customer,
                    Hairdresser = booking.Hairdresser,
                };

                return Ok(bookingRespond);
                // return Ok(new
                // {
                //     booking.Id,
                //     booking.Start,
                //     booking.End,
                //     Treatment = booking.Treatment.Name,
                //     Customer = $"{booking.Customer.FirstName} {booking.Customer.LastName}",
                //     booking.Customer.Email,
                //     booking.Customer.PhoneNumber
                // });
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Invalid booking ID");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
