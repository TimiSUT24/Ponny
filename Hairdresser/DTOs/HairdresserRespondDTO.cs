
namespace Hairdresser.DTOs;

public record HairdresserRespondDTO : UserDTO
{
    public ICollection<HairdresserBookingRespondDTO> Bookings { get; set; } = [];
}
