using Hairdresser.DTOs;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<Booking?> GetByIdWithDetailsAsync(int id, string customerId);

        Task<IEnumerable<Booking>> GetWeekScheduleWithDetailsAsync(string hairdresserId, DateTime weekStart);    
        Task<IEnumerable<Booking>> GetMonthlyScheduleWithDetailsAsync(string hairdresserId, int year, int month);
        Task<Booking?> GetBookingWithDetailsAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsBetweenDatesAsync(DateTime start, DateTime end, string hairdresserId);
    }
}
