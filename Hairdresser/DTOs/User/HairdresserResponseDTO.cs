namespace Hairdresser.DTOs.User;

public record HairdresserResponseDTO : UserDTO
{
    public ICollection<HairdresserBookingRespondDTO> Bookings { get; set; } = [];
}
