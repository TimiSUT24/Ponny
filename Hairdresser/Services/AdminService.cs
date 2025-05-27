using Hairdresser.DTOs;
using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services.Interfaces;

namespace Hairdresser.Services
{
    public class AdminService : IAdminService
    {
        private readonly IBookingRepository _bookingRepository;

        public AdminService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllBookingsOverviewAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();

            var detailedBookings = bookings.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                Start = b.Start,
                End = b.End,
                Treatment = b.Treatment,
                UserDto = new UserDto
                {
                    Id = b.Customer?.Id,
                    UserName = b.Customer?.UserName,
                    Email = b.Customer?.Email,
                    PhoneNumber = b.Customer?.PhoneNumber
                },
                Hairdresser = b.Hairdresser
            });

            return detailedBookings;
        }
    }
}
