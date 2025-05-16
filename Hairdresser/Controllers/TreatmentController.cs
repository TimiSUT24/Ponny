using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Hairdresser.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreatmentController : Controller
    {
        private readonly IGenericRepository<Treatment> _repository;

        public TreatmentController(IGenericRepository<Treatment> repository)
        {
            _repository = repository;
        }
        [AllowAnonymous]
        [HttpGet(Name = "GetAllTreatments")]
        public async Task<IActionResult> GetAll()
        {
            var treatments = await _repository.GetAllAsync();
            return Ok(treatments);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddNewTreatment")]
        public async Task<IActionResult> Create(string name, string description, int duration, double price  )
        {
            var treatment = new Treatment
            {
                Name = name,
                Description = description,
                Price = price,
                Duration = duration,

            };
            await _repository.AddAsync(treatment);
            await _repository.SaveChangesAsync();
            return CreatedAtAction(nameof(Treatment), treatment);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut(Name = "UpdateTreatment")]
        public async Task<IActionResult> Update(int id, [FromBody] Treatment treatment)
        {
            if(treatment.Id == id)
            {
                await _repository.UpdateAsync(treatment);
                await _repository.SaveChangesAsync();
            }
           
            return Ok("Treatment was updated");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Name = "DeleteTreatment")]

        public async Task<IActionResult> Delete(int id)
        {
            var treatment = await _repository.GetByIdAsync(id); 

            await _repository.DeleteAsync(treatment);
            await _repository.SaveChangesAsync();
            return Ok("Treatment was deleted"); 
        }
    }
}
