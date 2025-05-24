using Hairdresser.Data;
using Hairdresser.DTOs;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController(ApplicationDBContext context) : ControllerBase
    {
        private readonly ApplicationDBContext _context = context;

        [HttpGet("bookings-overview")]
        public async Task<IActionResult> GetAllBookingsOverview()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Treatment)
                .Include(b => b.Hairdresser)
                .OrderByDescending(b => b.Start)
                .ToListAsync();

            var bookingDtos = bookings.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                Start = b.Start,
                End = b.End,
                Treatment = b.Treatment,
                UserDto = new UserDto
                {
                    UserName = b.Customer.UserName,
                    Email = b.Customer.Email,
                    PhoneNumber = b.Customer.PhoneNumber
                },
                Hairdresser = b.Hairdresser
            });

            return Ok(bookingDtos);
        }
    }
}
