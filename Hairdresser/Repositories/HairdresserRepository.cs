using HairdresserClassLibrary.Models;
using Hairdresser.Data;
using Hairdresser.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Repositories
{
    public class HairdresserRepository : IHairdresserRepository
    {
        private readonly ApplicationDBContext _context;

        public HairdresserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            var hairdresserRoleId = await _context.Roles
                .Where(r => r.Name == "Hairdresser")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            return await (from user in _context.Users
                          join userRole in _context.UserRoles on user.Id equals userRole.UserId
                          where userRole.RoleId == hairdresserRoleId
                          select user).ToListAsync();
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            var hairdresserRoleId = await _context.Roles
                .Where(r => r.Name == "Hairdresser")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            return await (from user in _context.Users
                          join userRole in _context.UserRoles on user.Id equals userRole.UserId
                          where user.Id == id && userRole.RoleId == hairdresserRoleId
                          select user).FirstOrDefaultAsync();
        }
    }
}