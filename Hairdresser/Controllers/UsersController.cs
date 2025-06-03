using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairdresserClassLibrary.Models;
using Hairdresser.Data;
using Microsoft.AspNetCore.Identity;
using HairdresserClassLibrary.DTOs.User;
using Hairdresser.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Hairdresser.Mapping;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(ApplicationDBContext context, UserManager<ApplicationUser> userManager, IUserService userService) : ControllerBase
    {
        private readonly ApplicationDBContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUserService _userService = userService;



        // Get all users
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponseDto>))]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();

            var userRespondDtos = new List<UserResponseDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault(); // Takes the first role, or null if none exists.

                userRespondDtos.Add(new UserResponseDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = role
                });
            }

            return Ok(userRespondDtos);
        }

        [HttpGet("bookings-overview")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBookingsOverview()
        {
            var bookingDtos = await _userService.GetAllBookingsOverviewAsync();
            return Ok(bookingDtos);
        }

        // Change User Info
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserDto updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound();

            existingUser.UserName = updatedUser.UserName;
            existingUser.Email = updatedUser.Email;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
