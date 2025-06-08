using HairdresserClassLibrary.Models;
using HairdresserClassLibrary.DTOs;
using HairdresserClassLibrary.DTOs.User;

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
            Customer = new UserDto
            {
                Id = booking.Customer.Id,
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
                Id = booking.Hairdresser.Id,
                FirstName = booking.Hairdresser.FirstName,
                LastName = booking.Hairdresser.LastName,
                UserName = booking.Hairdresser.UserName,
                Email = booking.Hairdresser.Email,
                PhoneNumber = booking.Hairdresser.PhoneNumber
            }
        };
    }
}
