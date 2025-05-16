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


        [HttpPost("registerUser")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var newUser = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ApplicationUser updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound();

            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Email = updatedUser.Email;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("hairdressers")]
        public async Task<IActionResult> GetHairdressers()
        {
            var hairdressers = await _userManager.GetUsersInRoleAsync("Hairdresser");

            var result = hairdressers.Select(h => new
            {
                h.Id,
                h.UserName,
                h.Email,
                h.PhoneNumber
            });

            return Ok(result);
        }


    }
}
