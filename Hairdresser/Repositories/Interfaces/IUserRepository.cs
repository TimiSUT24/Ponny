using System;
using Hairdresser.DTOs;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetByIdAsync(Guid id);
    Task<HairdresserRespondDTO?> GetHairdressersWithBookings(string userId);
}
