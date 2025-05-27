using System;

namespace Hairdresser.DTOs;

public class HairdresserBookingRespondDTO
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public TreatmentDTO Treatment { get; set; } = null!;
}
