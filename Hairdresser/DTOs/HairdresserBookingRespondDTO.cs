using System;

namespace Hairdresser.DTOs;

public class HairdresserBookingRespondDto
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public TreatmentDto Treatment { get; set; } = null!;
    public UserDto Customer { get; set; } = null!; 
}
