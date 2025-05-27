namespace Hairdresser.DTOs.User;

public record HairdresserResponseDTO : UserDto
{
    public ICollection<HairdresserBookingRespondDTO> Bookings { get; set; } = [];
}
