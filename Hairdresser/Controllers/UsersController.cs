using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairdresserClassLibrary.Models;
using Hairdresser.Data;
using Microsoft.AspNetCore.Identity;
using HairdresserClassLibrary.DTOs.User;
using Hairdresser.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Hairdresser.Mapping;
using HairdresserClassLibrary.DTOs;
using System.Security.Claims;
using Hairdresser.Enums;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(ApplicationDBContext context, UserManager<ApplicationUser> userManager, IUserService userService) : ControllerBase
    {
        private readonly ApplicationDBContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUserService _userService = userService;

        // Get all users
        [Authorize(Roles = "Admin")]
        [HttpGet("Get-Users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponseDto>))]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();

            var userRespondDtos = new List<UserResponseDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault(); // Takes the first role, or null if none exists.

                userRespondDtos.Add(new UserResponseDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = role
                });
            }

            return Ok(userRespondDtos);
        }

        [HttpGet("Bookings-Overview")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BookingResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllBookingsOverview()
        {
            var bookingDtos = await _userService.GetAllBookingsOverviewAsync();
            if (bookingDtos is null)
            {
                return NotFound("No bookings found.");
            }
            return Ok(bookingDtos);
        }

        [Authorize(Roles = "Hairdresser,Admin")]
        [HttpGet("Hairdresser-Week-Schedule")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BookingResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSchedule([FromQuery] string hairdresserId, [FromQuery] DateTime weekStart)
        {
            var result = await _userService.GetWeekScheduleAsync(hairdresserId, weekStart);
            if (result is null)
            {
                return NotFound("No bookings found for the specified hairdresser and week start date.");
            }
            return Ok(result);
        }

        [Authorize(Roles = "Hairdresser,Admin")]
        [HttpGet("Hairdresser-Monthly-Schedule")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BookingResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMonthlySchedule([FromQuery] string hairdresserId, [FromQuery] int year, [FromQuery] int month)
        {
            var result = await _userService.GetMonthlyScheduleAsync(hairdresserId, year, month);
            if (result is null)
            {
                return NotFound("No bookings found for the specified hairdresser and month.");
            }
            return Ok(result);
        }

        [Authorize(Roles = "Hairdresser,Admin")]
        [HttpGet("Booking/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookingResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookingDetails(int id)
        {
            var booking = await _userService.GetBookingDetailsAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }


        [Authorize(Roles = "Hairdresser,Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HairdresserResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetHairdresserById(string id)
        {
            try
            {
                var hairdresser = await _userService.GetHairdresserWithId(id);

                if (hairdresser == null)
                {
                    return NotFound("Hairdresser not found");
                }

                return Ok(hairdresser);
            }
            catch
            {
                return NotFound();
            }

        }
        [HttpGet("Get-Hairdressers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHairdressers()
        {
            var hairdresser = await _userService.GetAllHairdressersAsync();
            if (hairdresser is null)
            {
                return NotFound("No hairdressers found.");
            }
            return Ok(hairdresser);
        }

        // Change User Info
        [Authorize(Roles = "User,Admin")]
        [HttpPut("Update-User/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto updatedUser)
        {
            var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loggedInUser != id && !User.IsInRole(UserRoleEnum.Admin.ToString()))
            {
                return Unauthorized("You are not authorized to update this user.");
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.UserName = updatedUser.UserName;
            existingUser.Email = updatedUser.Email;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;

            await _userManager.UpdateAsync(existingUser);
            await _context.SaveChangesAsync();

            return Ok("User was updated");
        }
    }
}
