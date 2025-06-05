using Hairdresser.Mapping;
using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.DTOs;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Services
{
    public class TreatmentService : ITreatmentService
    {
        private readonly IGenericRepository<Treatment> _treatmentRepo;

        public TreatmentService(IGenericRepository<Treatment> treatmentRepo)
        {
            _treatmentRepo = treatmentRepo;
        }

        public async Task<IEnumerable<Treatment>> GetAllAsync()
        {
            return await _treatmentRepo.GetAllAsync();
        }

        public async Task<Treatment?> GetByIdAsync(int id)
        {
            return await _treatmentRepo.GetByIdAsync(id);
        }

        public async Task<TreatmentDto> CreateAsync(CreateTreatmentDTO CreateTreatment)
        {
            var treatment = CreateTreatment.MapToTreatment();

            await _treatmentRepo.AddAsync(treatment);
            await _treatmentRepo.SaveChangesAsync();
            return treatment.MapToTreatmentDto();
        }

        public async Task<bool> UpdateAsync(Treatment treatment)
        {
            var exists = await _treatmentRepo.AnyAsync(t => t.Id == treatment.Id);
            if (!exists) return false;

            await _treatmentRepo.UpdateAsync(treatment);
            await _treatmentRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var treatment = await _treatmentRepo.GetByIdAsync(id);
            if (treatment == null) return false;

            await _treatmentRepo.DeleteAsync(treatment);
            await _treatmentRepo.SaveChangesAsync();
            return true;
        }
    }
}