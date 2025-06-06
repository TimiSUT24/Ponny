using System.ComponentModel.DataAnnotations;
using Hairdresser.Mapping;
using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreatmentController : ControllerBase
    {
        private readonly ITreatmentService _treatmentService;

        public TreatmentController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var treatments = await _treatmentService.GetAllAsync();
            return Ok(treatments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var treatment = await _treatmentService.GetByIdAsync(id);
            if (treatment == null)
            {
                return NotFound();
            }

            return Ok(treatment);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateTreatmentDTO treatment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _treatmentService.CreateAsync(treatment);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TreatmentDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([Required][Range(1, double.PositiveInfinity, ErrorMessage = "The field cannot be less than 1")] int id, [FromBody] TreatmentUpdateDto treatment)
        {
            var success = await _treatmentService.UpdateAsync(id, treatment);
            if (!success)
            {
                return NotFound();
            }

            return Ok(treatment.MapToTreatmentDTO());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete([Required] [Range(1, double.PositiveInfinity, ErrorMessage = "The field cannot be less than 1")] int id)
        {
            var success = await _treatmentService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}