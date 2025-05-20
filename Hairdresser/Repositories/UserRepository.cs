using Hairdresser.Data;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hairdresser.Repositories
{
	public class UserRepository :IGenericRepository<ApplicationUser>
	{
		private readonly ApplicationDBContext _context;
		public UserRepository(ApplicationDBContext context)
		{
			_context = context;
		}
		public async Task AddAsync(ApplicationUser entity)
		{
			await _context.Users.AddAsync(entity);
		}

		public async Task DeleteAsync(ApplicationUser entity)
		{
			_context.Users.Remove(entity);
		}

		public async Task<IEnumerable<ApplicationUser>> FindAsync(Expression<Func<ApplicationUser, bool>> predicate)
		{
			return await _context.Users.Where(predicate).ToListAsync();
		}

		public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<ApplicationUser?> GetByIdAsync(int id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task UpdateAsync(ApplicationUser entity)
		{
			_context.Users.Update(entity);
		}
		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

        public async Task<bool> AnyAsync(Expression<Func<Booking, bool>> predicate)
        {
            return await _context.Bookings.AnyAsync(predicate);
        }
    }
}
