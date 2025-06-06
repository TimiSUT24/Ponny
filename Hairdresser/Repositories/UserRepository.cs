using Hairdresser.Data;
using Hairdresser.Enums;
using Hairdresser.Mapping;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.DTOs.User;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hairdresser.Repositories
{
	public class UserRepository(ApplicationDBContext context, UserManager<ApplicationUser> userManager) : IGenericRepository<ApplicationUser>, IUserRepository
	{
		private readonly ApplicationDBContext _context = context;
		private readonly UserManager<ApplicationUser> _userManager = userManager;

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
			await _userManager.UpdateAsync(entity);
		}
		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

		public async Task<ApplicationUser?> GetByIdAsync(string id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<HairdresserResponseDto?> GetHairdressersWithBookings(string userId)
		{
			ArgumentNullException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

			return await _context.Users
							.Where(user => user.Id == userId)
							.Include(u => u.CustomerBookings)
								.ThenInclude(b => b.Treatment)
							.Select(user => user.MapToHairdresserWithBookingsRespondDTO())
							.FirstOrDefaultAsync();
		}

		public async Task<UserDto?> RegisterUserAsync(RegisterUserDto registerUserDto, UserRoleEnum userRole)
		{
			ArgumentNullException.ThrowIfNull(registerUserDto, nameof(registerUserDto));

			var existingUser = await _userManager.FindByNameAsync(registerUserDto.UserName);
			if (existingUser != null)
			{
				return null; // Username already taken
			}
			var user = new ApplicationUser
			{
				FirstName = registerUserDto.FirstName,
				LastName = registerUserDto.LastName,
				UserName = registerUserDto.UserName,
				Email = registerUserDto.Email,
				PhoneNumber = registerUserDto.PhoneNumber
			};
			var result = await _userManager.CreateAsync(user, registerUserDto.Password);

			if (!result.Succeeded)
			{
				return null;
			}

			await _userManager.AddToRoleAsync(user, userRole.ToString());
			return user.MapToUserDTO();
		}

		public Task<bool> AnyAsync(Expression<Func<ApplicationUser, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<UserDto?>> GetAllHairdressersAsync()
		{
			var hairdresserRoleId = await _context.Roles
				.Where(r => r.Name == "Hairdresser")
				.Select(r => r.Id)
				.FirstOrDefaultAsync();

			return await (from user in _context.Users
						  join userRole in _context.UserRoles on user.Id equals userRole.UserId
						  where userRole.RoleId == hairdresserRoleId
						  select user).Select(user => new UserDto
						  {
							  Id = user.Id,
							  FirstName = user.FirstName,
							  LastName = user.LastName,
							  UserName = user.UserName,
							  Email = user.Email,
							  PhoneNumber = user.PhoneNumber
						  }).ToListAsync();
		}
	}
}
