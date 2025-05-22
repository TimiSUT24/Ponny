using HairdresserClassLibrary.Models;
using Hairdresser.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hairdresser.DTOs;
using Hairdresser.Repositories.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService; 
        }

        // Get all available times for a hairdresser
        [HttpGet("Available-times")]
        public async Task<IActionResult> GetAvailableTimes(string hairdresserId, int treatmentId, DateTime day)
        {
            try
            {
                var availableTimes = await _bookingService.GetAllAvailableTimes(hairdresserId, treatmentId, day);
                return Ok(availableTimes);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); 
            }
        }
        [Authorize]
        [HttpGet("BookingsById")]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var booking = await _bookingService.GetBookingByIdAsync(bookingId,userId);
            if(booking == null)
            {
                return NotFound("Booking was not found");
            }
               
            return Ok(booking);
        }

        // Book an appointment
        [Authorize]
        [HttpPost("Book Appointment")]
        public async Task<IActionResult> BookAppointment([FromBody] BookingRequestDto request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User is not logged in.");
                }                  
                var booking = await _bookingService.BookAppointment(userId, request);
                return CreatedAtAction(nameof(GetBookingById), booking);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Cancel a booking
        [Authorize]
        [HttpDelete("Cancel Booking")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
           try
           {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User is not logged in.");
                }
                var booking = await _bookingService.CancelBooking(userId, bookingId);
                return CreatedAtAction(nameof(GetBookingById), booking);
            }
           catch (Exception ex)
           {
                return BadRequest(ex.Message);
           }
        }
    }
}
