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
                throw new Exception("Treatment was not found");
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
                throw new Exception("Treatment was not found.");
            }               

            var end = request.Start.AddMinutes(treatment.Duration);

            // check if hairdresser is booked 
            bool isAvailable = !await _bookingRepository.AnyAsync(b =>
                b.HairdresserId == request.HairdresserId &&
                b.Start < end && b.End > request.Start
            );

            if (!isAvailable)
            {
                throw new Exception("Hairdresser is booked at this time.");
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
                throw new Exception("Can only book from today and up to 4 month in advance.");
            }

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            return new BookingResponseDto
            {
                Id = booking.Id,
                Start = booking.Start,     
              
            };
        }

        public async Task<BookingRequestDto> CancelBooking(string customerId, int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                throw new Exception("Booking was not found.");
            }
                

            if (booking.CustomerId != customerId)
            {
                throw new Exception("Can only cancel your own bookings.");
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
                throw new Exception("Booking was not found.");
            }

            if (booking.CustomerId != customerId)
            {
                throw new Exception("Can only see your own bookings.");
            }

            return new BookingResponseDto
            {
                Id = booking.Id,
                Customer = booking.Customer,
            };

        }
	}
}
