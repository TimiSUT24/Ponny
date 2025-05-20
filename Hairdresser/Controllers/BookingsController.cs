using HairdresserClassLibrary.Models;
using Hairdresser.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hairdresser.DTOs;
using Hairdresser.Repositories.Interfaces;

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
        [HttpPost("book")]
        public async Task<IActionResult> BookAppointment([FromBody] BookingRequestDto request)
        {
            var treatment = await _context.Treatments.FindAsync(request.TreatmentId);
            if (treatment == null)
                return NotFound("Behandling hittades inte.");

            var end = request.Start.AddMinutes(treatment.Duration);

            // Kontrollera om frisören är upptagen
            bool isAvailable = !await _context.Bookings.AnyAsync(b =>
                b.HairdresserId == request.HairdresserId &&
                b.Start < end && b.End > request.Start
            );

            if (!isAvailable)
                return Conflict("Frisören är upptagen vid denna tid.");

            var booking = new Booking
            {
                CustomerId = request.CustomerId,
                HairdresserId = request.HairdresserId,
                TreatmentId = request.TreatmentId,
                Start = request.Start,
                End = end
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Bokning skapad!",
                booking.Id,
                booking.Start,
                booking.End
            });
        }

        // Cancel a booking
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
