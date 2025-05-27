using Hairdresser.DTOs;
using Hairdresser.DTOs.User;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Repositories.Interfaces
{
    public interface IHairdresserService
    {
        Task<IEnumerable<UserRespondDto>> GetAllHairdressersAsync();

        Task<IEnumerable<BookingResponseDto>> GetWeekScheduleAsync(string hairdresserId, DateTime weekStart);

        Task<IEnumerable<BookingResponseDto>> GetMonthlyScheduleAsync(string hairdresserId, int year, int month);

        Task<BookingResponseDto?> GetBookingDetailsAsync(int bookingId);
        Task<ApplicationUser?> UpdateHairdresserAsync(string id, UpdateUserDto userRequest);

        Task<UserDto> GetHairdresserWithId(string id);
    }
}
