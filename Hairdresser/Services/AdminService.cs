using Hairdresser.DTOs;
using Hairdresser.DTOs.User;
using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.DTOs;

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
                Treatment = new TreatmentDto
                {
                    Id = b.Treatment.Id,
                    Name = b.Treatment.Name,
                    Description = b.Treatment.Description,
                    Price = b.Treatment.Price
                },
                Costumer = new UserDto
                {
                    Id = b.Customer?.Id,
                    UserName = b.Customer?.UserName,
                    Email = b.Customer?.Email,
                    PhoneNumber = b.Customer?.PhoneNumber
                },
                Hairdresser = new UserDto
                {
                    Id = b.Hairdresser?.Id,
                    UserName = b.Hairdresser?.UserName,
                    Email = b.Hairdresser?.Email,
                    PhoneNumber = b.Hairdresser?.PhoneNumber
                }
            });


            return detailedBookings;
        }
    }
}
