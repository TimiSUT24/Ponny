namespace Hairdresser.DTOs.User;

public record HairdresserResponseDto : UserDto
{
    public ICollection<HairdresserBookingRespondDto> Bookings { get; set; } = [];
}
