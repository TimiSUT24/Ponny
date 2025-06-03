using HairdresserClassLibrary.Models;
using HairdresserClassLibrary.DTOs;
using HairdresserClassLibrary.DTOs.User;

namespace Hairdresser.Mapping;

public class BookingMapper : IBookingMapper
{
    public HairdresserBookingRespondDto MapToBookingResponseDto(Booking booking)
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
    public BookingDto MapToBookingDto(Booking booking)
    {
        ArgumentNullException.ThrowIfNull(booking);

        return new BookingDto
        {
            Id = booking.Id,
            Start = booking.Start,
            End = booking.End,
        };
    }

    public BookingResponseDto MapToBookingReponse2Dto(Booking booking)
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
