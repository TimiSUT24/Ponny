using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hairdresser.Data;
using Hairdresser.DTOs;
using Hairdresser.DTOs.User;
using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services;
using HairdresserClassLibrary.Models;
using HairdresserUnitTests.utils;
using HairdresserUnitTests.Utils;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace HairdresserUnitTests;

[TestClass]
public class HairdresserServiceTest
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock = null!;
    private Mock<IUserRepository> _userRepository = null!;
    private Mock<IBookingRepository> _bookingRepository = null!;



    [TestInitialize]
    public void Setup()
    {
        _userManagerMock = MockUser.InitializeUserManager();
        _userRepository = new Mock<IUserRepository>();
        _bookingRepository = new Mock<IBookingRepository>();
    }



    [TestMethod]
    public async Task GetAllHairdressersAsync_ShouldReturnHairdressers()
    {
        // Arrange
        var users = new List<ApplicationUser>
        {
            new ApplicationUser { Id = "1", UserName = "hairdresser1", Email = "Jon.Doe@exampel.com", PhoneNumber = "1234567890" },
            new ApplicationUser { Id = "2", UserName = "hairdresser2", Email = "Jan.Doe@exampel.com", PhoneNumber = "0987654321" }
        };

        _userRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(users);

        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act
        var result = await _serviceMock.GetAllHairdressersAsync();
        var expected = 2;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());
    }

    [TestMethod]
    public async Task GetWeekScheduleAsync_ShouldReturnBookingsForWeek()
    {
        // Arrange
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

        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act
        var result = await _serviceMock.GetWeekScheduleAsync("hairdresser1", DateTime.Now);
        var expected = 2;

        // Assert
        Assert.AreEqual(expected, result.Count());

    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("  ")]
    [DataRow(null)]
    public async Task GetWeekScheduleAsync_GetEmptyHairdresserId_ReturnsEmptyList(string hairdresserId)
    {
        // Arrange
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

        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act
        var result = await _serviceMock.GetWeekScheduleAsync(hairdresserId, DateTime.Now);
        var expected = 0;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());
    }
    [TestMethod]
    public async Task GetWeekScheduleAsync_GetDateInPast_ReturnsEmptyList()
    {
        // Arrange
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

        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act
        var result = await _serviceMock.GetWeekScheduleAsync("hairdresser1", DateTime.Now.AddDays(-1));
        var expected = 0;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());
    }
    [TestMethod]
    public async Task GetMonthlyScheduleAsync_ShouldReturnBookingsForMonth()
    {
        // Arrange
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

        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act
        var result = await _serviceMock.GetMonthlyScheduleAsync("hairdresser1", 2025, 1);
        var expected = 2;

        // Assert
        Assert.AreEqual(expected, result.Count());

    }
    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("  ")]
    [DataRow(null)]
    public async Task GetMonthlyScheduleAsync_GetEmptyHairdresserId_ReturnsEmptyList(string hairdresserId)
    {
        // Arrange
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

        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act
        var result = await _serviceMock.GetMonthlyScheduleAsync(hairdresserId, 2025, 1);
        var expected = 0;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());
    }
    [TestMethod]
    [DataRow(2025, -1)]
    [DataRow(2025, 0)]
    [DataRow(2025, 13)]
    [DataRow(2024, 12)]
    [DataRow(2026, 12)]
    public async Task GetMonthlyScheduleAsync_InvalidDateFilter_ReturnsEmptyList(int year, int month)
    {
        // Arrange
        var user = new ApplicationUser { UserName = "hairdresser1", Email = "Jon.Doe@exampel.com", PhoneNumber = "1234567890" };
        var treatment = new Treatment { Id = 1, Name = "Haircut", Price = 20, Description = "Basic haircut", Duration = 60 };

        var Bookings = new List<Booking>
        {
            new Booking { Id = 1, Start = DateTime.Now, End = DateTime.Now.AddHours(1), Customer = user, Hairdresser = user, Treatment = treatment },
        };
        _bookingRepository
            .Setup(repo => repo.GetMonthlyScheduleWithDetailsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(Bookings);

        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act
        var result = await _serviceMock.GetMonthlyScheduleAsync("hairdresser1", year, month);
        var expected = 0;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());
    }
    [TestMethod]
    public async Task GetBookingDetailsAsync_ShouldReturnBookingDetails()
    {
        // Arrange
        var user = new UserDto { UserName = "hairdresser1", Email = "Jon.Doe@exampel.com", PhoneNumber = "1234567890" };
        var treatment = new TreatmentDto { Id = 1, Name = "Haircut", Price = 20, Description = "Basic haircut", Duration = 60 };
        var Bookings = new HairdresserBookingRespondDto { Id = 1, Start = DateTime.Now, End = DateTime.Now.AddHours(1), Customer = user, Treatment = treatment };
        _bookingRepository
            .Setup(rep => rep.GetBookingWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(Bookings);
        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act
        var result = await _serviceMock.GetBookingDetailsAsync(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(Bookings.Id, result.Id);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public async Task GetBookingDetailsAsync_BookingNotFound_ReturnsNull(int BookingId)
    {
        // Arrange
        _bookingRepository
            .Setup(rep => rep.GetBookingWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync(null as HairdresserBookingRespondDto);
        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act
        var result = await _serviceMock.GetBookingDetailsAsync(BookingId);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task UpdateHairdresserAsync_ShouldUpdateHairdresser()
    {
        // Arrange
        var GetUsersInRoleAsyncReternValue = new List<ApplicationUser>
        {
            new ApplicationUser { Id = "1", FirstName = "John", LastName = "Doe", Email = "John.Doe@exampel.com", PhoneNumber = "1234567890", UserName = "JohnDoe" },
        };
        var updateUserDto = new UpdateUserDto { FirstName = "Updated", LastName = "Doe", Email = "John.Doe@exampel.com", PhoneNumber = "1234567890", UserName = "JohnDoe" };

        _userManagerMock
            .Setup(repo => repo.GetUsersInRoleAsync(It.IsAny<string>()))
            .ReturnsAsync(GetUsersInRoleAsyncReternValue);
        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        //Act
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

        //Assert
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
        // Arrange
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

        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act
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
        // Arrange
        var GetUsersInRoleAsyncReturnValue = new List<ApplicationUser>();

        _userManagerMock
            .Setup(repo => repo.GetUsersInRoleAsync(It.IsAny<string>()))
            .ReturnsAsync(GetUsersInRoleAsyncReturnValue);
        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act
        var result = await _serviceMock.UpdateHairdresserAsync(hairdresserId, new UpdateUserDto());

        // Assert
        Assert.IsNull(result);
        _userRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        _userRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [TestMethod]
    public async Task GetHairdresserWithId_ShouldReturnHairdresser()
    {
        // Arrange
        var GetUsersInRoleAsyncReturnValue = new List<ApplicationUser>
        {
            new ApplicationUser { Id = "1", FirstName = "John", LastName = "Doe", Email = "John.Doe@exampel.com", PhoneNumber = "1234567890", UserName = "JohnDoe" },
        };
        _userManagerMock
            .Setup(repo => repo.GetUsersInRoleAsync(It.IsAny<string>()))
            .ReturnsAsync(GetUsersInRoleAsyncReturnValue);
        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act
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
    public async Task GetHairdresserWithId_EmptyId_ReturnsNull(string hairdresserId)
    {
        // Arrange
        var GetUsersInRoleAsyncReturnValue = new List<ApplicationUser>
        {
            new ApplicationUser { Id = "1", FirstName = "John", LastName = "Doe", Email = "John.Doe@exampel.com", PhoneNumber = "1234567890", UserName = "JohnDoe" },
        };

        _userManagerMock
            .Setup(repo => repo.GetUsersInRoleAsync(It.IsAny<string>()))
            .ReturnsAsync(GetUsersInRoleAsyncReturnValue);
        var _serviceMock = new HairdresserService(_userRepository.Object, _bookingRepository.Object, _userManagerMock.Object);

        // Act 
        var result = await _serviceMock.GetHairdresserWithId(hairdresserId);
        // Assert
        Assert.IsNull(result);
    }
}