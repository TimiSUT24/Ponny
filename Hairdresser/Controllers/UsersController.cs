using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairdresserClassLibrary.Models;
using Hairdresser.Data;
using Microsoft.AspNetCore.Identity;
using HairdresserClassLibrary.DTOs.User;

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

            // Skapa användare med lösenord
            var result = await _userManager.CreateAsync(newUser, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Tilldela rollen "User"
            await _userManager.AddToRoleAsync(newUser, "User");

            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }

        // Get user by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // Get all users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();

            var userRespondDtos = new List<UserRespondDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault(); // Takes the first role, or null if none exists.

                userRespondDtos.Add(new UserRespondDto
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
