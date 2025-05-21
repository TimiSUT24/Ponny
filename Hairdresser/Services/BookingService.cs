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
		private readonly IGenericRepository<Booking> _repository;
        private readonly IGenericRepository<Treatment> _treatmentRepository;

        public BookingService(IGenericRepository<Booking> booking, IGenericRepository<Treatment> treatment)
		{
			_repository = booking;
            _treatmentRepository = treatment; 
		}

		public async Task<List<DateTime>> GetAllAvailableTimes(string hairdresserId, int treatmentId, DateTime day)
		{
            var treatment = await _treatmentRepository.GetByIdAsync(treatmentId);
            if (treatment == null)
            {
                throw new Exception("Behandling hittades inte");
            }
            var startOfDay = day.Date.AddHours(9); // frisör jobbar från 09:00
            var endOfDay = day.Date.AddHours(17);  // till 17:00
            var duration = TimeSpan.FromMinutes(treatment.Duration);

            // Hämta bokade tider
            var bookings = await _repository
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

        public async Task<BookingRequestDto> BookAppointment(string customerId, BookingRequestDto request)
        {
            var treatment = await _treatmentRepository.GetByIdAsync(request.TreatmentId);
            if (treatment == null)
            {
                throw new Exception("Behandling hittades inte.");
            }               

            var end = request.Start.AddMinutes(treatment.Duration);

            // Kontrollera om frisören är upptagen
            bool isAvailable = !await _repository.AnyAsync(b =>
                b.HairdresserId == request.HairdresserId &&
                b.Start < end && b.End > request.Start
            );

            if (!isAvailable)
            {
                throw new Exception("Frisören är upptagen vid denna tid.");
            }
                

            var booking = new Booking
            {
                CustomerId = customerId,
                HairdresserId = request.HairdresserId,
                TreatmentId = request.TreatmentId,
                Start = request.Start,
                End = end
            };

            await _repository.AddAsync(booking);
            await _repository.SaveChangesAsync();

            return new BookingRequestDto
            {
               
                HairdresserId = booking.HairdresserId,
                TreatmentId = booking.TreatmentId,
                Start = booking.Start,     
              
            };
        }

        public async Task<BookingRequestDto> CancelBooking(string customerId, int bookingId)
        {
            var booking = await _repository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                throw new Exception("Bokningen hittades inte.");
            }
                

            if (booking.CustomerId != customerId)
            {
                throw new Exception("Du kan bara avboka dina egna tider.");
            }
               

            await _repository.DeleteAsync(booking);
            await _repository.SaveChangesAsync();          

            return new BookingRequestDto
            {                
              // Id = booking.Id,
               Start = booking.Start,
               TreatmentId = booking.TreatmentId
            };
        }
	}
}
