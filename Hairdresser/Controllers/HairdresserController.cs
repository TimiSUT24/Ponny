using Hairdresser.Data;
using Hairdresser.DTOs;
using Hairdresser.Enums;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HairdresserController(ApplicationDBContext context, IUserRepository repository) : ControllerBase
    {
        private readonly IUserRepository _repository = repository;
        private readonly ApplicationDBContext _context = context;


        [AllowAnonymous]
        [HttpGet(Name = "GetAllHairdressers")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAll()
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

        // [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HairdresserRespondDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHairdresserById(string id)
        {
            var adminUser = await GetUserByRoleAsync(id, UserRoleEnum.Admin);
            if (adminUser == null)
            {
                return NotFound("Hairdresser not found");
            }

            var hairdresser = await _context.Users
                .Include(u => u.CustomerBookings)
                    .ThenInclude(b => b.Treatment)
                .Select(user => new HairdresserRespondDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    Bookings = user.HairdresserBookings.Select(booking => new HairdresserBookingRespondDTO
                    {
                        Id = booking.Id,
                        Start = booking.Start,
                        End = booking.End,
                        Treatment = new TreatmentDTO
                        {
                            Name = booking.Treatment.Name,
                            Duration = booking.Treatment.Duration,
                            Price = booking.Treatment.Price
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync(user => user.Id == adminUser.Id);
            if (hairdresser == null)
            {
                return NotFound("Hairdresser not found");
            }
            return Ok(hairdresser);
        }

        private async Task<ApplicationUser?> GetUserByRoleAsync(string id, UserRoleEnum userRole)
        {
            var roleId = await _context.Roles
                .Where(r => r.Name == userRole.ToString())
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
            if (roleId == null)
            {
                return null;
            }
            var userId = await _context.UserRoles
                .Where(ur => ur.RoleId == roleId && ur.UserId == id)
                .Select(ur => ur.UserId)
                .FirstOrDefaultAsync();

            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return null;
            }

            return await _repository.GetByIdAsync(userGuid);
        }
    }
}
