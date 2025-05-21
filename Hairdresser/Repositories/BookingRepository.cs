using Hairdresser.Data;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hairdresser.Repositories
{
	public class BookingRepository : IBookingRepository
	{
		private readonly ApplicationDBContext _context;
		public BookingRepository(ApplicationDBContext context)
		{
			_context = context;
		}
		public async Task AddAsync(Booking entity)
		{
			await _context.Bookings.AddAsync(entity);
		}

		public async Task DeleteAsync(Booking entity)
		{
			_context.Bookings.Remove(entity);
		}

		public async Task<IEnumerable<Booking>> FindAsync(Expression<Func<Booking, bool>> predicate)
		{
			return await _context.Bookings.Where(predicate).ToListAsync();
		}

		public async Task<IEnumerable<Booking>> GetAllAsync()
		{
			return await _context.Bookings.ToListAsync();
		}

		public async Task<Booking?> GetByIdAsync(int id)
		{
			return await _context.Bookings.FindAsync(id);
		}

		public async Task UpdateAsync(Booking entity)
		{
			_context.Bookings.Update(entity);
		}
		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

		public async Task<bool> AnyAsync(Expression<Func<Booking, bool>> predicate)
        {
            return await _context.Bookings.AnyAsync(predicate);
        }

        public async Task<Booking?> GetByIdWithDetailsAsync(int id, string customerId)
        {
            return await _context.Bookings
                .Include(b => b.Treatment)
                .Include(b => b.Customer)
                .Include(b => b.Hairdresser)
                .FirstOrDefaultAsync(b => b.Id == id && b.CustomerId == customerId);
        }
    }
}
