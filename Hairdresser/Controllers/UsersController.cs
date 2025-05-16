using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairdresserClassLibrary.Models;
using Hairdresser.Data;
using Hairdresser.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public UsersController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Register User
        [HttpPost("registerUser")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var newUser = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }

        // Get user
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // Change User Info
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ApplicationUser updatedUser)
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
