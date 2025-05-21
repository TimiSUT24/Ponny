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
        private readonly ApplicationDBContext _context;
        private readonly IBookingService _bookingService;

        public BookingsController(ApplicationDBContext context, IBookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService; 
        }

        // Get all available times for a hairdresser
        [HttpGet("available-times")]
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

        // Book an appointment
        [Authorize]
        [HttpPost("book")]
        public async Task<IActionResult> BookAppointment([FromBody] BookingRequestDto request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("Användaren är inte inloggad.");
                }                  
                var booking = await _bookingService.BookAppointment(userId, request);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Cancel a booking
        [Authorize]
        [HttpDelete("cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId, [FromQuery] string customerId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null)
                return NotFound("Bokning hittades inte.");

            if (booking.CustomerId != customerId)
                return Forbid("Du kan bara avboka dina egna tider.");

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Bokningen har avbokats.",
                booking.Id,
                booking.Start,
                booking.TreatmentId
            });
        }
    }
}
