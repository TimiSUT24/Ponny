using Hairdresser.Data;
using Hairdresser.DTOs;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

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
                    Id = b.Customer.Id,
                    UserName = b.Customer.UserName,
                    Email = b.Customer.Email,
                    PhoneNumber = b.Customer.PhoneNumber
                },
                Hairdresser = b.Hairdresser
            });

            return Ok(bookingDtos);
        }

        [HttpPost("add-hairdresser")]
        public async Task<IActionResult> CreateHairdresser([FromBody] UserDto newHairdresser)
        {
            var user = new ApplicationUser
            {
                UserName = newHairdresser.UserName,
                Email = newHairdresser.Email,
                PhoneNumber = newHairdresser.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, "DefaultPassword123!");

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Hairdresser");

            var response = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = "Hairdresser"
            };

            return Ok(response);
        }
    }
}