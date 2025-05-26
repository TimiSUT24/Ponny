using Hairdresser.DTOs;

namespace Hairdresser.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<BookingResponseDto>> GetAllBookingsOverviewAsync();
    }
}
