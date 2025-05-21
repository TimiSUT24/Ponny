using Hairdresser.DTOs;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Repositories.Interfaces
{
    public interface IBookingService
    {
        Task<List<DateTime>> GetAllAvailableTimes(string hairdresserId, int treatmentId, DateTime day);

        Task<BookingResponseDto> BookAppointment(string customerId, BookingRequestDto request);

        Task<BookingRequestDto> CancelBooking(string customerId, int bookingId);
        Task<BookingResponseDto> GetBookingByIdAsync(int bookingId,string customerId);
    }
}
