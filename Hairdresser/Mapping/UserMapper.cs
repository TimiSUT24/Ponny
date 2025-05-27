using System;
using Hairdresser.DTOs.User;
using HairdresserClassLibrary.Models;
using Hairdresser.DTOs;

namespace Hairdresser.Mapping;

public static class UserMapper
{
    public static HairdresserResponseDto MapToHairdresserWithBookingsRespondDTO(this ApplicationUser userDto)
    {
        ArgumentNullException.ThrowIfNull(userDto);

        return new HairdresserResponseDto
        {
            Id = userDto.Id,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email ?? string.Empty,
            PhoneNumber = userDto.PhoneNumber ?? string.Empty,
            Bookings = userDto.HairdresserBookings.Select(booking => booking.MapToBookingResponseDto()).ToList()
        };
    }
    public static UserDto MapToUserDTO(this ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserDto
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
