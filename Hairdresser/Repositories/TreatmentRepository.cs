using Hairdresser.Data;
using HairdresserClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Hairdresser.Repositories.Interfaces;
namespace Hairdresser.Repositories
{
	public class TreatmentRepository : IGenericRepository<Treatment>
	{
		private readonly ApplicationDBContext _context;
		private readonly DbSet<Treatment> _dbSet;

		public TreatmentRepository(ApplicationDBContext context)
		{
			_context = context;
			_dbSet = context.Set<Treatment>();
		}

		public async Task<IEnumerable<Treatment>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public async Task<Treatment?> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task<IEnumerable<Treatment>> FindAsync(Expression<Func<Treatment, bool>> predicate)
		{
			return await _dbSet.Where(predicate).ToListAsync();
		}

		public async Task AddAsync(Treatment entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public async Task UpdateAsync(Treatment entity)
		{
			_dbSet.Update(entity);
		}

		public async Task DeleteAsync(Treatment entity)
		{
			_dbSet.Remove(entity);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
