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
    }
}
