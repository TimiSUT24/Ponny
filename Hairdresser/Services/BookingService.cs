using Hairdresser.Mapping;
using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.DTOs;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;

namespace Hairdresser.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IGenericRepository<Treatment> _treatmentRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingService(IGenericRepository<Treatment> treatment, IBookingRepository bookingRepository, UserManager<ApplicationUser> usermanager)
        {
            _treatmentRepository = treatment;
            _bookingRepository = bookingRepository;
            _userManager = usermanager;
        }

        public async Task<List<DateTime>> GetAllAvailableTimes(string hairdresserId, int treatmentId, DateTime day)
        {
            var hairdresser = await _userManager.FindByIdAsync(hairdresserId);
            if (hairdresser == null)
            {
                throw new KeyNotFoundException("Hairdresser was not found");
            }

            var treatment = await _treatmentRepository.GetByIdAsync(treatmentId);
            if (treatment == null)
            {
                throw new KeyNotFoundException("Treatment was not found");
            }

            if (day.Date < DateTime.Now.Date || day.Date > DateTime.Now.AddMonths(4).Date)
            {
                throw new ArgumentException("Can only book from today and up to 4 month in advance.");
            }

            var startOfDay = day.Date.AddHours(9); // Hairdresser works from 09:00
            var endOfDay = day.Date.AddHours(17);  // until 17:00
            var duration = TimeSpan.FromMinutes(treatment.Duration);

            // Retrive occupied bookings 
            var bookings = await _bookingRepository
                .FindAsync(b => b.HairdresserId == hairdresserId && b.Start.Date == day.Date);

            var availableSlots = new List<DateTime>();

            for (var time = startOfDay; time + duration <= endOfDay; time += TimeSpan.FromMinutes(15))
            {
                bool overlaps = bookings.Any(b =>
                    time < b.End && (time + duration) > b.Start);

                if (!overlaps)
                {
                    availableSlots.Add(time);
                }

            }
            return availableSlots;
        }

        public async Task<BookingResponseDto> BookAppointment(string customerId, BookingRequestDto request)
        {

            var treatment = await _treatmentRepository.GetByIdAsync(request.TreatmentId);
            if (treatment == null)
            {
                throw new KeyNotFoundException("Treatment was not found.");
            }

            var hairdresser = await _userManager.FindByIdAsync(request.HairdresserId);
            if (hairdresser == null)
            {
                throw new KeyNotFoundException("Hairdresser was not found");
            }

            var end = request.Start.AddMinutes(treatment.Duration);

            // check if hairdresser is booked 
            bool isAvailable = !await _bookingRepository.AnyAsync(b =>
                b.HairdresserId == request.HairdresserId &&
                b.Start < end && b.End > request.Start
            );

            if (!isAvailable)
            {
                throw new InvalidOperationException("Hairdresser is booked at this time.");
            }

            if (request.Start < DateTime.Now || request.Start > DateTime.Now.AddMonths(4))
            {
                throw new ArgumentException("Can only book from today and up to 4 month in advance.");
            }
            var booking = new Booking
            {
                CustomerId = customerId,
                HairdresserId = request.HairdresserId,
                TreatmentId = request.TreatmentId,
                Start = request.Start,
                End = end
            };

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            var savedBooking = await _bookingRepository.GetByIdWithDetailsAsync(booking.Id, customerId);

            return savedBooking.MapToBookingReponse2Dto();                  
        }

        public async Task<BookingDto> CancelBooking(string customerId, int bookingId)
        {
            var booking = await _bookingRepository.GetByIdWithDetailsAsync(bookingId, customerId);

            if (booking == null)
            {
                throw new KeyNotFoundException("Booking was not found.");
            }


            if (booking.CustomerId != customerId)
            {
                throw new UnauthorizedAccessException("Not a valid customer");
            }

            await _bookingRepository.DeleteAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            var message = "This booking was removed";
            return new BookingDto
            {
                Id = booking.Id,
                Start = booking.Start,
                End = booking.End,
                Message = message
            };
        }

        public async Task<BookingResponseDto> GetBookingByIdAsync(int bookingId, string customerId)
        {
            var booking = await _bookingRepository.GetByIdWithDetailsAsync(bookingId, customerId);
            if (booking == null)
            {
                throw new KeyNotFoundException("Booking was not found.");
            }

            if (booking.CustomerId != customerId)
            {
                throw new UnauthorizedAccessException("Not a valid customer");
            }      
            return booking.MapToBookingReponse2Dto();

        }

        public async Task<BookingResponseDto> RebookBooking(string customerId, int bookingId, BookingRequestDto bookingRequestDto)
        {
            var booking = await _bookingRepository.GetByIdWithDetailsAsync(bookingId, customerId);

            if (booking == null)
            {
                throw new KeyNotFoundException("Booking was not found.");
            }

            if (booking.CustomerId != customerId)
            {
                throw new UnauthorizedAccessException("Not a valid customer");
            }

            var hairdresser = await _userManager.FindByIdAsync(bookingRequestDto.HairdresserId);
            if (hairdresser == null)
            {
                throw new KeyNotFoundException("Hairdresser was not found");
            }

            var treatment = await _treatmentRepository.GetByIdAsync(bookingRequestDto.TreatmentId);
            if (treatment == null)
            {
                throw new KeyNotFoundException("Treatment was not found.");
            }
            var end = bookingRequestDto.Start.AddMinutes(treatment.Duration);

            bool isAvailable = !await _bookingRepository.AnyAsync(b =>
              b.Id == booking.Id &&
              b.HairdresserId == bookingRequestDto.HairdresserId &&
              b.Start < end && b.End > bookingRequestDto.Start
            );

            if (!isAvailable)
            {
                throw new InvalidOperationException("Hairdresser is booked at this time.");
            }

            if (bookingRequestDto.Start < DateTime.Now || bookingRequestDto.Start > DateTime.Now.AddMonths(4))
            {
                throw new ArgumentException("Can only book from today and up to 4 month in advance.");
            }
            booking.Id = bookingId;
            booking.CustomerId = customerId;
            booking.HairdresserId = bookingRequestDto.HairdresserId;
            booking.TreatmentId = bookingRequestDto.TreatmentId;
            booking.Start = bookingRequestDto.Start;
            booking.End = end;

            await _bookingRepository.UpdateAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            var updatedBooking = await _bookingRepository.GetByIdWithDetailsAsync(booking.Id, customerId);         

            return updatedBooking.MapToBookingReponse2Dto();

        }
    }
}
