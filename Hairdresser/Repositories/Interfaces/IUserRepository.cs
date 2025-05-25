using System;
using Hairdresser.DTOs.User;
using Hairdresser.Enums;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<HairdresserRespondDTO?> GetHairdressersWithBookings(string userId);
    Task<UserDTO?> RigisterUserAsync(RegisterUserDto registerUserDto, UserRoleEnum userRole);
}
