using Hairdresser.DTOs;
using Hairdresser.DTOs.User;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Mapping;

public static class BookingMapper
{
    public static HairdresserBookingRespondDto MapToBookingResponseDto(this Booking booking)
    {
        ArgumentNullException.ThrowIfNull(booking);

        return new HairdresserBookingRespondDto
        {
            Id = booking.Id,
            Start = booking.Start,
            End = booking.End,
            Treatment = booking.Treatment.MapToTreatmentDto()
        };
    }
    public static BookingDto MapToBookingDto(this Booking booking)
    {
        ArgumentNullException.ThrowIfNull(booking);

        return new BookingDto
        {
            Id = booking.Id,
            Start = booking.Start,
            End = booking.End,
        };
    }

    public static BookingResponseDto MapToBookingReponse2Dto(this Booking booking)
    {
        ArgumentNullException.ThrowIfNull(booking);

        return new BookingResponseDto
        {
            Id = booking.Id,
            Start = booking.Start,
            End = booking.End,
            Costumer = booking.Customer.MapToUserDTO(),
            Treatment = booking.Treatment.MapToTreatmentDto(),
            Hairdresser = booking.Hairdresser.MapToUserDTO()
        };
    }
    public static Booking MapToBookingFromBookingResponseDto(this BookingResponseDto bookingResponseDto)
    {
        ArgumentNullException.ThrowIfNull(bookingResponseDto);

        return new Booking
        {
            Id = bookingResponseDto.Id,
            Start = bookingResponseDto.Start,
            End = bookingResponseDto.End,
            Customer = new ApplicationUser
            {
                Id = bookingResponseDto.Costumer.Id,
                FirstName = bookingResponseDto.Costumer.FirstName,
                LastName = bookingResponseDto.Costumer.LastName,
                UserName = bookingResponseDto.Costumer.UserName,
                Email = bookingResponseDto.Costumer.Email,
                PhoneNumber = bookingResponseDto.Costumer.PhoneNumber

            },
            Hairdresser = new ApplicationUser
            {
                Id = bookingResponseDto.Hairdresser.Id,
                FirstName = bookingResponseDto.Hairdresser.FirstName,
                LastName = bookingResponseDto.Hairdresser.LastName,
                UserName = bookingResponseDto.Hairdresser.UserName,
                Email = bookingResponseDto.Hairdresser.Email,
                PhoneNumber = bookingResponseDto.Hairdresser.PhoneNumber
            },
            Treatment = new Treatment
            {
                Id = bookingResponseDto.Treatment.Id,
                Name = bookingResponseDto.Treatment.Name,
                Duration = bookingResponseDto.Treatment.Duration
            }
        };
    }
}
