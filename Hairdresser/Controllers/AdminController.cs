using HairdresserClassLibrary.DTOs.User;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(IAdminService adminService, UserManager<ApplicationUser> userManager)
        {
            _adminService = adminService;
            _userManager = userManager;
        }

        [HttpGet("bookings-overview")]
        public async Task<IActionResult> GetAllBookingsOverview()
        {
            var bookingDtos = await _adminService.GetAllBookingsOverviewAsync();
            return Ok(bookingDtos);
        }

        [HttpPost("add-hairdresser")]
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

            var response = new UserResponseDto
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