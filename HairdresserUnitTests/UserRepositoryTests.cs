using Hairdresser.Data;
using Hairdresser.DTOs.User;
using Hairdresser.Enums;
using Hairdresser.Repositories;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairdresserUnitTests
{
	[TestClass]
	public class UserRepositoryTests
	{
		private ApplicationDBContext? _context;
		private Mock<UserManager<ApplicationUser>> _userManagerMock = null!;
		private UserRepository _userRepository = null!;

		[TestInitialize]
		public void Setup()
		{
			// Set up in-memory database
			var options = new DbContextOptionsBuilder<ApplicationDBContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			var store = new Mock<IUserStore<ApplicationUser>>();
			store.As<IUserEmailStore<ApplicationUser>>(); // Required for FindByEmailAsync

			_userManagerMock = new Mock<UserManager<ApplicationUser>>(
				store.Object, null!, null!, null!, null!, null!, null!, null!, null!
			);

			_context = new ApplicationDBContext(options);
			_userRepository = new UserRepository(_context, _userManagerMock.Object);
		}

		[TestMethod]
		public async Task AddAsync_ShouldAddUser()
		{
			var user = new ApplicationUser { Id = "1", UserName = "testuser" };

			await _userRepository.AddAsync(user);
			await _userRepository.SaveChangesAsync();

			var users = await _userRepository.GetAllAsync();
			Assert.AreEqual(1, users.Count());
			Assert.AreEqual("testuser", users.First().UserName);
		}

		[TestMethod]
		public async Task DeleteAsync_ShouldRemoveUser()
		{
			var user = new ApplicationUser { Id = "1", UserName = "todelete" };
			await _context!.Users.AddAsync(user);
			await _context.SaveChangesAsync();

			await _userRepository.DeleteAsync(user);
			await _userRepository.SaveChangesAsync();

			var users = await _userRepository.GetAllAsync();
			Assert.AreEqual(0, users.Count());
		}

		[TestMethod]
		public async Task GetByIdAsync_ShouldReturnUser()
		{
			var user = new ApplicationUser { Id = "123", UserName = "byid" };
			await _context!.Users.AddAsync(user);
			await _context.SaveChangesAsync();

			var result = await _userRepository.GetByIdAsync("123");
			Assert.IsNotNull(result);
			Assert.AreEqual("byid", result!.UserName);
		}

		[TestMethod]
		public async Task FindAsync_ShouldReturnMatchingUsers()
		{
			await _context.Users.AddRangeAsync(
				new ApplicationUser { Id = "1", UserName = "match1" },
				new ApplicationUser { Id = "2", UserName = "match2" },
				new ApplicationUser { Id = "3", UserName = "other" }
			);
			await _context.SaveChangesAsync();

			var result = await _userRepository.FindAsync(u => u.UserName!.Contains("match"));
			Assert.AreEqual(2, result.Count());
		}

		[TestMethod]
		public async Task RegisterUserAsync_InvalidPassword_ShouldNotCreate()
		{
			//this test checks if the user registration fails when the user manager returns an error
			var dto = new RegisterUserDto
			{
				FirstName = "Fail",
				LastName = "Case",
				UserName = "failcase",
				Email = "fail@example.com",
				PhoneNumber = "000000000",
				Password = "fail"
			};

			_userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
				.ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failure" }));

			var result = await _userRepository.RegisterUserAsync(dto, UserRoleEnum.User);

			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task RegisterUserAsync_InvalidEmail_ShouldNotCreate()
		{
			//this test checks if the user registration fails when the user manager returns an error
			var dto = new RegisterUserDto
			{
				FirstName = "Fail",
				LastName = "Case",
				UserName = "failcase",
				Email = "fail",
				PhoneNumber = "000000000",
				Password = "faiKqh2fdf(266!!1l"
			};

			_userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
				.ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failure" }));

			var result = await _userRepository.RegisterUserAsync(dto, UserRoleEnum.User);
			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task RegisterUserAsync_ShouldReturn_UserDto()
		{
			//this test checks if the user registration sucsedes when the user manager returns an Success
			var dto = new RegisterUserDto
			{
				FirstName = "Success",
				LastName = "Case",
				UserName = "successcase",
				Email = "success@example.com",
				Password = "Password123!"
			};

			_userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
				.ReturnsAsync(IdentityResult.Success);

			var result = await _userRepository.RegisterUserAsync(dto, UserRoleEnum.User);

			Assert.IsNotNull(result);
			Assert.AreEqual(dto.FirstName, result!.FirstName);
		}
		[TestMethod]	
		public async Task GetHairdressersWithBookings_ShouldReturn_Hairdressers()
		{
			_context.Users.AddRange(
				new ApplicationUser { Id = "1", UserName = "hairdresser1"}
			);
			_context.Bookings.AddRange(
				new Booking { Id = 1, HairdresserId = "1", TreatmentId = 1 },
				new Booking { Id = 2, HairdresserId = "1", TreatmentId = 2 }
			);
		}
	}
}
