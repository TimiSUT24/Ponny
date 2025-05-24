using System;
using Hairdresser.DTOs;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Mapping;

public static class UserMapper
{
    public static HairdresserRespondDTO MapToHairdresserWithBookingsRespondDTO(this ApplicationUser userDto)
    {
        ArgumentNullException.ThrowIfNull(userDto);

        return new HairdresserRespondDTO
        {
            Id = userDto.Id,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email ?? string.Empty,
            PhoneNumber = userDto.PhoneNumber ?? string.Empty,
            Bookings = userDto.HairdresserBookings.Select(booking => booking.MapToBookingResponseDto()).ToList()
        };
    }
    public static UserDTO MapToUserDTO(this ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserDTO
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
        };
    }
}
