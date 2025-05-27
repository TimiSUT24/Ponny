using HairdresserClassLibrary.Models;

namespace Hairdresser.Repositories.Interfaces
{
    public interface IHairdresserRepository : IGenericRepository<ApplicationUser>
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();

        Task<ApplicationUser?> GetByIdAsync(string id);
    }
}
