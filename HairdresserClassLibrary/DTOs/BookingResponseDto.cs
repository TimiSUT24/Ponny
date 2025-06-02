using System;
using Hairdresser.DTOs.User;
using HairdresserClassLibrary.Models;

namespace Hairdresser.DTOs;

public class BookingResponseDto
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public TreatmentDto Treatment { get; set; } = null!;
    public UserDto Costumer { get; set; } = null!;
    public UserDto Hairdresser { get; set; } = null!;
}
