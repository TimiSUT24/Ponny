using Hairdresser.Mapping;
using Hairdresser.Services;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.DTOs.User;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hairdresser.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(UserManager<ApplicationUser> userManager, IUserService userService, SignInManager<ApplicationUser> signInManager, JWT_Service jwtService) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IUserService _userService = userService;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly JWT_Service _jwtService = jwtService;

    // Get user by ID
    [Authorize(Roles = "User,Admin")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (loggedInUser != id && !User.IsInRole("Admin"))
        {
            return Unauthorized("You are not authorized to update this user.");
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user.MapToUserDTO());
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized("Invalid credentials");
        }
          
        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized("Invalid credentials");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateToken(user.Id, roles);

        return Ok(new
        {
            Token = token,
            user.Id
        });
    }
    [HttpPost("Add-Hairdresser")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateHairdresser([FromBody] RegisterUserDto newHairdresser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new ApplicationUser
        {
            UserName = newHairdresser.UserName,
            Email = newHairdresser.Email,
            PhoneNumber = newHairdresser.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, newHairdresser.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _userManager.AddToRoleAsync(user, "Hairdresser");

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user.MapToUserDTO());
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var newUser = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
        };

        // Create user with password
        var result = await _userManager.CreateAsync(newUser, dto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        // Assign the role "User"
        await _userManager.AddToRoleAsync(newUser, "User");

        return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser.MapToUserDTO());
    }

    [Authorize(Roles = "Hairdresser,Admin")]
    [HttpPut("Hairdresser/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateHairdresser(string id, [FromBody] UpdateUserDto userRequest)
    {

        var hairdresser = await _userService.UpdateHairdresserAsync(id, userRequest);

        if (hairdresser is null)
        {
            return Unauthorized("Hairdresser is Unauthorized");
        }

        return Ok(hairdresser);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = "User")]
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(loggedInUser != id)
        {
            return Unauthorized("You can only delete your own account");
        }
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        
        var result = await _userManager.DeleteAsync(user);
        if(result is null)
        {
            return BadRequest("Error deleting user");
        }

        return Ok(result.Succeeded ? "User deleted successfully" : "Error deleting user");
    }
}
