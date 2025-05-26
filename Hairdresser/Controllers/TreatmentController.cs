using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Hairdresser.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Hairdresser.DTOs;

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
            var treatmentDtos = treatments.Select(t => new TreatmentDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Duration = t.Duration,
                Price = t.Price
            });

            return Ok(treatmentDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddNewTreatment")]
        public async Task<IActionResult> Create([FromBody] TreatmentCreateUpdateDto dto)
        {
            var treatment = new Treatment
            {
                Name = dto.Name,
                Description = dto.Description,
                Duration = dto.Duration,
                Price = dto.Price
            };

            await _repository.AddAsync(treatment);
            await _repository.SaveChangesAsync();

            var result = new TreatmentDto
            {
                Id = treatment.Id,
                Name = treatment.Name,
                Description = treatment.Description,
                Duration = treatment.Duration,
                Price = treatment.Price
            };

            return CreatedAtAction(nameof(GetAll), new { id = treatment.Id }, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdateTreatment")]
        public async Task<IActionResult> Update(int id, [FromBody] TreatmentCreateUpdateDto dto)
        {
            var treatment = await _repository.GetByIdAsync(id);
            if (treatment == null)
            {
                return NotFound("Treatment not found");
            }

            treatment.Name = dto.Name;
            treatment.Description = dto.Description;
            treatment.Duration = dto.Duration;
            treatment.Price = dto.Price;

            await _repository.UpdateAsync(treatment);
            await _repository.SaveChangesAsync();

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
