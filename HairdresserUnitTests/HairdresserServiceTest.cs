using Hairdresser.Data;
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
}
