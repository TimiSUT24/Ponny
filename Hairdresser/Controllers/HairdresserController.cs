using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HairdresserController : ControllerBase
    {
        private readonly IGenericRepository<ApplicationUser> _repository;

        public HairdresserController(IGenericRepository<ApplicationUser> repository)
        {
            _repository = repository;
        }
        [AllowAnonymous]
        [HttpGet(Name = "GetAllHairdressers")]
        public async Task<IActionResult> GetAll()
        {
            var hairdressers = await _repository.GetAllAsync();
            return Ok(hairdressers);
        }

        [Authorize]
        [HttpPost(Name = "AddNewHairdresser")]
        public async Task<IActionResult> Create(string firstName, string lastName, string email, string phone)
        {
            var hairdresser = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phone,
            };
            await _repository.AddAsync(hairdresser);
            await _repository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), hairdresser);
        }
    }
}
