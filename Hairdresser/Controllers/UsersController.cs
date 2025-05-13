using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairdresserClassLibrary.Models;
using Hairdresser.Data;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDBContext _context;


        public UsersController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> Register([FromBody] IdentityUser newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] IdentityUser updatedUser)
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
