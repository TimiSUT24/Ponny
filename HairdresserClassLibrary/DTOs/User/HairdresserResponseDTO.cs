using Hairdresser.DTOs;
using Hairdresser.DTOs.User;

namespace HairdresserClassLibrary.DTOs.User;

public record HairdresserResponseDto : UserDto
{
    public ICollection<HairdresserBookingRespondDto> Bookings { get; set; } = [];
}
