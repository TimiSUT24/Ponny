using HairdresserClassLibrary.Models;
using Hairdresser.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hairdresser.DTOs;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public BookingsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Get all available times for a hairdresser
        [HttpGet("available-times")]
        public async Task<IActionResult> GetAvailableTimes(string hairdresserId, int treatmentId, DateTime day)
        {
            var treatment = await _context.Treatments.FindAsync(treatmentId);
            if (treatment == null) return NotFound("Behandling hittades inte");

            var startOfDay = day.Date.AddHours(9); // frisör jobbar från 09:00
            var endOfDay = day.Date.AddHours(17);  // till 17:00
            var duration = TimeSpan.FromMinutes(treatment.Duration);

            // Hämta bokade tider
            var bookings = await _context.Bookings
                .Where(b => b.HairdresserId == hairdresserId && b.Start.Date == day.Date)
                .ToListAsync();

            var availableSlots = new List<DateTime>();

            for (var time = startOfDay; time + duration <= endOfDay; time += TimeSpan.FromMinutes(15))
            {
                bool overlaps = bookings.Any(b =>
                    time < b.End && (time + duration) > b.Start);

                if (!overlaps)
                    availableSlots.Add(time);
            }

            return Ok(availableSlots);
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

            return Ok(new BookingResponseDto
            {
				Id = booking.Id,
				Start = booking.Start,
				End = booking.End,
				Treatment = booking.Treatment,
				Customer = booking.Customer
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
