﻿using Azure.Core;
using Hairdresser.DTOs;
using Hairdresser.DTOs.User;
using Hairdresser.Mapping;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            if (booking.Start < DateTime.Now || booking.Start > DateTime.Now.AddMonths(4))
            {
                throw new ArgumentException("Can only book from today and up to 4 month in advance.");
            }

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            var savedBooking = await _bookingRepository.GetByIdWithDetailsAsync(booking.Id, customerId);

            var mapp = BookingMapper.MapToBookingReponse2Dto(savedBooking);
            return mapp;            
        }

        public async Task<BookingDto> CancelBooking(string customerId, int bookingId)
        {
            var booking = await _bookingRepository.GetByIdWithDetailsAsync(bookingId,customerId);

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
                throw new UnauthorizedAccessException("Can only see your own bookings.");
            }

            var currentBooking = BookingMapper.MapToBookingReponse2Dto(booking);
            return currentBooking; 
        }

        public async Task<BookingResponseDto> RebookBooking(string customerId, int bookingId, BookingRequestDto bookingRequestDto)
        {
            var booking = await _bookingRepository.GetByIdWithDetailsAsync(bookingId,customerId);

            if (booking == null)
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

            var returnUpdatedBooking = BookingMapper.MapToBookingReponse2Dto(updatedBooking);

            return returnUpdatedBooking; 

        }
    }
}
