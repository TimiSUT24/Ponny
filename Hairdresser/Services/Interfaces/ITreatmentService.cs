using HairdresserClassLibrary.DTOs;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Services.Interfaces
{
    public interface ITreatmentService
    {
        Task<IEnumerable<TreatmentDto>> GetAllAsync();
        Task<Treatment?> GetByIdAsync(int id);
        Task<TreatmentDto> CreateAsync(CreateTreatmentDTO treatment);
        Task<bool> UpdateAsync(int id, TreatmentUpdateDto treatment);
        Task<bool> DeleteAsync(int id);
    }
}
