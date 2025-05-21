using HairdresserClassLibrary.Models;

namespace Hairdresser.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<Booking> GetByIdWithDetailsAsync(int id,string customerId);
    }
}
