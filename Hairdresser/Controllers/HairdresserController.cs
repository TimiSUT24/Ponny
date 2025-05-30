﻿using Hairdresser.DTOs;
using Hairdresser.DTOs.User;
using Hairdresser.Enums;
using Hairdresser.Mapping;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IActionResult> UpdateHairdresser(string id, [FromBody] UpdateUserDto userRequest)
        {

           var hairdresser =  await _hairdresserService.UpdateHairdresserAsync(id, userRequest);

            if (hairdresser is null)
            {
                return Unauthorized("Hairdresser is Unauthorized");
            }           

            return Ok(hairdresser);
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

                var hairdresser = await _hairdresserService.GetHairdresserWithId(id);

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
    }
}
