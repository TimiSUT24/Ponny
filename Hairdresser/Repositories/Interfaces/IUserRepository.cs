using System;
using Hairdresser.Enums;
using HairdresserClassLibrary.DTOs.User;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<HairdresserResponseDto?> GetHairdressersWithBookings(string userId);
    Task<UserDto?> RegisterUserAsync(RegisterUserDto registerUserDto, UserRoleEnum userRole);
    Task<IEnumerable<UserDto?>> GetALLHairdressersAsync();
}
