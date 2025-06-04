using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services;
using HairdresserClassLibrary.DTOs;
using HairdresserClassLibrary.DTOs.User;
using HairdresserClassLibrary.Models;
using HairdresserUnitTests.utils;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace HairdresserUnitTests;

[TestClass]
public class UserServiceTest
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock = null!;
    private Mock<IUserRepository> _userRepository = null!;
    private Mock<IBookingRepository> _bookingRepository = null!;
    private UserService _serviceMock = null!;



    [TestInitialize]
    public void Setup()
    {
        _userManagerMock = MockUser.InitializeUserManager();
        _userRepository = new Mock<IUserRepository>();
        _bookingRepository = new Mock<IBookingRepository>();
        _serviceMock = new UserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);
    }



    [TestMethod]
    public async Task GetAllHairdressersAsync_ShouldReturnHairdressers()
    {
        // Arrange - Mocking the user repository to return a list of hairdressers
        var users = new List<ApplicationUser>
        {
            new ApplicationUser { Id = "1", UserName = "hairdresser1", Email = "Jon.Doe@exampel.com", PhoneNumber = "1234567890" },
            new ApplicationUser { Id = "2", UserName = "hairdresser2", Email = "Jan.Doe@exampel.com", PhoneNumber = "0987654321" }
        };

        _userRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(users);

        // Act - Call the method to get all hairdressers

        var result = await _serviceMock.GetAllHairdressersAsync();
        var expected = 2;

        // Assert - Check if the result is not null and contains the expected number of hairdressers
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());
    }

    [TestMethod]
    public async Task GetWeekScheduleAsync_ShouldReturnBookingsForWeek()
    {
        // Arrange - Mocking the booking repository to return a list of bookings for a specific hairdresser
        var user = new ApplicationUser { UserName = "hairdresser1", Email = "Jon.Doe@exampel.com", PhoneNumber = "1234567890" };
        var treatment = new Treatment { Id = 1, Name = "Haircut", Price = 20, Description = "Basic haircut", Duration = 60 };

        var Bookings = new List<Booking>
        {
            new Booking { Id = 1, Start = DateTime.Now, End = DateTime.Now.AddHours(1), Customer = user, Hairdresser = user, Treatment = treatment },
            new Booking { Id = 2, Start = DateTime.Now.AddDays(1), End = DateTime.Now.AddDays(1).AddHours(1), Customer = user, Hairdresser = user, Treatment = treatment }
        };
        _bookingRepository
            .Setup(repo => repo.GetWeekScheduleWithDetailsAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(Bookings);

        // Act - Call the method to get the week schedule for a specific hairdresser
        var result = await _serviceMock.GetWeekScheduleAsync("hairdresser1", DateTime.Now);
        var expected = 2;

        // Assert - Check if the result is not null and contains the expected number of bookings
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());

    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("  ")]
    [DataRow(null)]
    public async Task GetWeekScheduleAsync_GetEmptyHairdresserId_ReturnsEmptyList(string hairdresserId)
    {
        // Arrange - Mocking the booking repository to return a list of bookings for a specific hairdresser
        var user = new ApplicationUser { UserName = "hairdresser1", Email = "Jon.Doe@exampel.com", PhoneNumber = "1234567890" };
        var treatment = new Treatment { Id = 1, Name = "Haircut", Price = 20, Description = "Basic haircut", Duration = 60 };

        var Bookings = new List<Booking>
        {
            new Booking { Id = 1, Start = DateTime.Now, End = DateTime.Now.AddHours(1), Customer = user, Hairdresser = user, Treatment = treatment },
            new Booking { Id = 2, Start = DateTime.Now.AddDays(1), End = DateTime.Now.AddDays(1).AddHours(1), Customer = user, Hairdresser = user, Treatment = treatment }
        };
        _bookingRepository
            .Setup(repo => repo.GetWeekScheduleWithDetailsAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(Bookings);

        // Act & Assert - Call the method to get the week schedule for a specific hairdresser with an empty ID
        var ex = Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _serviceMock.GetHairdresserWithId(hairdresserId));
        Assert.AreEqual("Id cannot be null or whitespace. (Parameter 'id')", ex.Result.Message);
    }
    [TestMethod]
    public async Task GetWeekScheduleAsync_GetDateInPast_ReturnsEmptyList()
    {
        // Arrange - Mocking the booking repository to return a list of bookings for a specific hairdresser
        var user = new ApplicationUser { UserName = "hairdresser1", Email = "Jon.Doe@exampel.com", PhoneNumber = "1234567890" };
        var treatment = new Treatment { Id = 1, Name = "Haircut", Price = 20, Description = "Basic haircut", Duration = 60 };

        var Bookings = new List<Booking>
        {
            new Booking { Id = 1, Start = DateTime.Now, End = DateTime.Now.AddHours(1), Customer = user, Hairdresser = user, Treatment = treatment },
            new Booking { Id = 2, Start = DateTime.Now.AddDays(1), End = DateTime.Now.AddDays(1).AddHours(1), Customer = user, Hairdresser = user, Treatment = treatment }
        };
        _bookingRepository
            .Setup(repo => repo.GetWeekScheduleWithDetailsAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(Bookings);


        // Act - Call the method to get the week schedule for a specific hairdresser with a date in the past
        var result = await _serviceMock.GetWeekScheduleAsync("hairdresser1", DateTime.Now.AddDays(-1));
        var expected = 0;

        // Assert - Check if the result is not null and contains the expected number of bookings
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());
    }
    [TestMethod]
    public async Task GetMonthlyScheduleAsync_ShouldReturnBookingsForMonth()
    {
        // Arrange - Mocking the booking repository to return a list of bookings for a specific hairdresser
        var user = new ApplicationUser { UserName = "hairdresser1", Email = "Jon.Doe@exampel.com", PhoneNumber = "1234567890" };
        var treatment = new Treatment { Id = 1, Name = "Haircut", Price = 20, Description = "Basic haircut", Duration = 60 };

        var Bookings = new List<Booking>
        {
            new Booking { Id = 1, Start = DateTime.Now, End = DateTime.Now.AddHours(1), Customer = user, Hairdresser = user, Treatment = treatment },
            new Booking { Id = 2, Start = DateTime.Now.AddDays(1), End = DateTime.Now.AddDays(1).AddHours(1), Customer = user, Hairdresser = user, Treatment = treatment }
        };
        _bookingRepository
            .Setup(repo => repo.GetMonthlyScheduleWithDetailsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(Bookings);


        // Act - Call the method to get the monthly schedule for a specific hairdresser
        var result = await _serviceMock.GetMonthlyScheduleAsync("hairdresser1", 2025, 1);
        var expected = 2;

        // Assert - Check if the result is not null and contains the expected number of bookings
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());

    }
    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("  ")]
    [DataRow(null)]
    public async Task GetMonthlyScheduleAsync_GetEmptyHairdresserId_ReturnsEmptyList(string hairdresserId)
    {
        // Arrange - Mocking the booking repository to return a list of bookings for a specific hairdresser
        var user = new ApplicationUser { UserName = "hairdresser1", Email = "Jon.Doe@exampel.com", PhoneNumber = "1234567890" };
        var treatment = new Treatment { Id = 1, Name = "Haircut", Price = 20, Description = "Basic haircut", Duration = 60 };

        var Bookings = new List<Booking>
        {
            new Booking { Id = 1, Start = DateTime.Now, End = DateTime.Now.AddHours(1), Customer = user, Hairdresser = user, Treatment = treatment },
            new Booking { Id = 2, Start = DateTime.Now.AddDays(1), End = DateTime.Now.AddDays(1).AddHours(1), Customer = user, Hairdresser = user, Treatment = treatment }
        };
        _bookingRepository
            .Setup(repo => repo.GetMonthlyScheduleWithDetailsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(Bookings);


        // Act & Assert - Call the method to get the monthly schedule for a specific hairdresser with an empty ID
        var ex = Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _serviceMock.GetHairdresserWithId(hairdresserId));
        Assert.AreEqual("Id cannot be null or whitespace. (Parameter 'id')", ex.Result.Message);
    }
    [TestMethod]
    [DataRow(2025, -1)]
    [DataRow(2025, 0)]
    [DataRow(2025, 13)]
    [DataRow(2024, 12)]
    [DataRow(2026, 12)]
    public async Task GetMonthlyScheduleAsync_InvalidDateFilter_ReturnsEmptyList(int year, int month)
    {
        // Arrange - Mocking the booking repository to return a list of bookings for a specific hairdresser
        var user = new ApplicationUser { UserName = "hairdresser1", Email = "Jon.Doe@exampel.com", PhoneNumber = "1234567890" };
        var treatment = new Treatment { Id = 1, Name = "Haircut", Price = 20, Description = "Basic haircut", Duration = 60 };

        var Bookings = new List<Booking>
        {
            new Booking { Id = 1, Start = DateTime.Now, End = DateTime.Now.AddHours(1), Customer = user, Hairdresser = user, Treatment = treatment },
        };
        _bookingRepository
            .Setup(repo => repo.GetMonthlyScheduleWithDetailsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(Bookings);


        // Act - Call the method to get the monthly schedule for a specific hairdresser with an invalid date filter
        var result = await _serviceMock.GetMonthlyScheduleAsync("hairdresser1", year, month);
        var expected = 0;

        // Assert - Check if the result is not null and contains the expected number of bookings
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());
    }
    [TestMethod]
    public async Task GetBookingDetailsAsync_ShouldReturnBookingDetails()
    {
        // Arrange - Mocking the booking repository to return a booking with details
        var user = new UserDto { UserName = "hairdresser1", Email = "Jon.Doe@exampel.com", PhoneNumber = "1234567890" };
        var treatment = new TreatmentDto { Id = 1, Name = "Haircut", Price = 20, Description = "Basic haircut", Duration = 60 };
        var Bookings = new HairdresserBookingRespondDto { Id = 1, Start = DateTime.Now, End = DateTime.Now.AddHours(1), Customer = user, Treatment = treatment };
        _bookingRepository
            .Setup(rep => rep.GetBookingWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(Bookings);

        // Act - Call the method to get booking details
        var result = await _serviceMock.GetBookingDetailsAsync(1);

        // Assert - Check if the result is not null and contains the expected booking details
        Assert.IsNotNull(result);
        Assert.AreEqual(Bookings.Id, result.Id);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public async Task GetBookingDetailsAsync_BookingNotFound_ReturnsNull(int BookingId)
    {
        // Arrange - Mocking the booking repository to return null for a non-existing booking
        _bookingRepository
            .Setup(rep => rep.GetBookingWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(null as HairdresserBookingRespondDto);

        // Act - Call the method to get booking details for a non-existing booking
        var result = await _serviceMock.GetBookingDetailsAsync(BookingId);

        // Assert - Check if the result is null
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task UpdateHairdresserAsync_ShouldUpdateHairdresser()
    {
        // Arrange - Mocking the user manager to return a list of hairdressers
        var GetUsersInRoleAsyncReturnValue = new List<ApplicationUser>
        {
            new ApplicationUser { Id = "1", FirstName = "John", LastName = "Doe", Email = "John.Doe@exampel.com", PhoneNumber = "1234567890", UserName = "JohnDoe" },
        };
        var updateUserDto = new UpdateUserDto { FirstName = "Updated", LastName = "Doe", Email = "John.Doe@exampel.com", PhoneNumber = "1234567890", UserName = "JohnDoe" };

        _userManagerMock
            .Setup(repo => repo.GetUsersInRoleAsync(It.IsAny<string>()))
            .ReturnsAsync(GetUsersInRoleAsyncReturnValue);

        // Act - Call the method to update the hairdresser
        var result = await _serviceMock.UpdateHairdresserAsync("1", updateUserDto);
        var expected = new UserDto
        {
            Id = "1",
            FirstName = "Updated",
            LastName = "Doe",
            Email = "John.Doe@exampel.com",
            PhoneNumber = "1234567890",
            UserName = "JohnDoe"
        };

        // Assert - Check if the result is not null and contains the expected updated hairdresser details
        Assert.IsNotNull(result);
        Assert.AreEqual(expected.Id, result.Id);
        Assert.AreEqual(expected.FirstName, result.FirstName);
        Assert.AreEqual(expected.LastName, result.LastName);
        Assert.AreEqual(expected.Email, result.Email);
        Assert.AreEqual(expected.PhoneNumber, result.PhoneNumber);
        Assert.AreEqual(expected.UserName, result.UserName);
    }


    [TestMethod]
    public async Task UpdateHairdresserAsync_HairdresserNotFound_ReturnsNull()
    {
        // Arrange - Mocking the user manager to return an empty list of hairdressers
        var GetUsersInRoleAsyncReturnValue = new List<ApplicationUser>(); // No users
        var updateUserDto = new UpdateUserDto
        {
            FirstName = "Updated",
            LastName = "Doe",
            Email = "updated.doe@example.com",
            PhoneNumber = "1234567890",
            UserName = "UpdatedDoe"
        };

        _userManagerMock
            .Setup(repo => repo.GetUsersInRoleAsync(It.IsAny<string>()))
            .ReturnsAsync(GetUsersInRoleAsyncReturnValue);


        // Act - Call the method to update the hairdresser with a non-existing ID
        var result = await _serviceMock.UpdateHairdresserAsync("1", updateUserDto);

        // Assert
        Assert.IsNull(result);
        _userRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        _userRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("  ")]
    [DataRow(null)]
    public async Task UpdateHairdresserAsync_EmptyId_ReturnsNull(string hairdresserId)
    {
        // Arrange - Mocking the user manager to return an empty list of hairdressers
        var GetUsersInRoleAsyncReturnValue = new List<ApplicationUser>();

        _userManagerMock
            .Setup(repo => repo.GetUsersInRoleAsync(It.IsAny<string>()))
            .ReturnsAsync(GetUsersInRoleAsyncReturnValue);

        // Act - Call the method to update the hairdresser with an empty ID
        var result = await _serviceMock.UpdateHairdresserAsync(hairdresserId, new UpdateUserDto());

        // Assert - Check if the result is null and no update was attempted to run
        Assert.IsNull(result);
        _userRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        _userRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [TestMethod]
    public async Task GetHairdresserWithId_ShouldReturnHairdresser()
    {
        // Arrange - Mocking the user manager to return a list of hairdressers
        var GetUsersInRoleAsyncReturnValue = new List<ApplicationUser>
        {
            new ApplicationUser { Id = "1", FirstName = "John", LastName = "Doe", Email = "John.Doe@exampel.com", PhoneNumber = "1234567890", UserName = "JohnDoe" },
        };
        _userManagerMock
            .Setup(repo => repo.GetUsersInRoleAsync(It.IsAny<string>()))
            .ReturnsAsync(GetUsersInRoleAsyncReturnValue);

        // Act - Call the method to get a hairdresser by ID
        var result = await _serviceMock.GetHairdresserWithId("1");
        var expected = new UserDto { Id = "1", FirstName = "John", LastName = "Doe", Email = "John.Doe@exampel.com", PhoneNumber = "1234567890", UserName = "JohnDoe" };

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expected.Id, result.Id);
        Assert.AreEqual(expected.FirstName, result.FirstName);
        Assert.AreEqual(expected.LastName, result.LastName);
        Assert.AreEqual(expected.Email, result.Email);
        Assert.AreEqual(expected.PhoneNumber, result.PhoneNumber);
        Assert.AreEqual(expected.UserName, result.UserName);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("  ")]
    [DataRow(null)]
    public void GetHairdresserWithId_EmptyId_throwException(string hairdresserId)
    {
        // Arrange - Mocking the user manager to return a list of hairdressers
        var GetUsersInRoleAsyncReturnValue = new List<ApplicationUser>
        {
            new ApplicationUser { Id = "1", FirstName = "John", LastName = "Doe", Email = "John.Doe@exampel.com", PhoneNumber = "1234567890", UserName = "JohnDoe" },
        };

        _userManagerMock
            .Setup(repo => repo.GetUsersInRoleAsync(It.IsAny<string>()))
            .ReturnsAsync(GetUsersInRoleAsyncReturnValue);

        // Act & Assert - Call the method with an empty ID and expect an exception
        var ex = Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _serviceMock.GetHairdresserWithId(hairdresserId));
        Assert.AreEqual("Id cannot be null or whitespace. (Parameter 'id')", ex.Result.Message);
    }

	[TestMethod]
	public async Task GetAllHairdressersAsync_ShouldReturnEmptyList_WhenNoUsersExist()
	{
		// Arrange
		_userRepository.Setup(repo => repo.GetAllAsync())
			.ReturnsAsync(new List<ApplicationUser>());

		// Act
		var result = await _serviceMock.GetAllHairdressersAsync();

		// Assert
		Assert.IsNotNull(result);
		Assert.AreEqual(0, result.Count());
	}

	[TestMethod]
	public async Task UpdateHairdresserAsync_PartialUpdate_OnlyUpdatesProvidedFields()
	{
		// Arrange
		var hairdresser = new ApplicationUser
		{
			Id = "1",
			FirstName = "John",
			LastName = "Doe",
			Email = "old@example.com",
			PhoneNumber = "9999999999",
			UserName = "JohnSmith"
		};
		var updatedDto = new UpdateUserDto
		{
			Email = "new@example.com"
		};

		_userManagerMock.Setup(repo => repo.GetUsersInRoleAsync(It.IsAny<string>()))
			.ReturnsAsync(new List<ApplicationUser> { hairdresser });

		// Act
		var result = await _serviceMock.UpdateHairdresserAsync("1", updatedDto);

		// Assert
		Assert.IsNotNull(result);
		Assert.AreEqual("new@example.com", result.Email);
		Assert.AreEqual("John", result.FirstName, "The name should not change if its not updated."); // unchanged
	}


}