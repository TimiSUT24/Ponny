using HairdresserClassLibrary.DTOs;
using HairdresserClassLibrary.DTOs.User;

namespace Hairdresser.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllHairdressersAsync();

        Task<IEnumerable<BookingResponseDto>> GetWeekScheduleAsync(string hairdresserId, DateTime weekStart);

        Task<IEnumerable<BookingResponseDto>> GetMonthlyScheduleAsync(string hairdresserId, int year, int month);

        Task<BookingResponseDto?> GetBookingDetailsAsync(int bookingId);
        Task<UserDto?> UpdateHairdresserAsync(string id, UpdateUserDto userRequest);

        Task<UserDto> GetHairdresserWithId(string id);
    }
}
