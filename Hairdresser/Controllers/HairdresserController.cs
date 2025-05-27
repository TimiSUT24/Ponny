using Hairdresser.DTOs;
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
        [HttpGet("booking/{id}")]
        public async Task<ActionResult<BookingResponseDto>> GetBookingDetails(int id)
        {
            var booking = await _hairdresserService.GetBookingDetailsAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }
    }
}
