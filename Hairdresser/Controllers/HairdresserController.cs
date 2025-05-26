using Hairdresser.Data;
using Hairdresser.DTOs;
using Hairdresser.DTOs.User;
using Hairdresser.Enums;
using Hairdresser.Mapping;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HairdresserController(ApplicationDBContext context, IUserRepository UserRepository, IGenericRepository<Booking> BookingRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = UserRepository;
        private readonly IGenericRepository<Booking> _bookingRepository = BookingRepository;
        private readonly ApplicationDBContext _context = context;


        [AllowAnonymous]
        [HttpGet(Name = "GetAllHairdressers")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var hairdressers = (await _userRepository.GetAllAsync()).Select(user => user.MapToUserDTO());
            return Ok(hairdressers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddNewHairdresser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] RegisterUserDto userRequest)
        {
            var hairdresser = await _userRepository.RigisterUserAsync(userRequest, UserRoleEnum.Hairdresser);

            if (hairdresser == null)
            {
                return BadRequest("Failed to create hairdresser");
            }

            await _userRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHairdresserById), new { id = hairdresser.Id }, hairdresser);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateHairdresser(string id, [FromBody] UpdateUserDTO userRequest)
        {
            var hairdresser = await GetUserByRoleAsync(id, UserRoleEnum.Hairdresser);
            // var hairdresser = await _repository.GetByIdAsync(id);

            if (hairdresser is null)
            {
                return Unauthorized("Hairdresser is Unauthorized");
            }

            hairdresser.FirstName = userRequest.FirstName;
            hairdresser.LastName = userRequest.LastName;
            hairdresser.Email = userRequest.Email;
            hairdresser.PhoneNumber = userRequest.PhoneNumber;
            hairdresser.UserName = userRequest.Email;

            await _userRepository.UpdateAsync(hairdresser);

            await _userRepository.SaveChangesAsync();
            await _userRepository.SaveChangesAsync();

            return Ok(hairdresser.MapToUserDTO());
        }

        [Authorize(Roles = "Hairdresser,Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteHairdresser(string id)
        {
            var hairdresser = await _userRepository.GetByIdAsync(id);

            if (hairdresser is null)
            {
                return NotFound("Hairdresser not found");
            }

            var userBookings = await _bookingRepository.FindAsync(booking => booking.HairdresserId == hairdresser.Id);

            if (userBookings.Any())
            {
                return BadRequest("Cannot delete hairdresser with existing bookings");
            }

            await _userRepository.DeleteAsync(hairdresser);
            await _userRepository.SaveChangesAsync();

            return NoContent();
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
                    .Select(booking => booking.MapToBookingResponseDto())
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

        [Authorize(Roles = "Hairdresser,Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HairdresserRespondDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetHairdresserById(string id)
        {
            var adminUser = await GetUserByRoleAsync(id, UserRoleEnum.Hairdresser);
            if (adminUser == null)
            {
                return Unauthorized("Hairdresser is Unauthorized");
            }

            var hairdresser = await _userRepository.GetHairdressersWithBookings(adminUser.Id);

            if (hairdresser == null)
            {
                return NotFound("Hairdresser not found");
            }

            return Ok(hairdresser);
        }

        // Move this method to a more appropriate place, like a service
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

            if (userId is null)
            {
                return null;
            }

            return await _userRepository.GetByIdAsync(userId);
        }
    }
}
