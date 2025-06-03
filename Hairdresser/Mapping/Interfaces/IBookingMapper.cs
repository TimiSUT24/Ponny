
using HairdresserClassLibrary.DTOs;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Mapping.Interfaces
{
    public interface IBookingMapper
    {
        HairdresserBookingRespondDto MapToBookingResponseDto(Booking booking); 
        BookingResponseDto MapToBookingReponse2Dto(Booking booking);

        BookingDto MapToBookingDto(Booking booking);


    }
}
