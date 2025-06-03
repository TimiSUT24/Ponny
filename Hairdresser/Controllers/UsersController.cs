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

        // Register User

        [HttpPost("registerUser")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto ))]
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

            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser.MapToUserDTO());
        }
        [HttpPost("add-hairdresser")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateHairdresser([FromBody] RegisterUserDto newHairdresser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = newHairdresser.UserName,
                Email = newHairdresser.Email,
                PhoneNumber = newHairdresser.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, newHairdresser.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Hairdresser");

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user.MapToUserResponseDTO());
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
