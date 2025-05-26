using System;
using Hairdresser.DTOs.User;
using HairdresserClassLibrary.Models;

namespace Hairdresser.DTOs;

public class BookingResponseDto 
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public Treatment Treatment { get; set; } = null!;
    public UserDTO UserDto { get; set; } = null!;
    public ApplicationUser Hairdresser { get; set; } = null!;
}
