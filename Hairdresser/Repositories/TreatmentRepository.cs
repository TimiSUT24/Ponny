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

		public TreatmentRepository(ApplicationDBContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Treatment>> GetAllAsync()
		{
			return await _context.Treatments.ToListAsync();
		}

		public async Task<Treatment?> GetByIdAsync(int id)
		{
			return await _context.Treatments.FindAsync(id);
		}

		public async Task<IEnumerable<Treatment>> FindAsync(Expression<Func<Treatment, bool>> predicate)
		{
			return await _context.Treatments.Where(predicate).ToListAsync();
		}

		public async Task AddAsync(Treatment entity)
		{
			await _context.Treatments.AddAsync(entity);
		}

		public async Task UpdateAsync(Treatment entity)
		{
			_context.Treatments.Update(entity);
		}

		public async Task DeleteAsync(Treatment entity)
		{
			_context.Treatments.Remove(entity);
		}
		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

	}
}
