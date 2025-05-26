using Hairdresser.DTOs;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Repositories.Interfaces
{
    public interface IHairdresserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllHairdressersAsync();

        Task<IEnumerable<BookingResponseDto>> GetWeekScheduleAsync(string hairdresserId, DateTime weekStart);

        Task<IEnumerable<BookingResponseDto>> GetMonthlyScheduleAsync(string hairdresserId, int year, int month);

        Task<BookingResponseDto?> GetBookingDetailsAsync(int bookingId);
    }
}
