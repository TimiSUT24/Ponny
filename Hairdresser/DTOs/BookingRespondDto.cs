using System;
using HairdresserClassLibrary.Models;

namespace Hairdresser.DTOs;

public class BookingRespondDto
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public Treatment Treatment { get; set; } = null!;
    public ApplicationUser Customer { get; set; } = null!;
}
