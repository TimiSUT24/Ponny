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
            Costumer = new UserDto
            {
                Id = booking.CustomerId,
                UserName = booking.Customer.UserName,
                Email = booking.Customer.Email,
                PhoneNumber = booking.Customer.PhoneNumber
            },
            Treatment = new TreatmentDto
            {
                Name = booking.Treatment.Name,
                Description = booking.Treatment.Description,
                Duration = booking.Treatment.Duration,
                Price = booking.Treatment.Price,       
            },
            Hairdresser = new UserDto
            {
                UserName = booking.Hairdresser.UserName,
                Email = booking.Hairdresser.Email,
                PhoneNumber = booking.Hairdresser.PhoneNumber
            }
        };
    }
}
