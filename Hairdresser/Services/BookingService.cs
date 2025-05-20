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
	}
}
