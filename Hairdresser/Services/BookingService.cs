using Azure.Core;
using Hairdresser.DTOs;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Services
{
	public class BookingService : IBookingService
	{
        private readonly IBookingRepository _bookingRepository;       
        private readonly IGenericRepository<Treatment> _treatmentRepository;

        public BookingService(IGenericRepository<Treatment> treatment, IBookingRepository bookingRepository)
		{			
            _treatmentRepository = treatment;
            _bookingRepository = bookingRepository;           
        }

		public async Task<List<DateTime>> GetAllAvailableTimes(string hairdresserId, int treatmentId, DateTime day)
		{
            var treatment = await _treatmentRepository.GetByIdAsync(treatmentId);
            if (treatment == null)
            {
                throw new KeyNotFoundException("Treatment was not found");
            }
            var startOfDay = day.Date.AddHours(9); // frisör jobbar från 09:00
            var endOfDay = day.Date.AddHours(17);  // till 17:00
            var duration = TimeSpan.FromMinutes(treatment.Duration);

            // Hämta bokade tider
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
            

            var booking = new Booking
            {
                CustomerId = customerId,
                HairdresserId = request.HairdresserId,
                TreatmentId = request.TreatmentId,
                Start = request.Start,
                End = end
            };           

            if(booking.Start < DateTime.Now ||  booking.Start > DateTime.Now.AddMonths(4))
            {
                throw new ArgumentException("Can only book from today and up to 4 month in advance.");
            }

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            var savedBooking = await _bookingRepository.GetByIdWithDetailsAsync(booking.Id,customerId);

            return new BookingResponseDto
            {
                Id = booking.Id,
                Start = booking.Start,  
                End = booking.End,
                UserDto = new UserDto
                {
                    Id = savedBooking.CustomerId,
                    UserName =  savedBooking.Customer.UserName,
                    Email = savedBooking.Customer.Email,
                    PhoneNumber = savedBooking.Customer.PhoneNumber
                },

            };
        }

        public async Task<BookingRequestDto> CancelBooking(string customerId, int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                throw new KeyNotFoundException("Booking was not found.");
            }
                

            if (booking.CustomerId != customerId)
            {
                throw new UnauthorizedAccessException("Can only cancel your own bookings.");
            }
               

            await _bookingRepository.DeleteAsync(booking);
            await _bookingRepository.SaveChangesAsync();          

            return new BookingRequestDto
            {                
              // Id = booking.Id,
               Start = booking.Start,
               TreatmentId = booking.TreatmentId
            };
        }

        public async Task<BookingResponseDto> GetBookingByIdAsync(int bookingId, string customerId)
        {
            var booking = await _bookingRepository.GetByIdWithDetailsAsync(bookingId,customerId);
            if (booking == null)
            {
                throw new KeyNotFoundException("Booking was not found.");
            }

            if (booking.CustomerId != customerId)
            {
                throw new UnauthorizedAccessException("Can only see your own bookings.");
            }

            return new BookingResponseDto
            {
                Id = booking.Id,
                //Customer = booking.Customer,
            };

        }

        public async Task<BookingResponseDto> RebookBooking(string customerId, int bookingId, BookingRequestDto bookingRequestDto)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if(booking == null)
            {
                throw new KeyNotFoundException("Booking was not found.");
            }

            if (booking.CustomerId != customerId)
            {
                throw new UnauthorizedAccessException("Can only update your own bookings.");
            }

            var treatment = await _treatmentRepository.GetByIdAsync(booking.TreatmentId);
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
            booking.Id = bookingId;
            booking.CustomerId = customerId;
            booking.HairdresserId = bookingRequestDto.HairdresserId;
            booking.TreatmentId = bookingRequestDto.TreatmentId;
            booking.Start = bookingRequestDto.Start;
            booking.End = end;

            await _bookingRepository.UpdateAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            var updatedBooking = await _bookingRepository.GetByIdWithDetailsAsync(booking.Id, customerId);

            return new BookingResponseDto
            {
                Id = booking.Id,
                Start = booking.Start,
                End = booking.End,
                UserDto = new UserDto
                {
                    Id = updatedBooking.CustomerId,
                    UserName = updatedBooking.Customer.UserName,
                    Email = updatedBooking.Customer.Email,
                    PhoneNumber = updatedBooking.Customer.PhoneNumber
                }
            };

        }
	}
}
