using Hairdresser.DTOs;
using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Services
{
    public class HairdresserService : IHairdresserService
    {
        private readonly IHairdresserRepository _hairdresserRepo;
        private readonly IBookingRepository _bookingRepo;

        public HairdresserService(IHairdresserRepository hairdresserRepo, IBookingRepository bookingRepo)
        {
            _hairdresserRepo = hairdresserRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllHairdressersAsync()
        {
            return await _hairdresserRepo.GetAllAsync();
        }

        public async Task<IEnumerable<BookingResponseDto>> GetWeekScheduleAsync(string hairdresserId, DateTime weekStart)
        {
            var bookings = await _bookingRepo.GetWeekScheduleWithDetailsAsync(hairdresserId, weekStart);
            return ConvertToDtoList(bookings);
        }

        public async Task<IEnumerable<BookingResponseDto>> GetMonthlyScheduleAsync(string hairdresserId, int year, int month)
        {
            var bookings = await _bookingRepo.GetMonthlyScheduleWithDetailsAsync(hairdresserId, year, month);
            return ConvertToDtoList(bookings);
        }

        public async Task<BookingResponseDto?> GetBookingDetailsAsync(int bookingId)
        {
            var booking = await _bookingRepo.GetBookingWithDetailsAsync(bookingId);
            if (booking == null) return null;

            return new BookingResponseDto
            {
                Id = booking.Id,
                Start = booking.Start,
                End = booking.End,
                Treatment = booking.Treatment,
                UserDto = new UserDto
                {
                    UserName = booking.Customer?.UserName,
                    Email = booking.Customer?.Email,
                    PhoneNumber = booking.Customer?.PhoneNumber
                },
                Hairdresser = booking.Hairdresser
            };
        }

        private List<BookingResponseDto> ConvertToDtoList(IEnumerable<Booking> bookings)
        {
            return bookings.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                Start = b.Start,
                End = b.End,
                Treatment = b.Treatment,
                UserDto = new UserDto
                {
                    UserName = b.Customer?.UserName,
                    Email = b.Customer?.Email,
                    PhoneNumber = b.Customer?.PhoneNumber
                },
                Hairdresser = b.Hairdresser
            }).ToList();
        }
    }
}