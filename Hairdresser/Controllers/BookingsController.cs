using HairdresserClassLibrary.Models;
using Hairdresser.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("treatments")]
        public async Task<ActionResult<IEnumerable<Treatment>>> GetAllTreatments()
        {
            var treatments = await _context.Treatments
                .AsNoTracking()
                .ToListAsync();

            return Ok(treatments);
        }

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

    }
}
