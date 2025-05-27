using System;
using Hairdresser.DTOs.User;
using Hairdresser.Enums;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<HairdresserResponseDTO?> GetHairdressersWithBookings(string userId);
    Task<UserDto?> RegisterUserAsync(RegisterUserDto registerUserDto, UserRoleEnum userRole);
    Task<IEnumerable<UserDto?>> GetALLHairdressersAsync();
}
