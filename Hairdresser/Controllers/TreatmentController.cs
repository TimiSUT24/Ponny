using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.Models;
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
                return NotFound();

            return Ok(treatment);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Treatment treatment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _treatmentService.CreateAsync(treatment);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Treatment treatment)
        {
            if (id != treatment.Id)
                return BadRequest("ID mismatch");

            var success = await _treatmentService.UpdateAsync(treatment);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _treatmentService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}