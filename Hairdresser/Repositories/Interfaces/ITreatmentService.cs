using HairdresserClassLibrary.Models;

namespace Hairdresser.Repositories.Interfaces
{
    public interface ITreatmentService
    {
        Task<IEnumerable<Treatment>> GetAllAsync();
        Task<Treatment?> GetByIdAsync(int id);
        Task<Treatment> CreateAsync(Treatment treatment);
        Task<bool> UpdateAsync(Treatment treatment);
        Task<bool> DeleteAsync(int id);
    }
}
