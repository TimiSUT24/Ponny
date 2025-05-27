using Hairdresser.DTOs;
using Hairdresser.DTOs.User;
using Hairdresser.Enums;
using Hairdresser.Mapping;
using Hairdresser.Repositories;
using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HairdresserController : ControllerBase
    {
        private readonly IHairdresserService _hairdresserService;

        public HairdresserController(IHairdresserService hairdresserService)
        {
            _hairdresserService = hairdresserService;
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetAllHairdressers")]
        public async Task<IEnumerable<UserDto>> GetAllHairdressersAsync()
        {
            return await _hairdresserService.GetAllHairdressersAsync();
        }

        [Authorize(Roles = "Hairdresser")]
        [HttpGet("schedule")]
        public async Task<IActionResult> GetSchedule([FromQuery] string hairdresserId, [FromQuery] DateTime weekStart)
        {
            var result = await _hairdresserService.GetWeekScheduleAsync(hairdresserId, weekStart);
            return Ok(result);
        }

        [Authorize(Roles = "Hairdresser")]
        [HttpGet("monthly-schedule")]
        public async Task<IActionResult> GetMonthlySchedule([FromQuery] string hairdresserId, [FromQuery] int year, [FromQuery] int month)
        {
            var result = await _hairdresserService.GetMonthlyScheduleAsync(hairdresserId, year, month);
            return Ok(result);
        }

        [Authorize(Roles = "Hairdresser,Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateHairdresser(string id, [FromBody] UpdateUserDTO userRequest)
        {
            var hairdresser = await GetUserByRoleAsync(id, UserRoleEnum.Hairdresser);

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

            return Ok(hairdresser.MapToUserDTO());
        }

        [Authorize(Roles = "Hairdresser,Admin")]
        [HttpGet("booking/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookingResponseDto>> GetBookingDetails(int id)
        {
            var booking = await _hairdresserService.GetBookingDetailsAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }
        

        [Authorize(Roles = "Hairdresser,Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HairdresserResponseDTO))]
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
