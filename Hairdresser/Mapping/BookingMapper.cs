using Hairdresser.DTOs;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Mapping;

public static class BookingMapper
{
    public static HairdresserBookingRespondDTO MapToBookingResponseDto(this Booking booking)
    {
        ArgumentNullException.ThrowIfNull(booking);

        return new HairdresserBookingRespondDTO
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
}
