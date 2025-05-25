namespace Hairdresser.DTOs.User;

public record HairdresserRespondDTO : UserDTO
{
    public ICollection<HairdresserBookingRespondDTO> Bookings { get; set; } = [];
}
