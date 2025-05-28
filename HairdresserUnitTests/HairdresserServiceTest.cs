using Hairdresser.Data;
using Hairdresser.DTOs;
using Hairdresser.DTOs.User;
using Hairdresser.Repositories;
using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HairdresserUnitTests;

[TestClass]
public class HairdresserServiceTest
{
    private ApplicationDBContext? _context;
    private Mock<UserManager<ApplicationUser>> _userManagerMock = null!;
    private UserRepository _userRepository = null!;
    private IBookingRepository _bookingRepository = null!;
    private IHairdresserService _service = null!;



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
        _bookingRepository = new BookingRepository(_context);
        _service = new HairdresserService(_userRepository, _bookingRepository, _userManagerMock.Object);
    }

    [TestMethod]
    public async Task GetAllHairdressersAsync_ShouldReturnHairdressers()
    {
        // Arrange
        var hairdresser1 = new ApplicationUser { Id = "1", UserName = "hairdresser1", Email = "Jon.Doe@exampel.com", PhoneNumber = "1234567890" };
        var hairdresser2 = new ApplicationUser { Id = "2", UserName = "hairdresser2", Email = "Jan.Doe@exampel.com", PhoneNumber = "0987654321" };

        await _userRepository.AddAsync(hairdresser1);
        await _userRepository.AddAsync(hairdresser2);
        await _userRepository.SaveChangesAsync();
        // Act
        var result = await _service.GetAllHairdressersAsync();
        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<IEnumerable<UserRespondDto>>(result);
    }

    [TestMethod]
    public async Task GetWeekScheduleAsync_ShouldReturnBookingsForWeek()
    {
        // Arrange
        var weekStart = DateTime.Now.Date;

        var hairdresser = new ApplicationUser
        {
            FirstName = "John",
            LastName = "Doe",
            UserName = "hairdresser1",
            Email = "John.doe@exampel.com",
            PhoneNumber = "1234567890"
        };
        var costumer = new ApplicationUser
        {
            FirstName = "Jane",
            LastName = "Doe",
            UserName = "customer1",
            Email = "Jane.doe@exampel.com",
            PhoneNumber = "0987654321"
        };
        await _userRepository.AddAsync(costumer);
        await _userRepository.AddAsync(hairdresser);
        await _userRepository.SaveChangesAsync();

        var booking1 = new Booking
        {
            Start = weekStart.AddDays(1).AddHours(10),
            End = weekStart.AddDays(1).AddHours(11),
            HairdresserId = costumer.Id,
            CustomerId = hairdresser.Id
        };
        var booking2 = new Booking
        {
            Start = weekStart.AddDays(3).AddHours(14),
            End = weekStart.AddDays(3).AddHours(15),
            HairdresserId = costumer.Id,
            CustomerId = hairdresser.Id
        };

        await _bookingRepository.AddAsync(booking1);
        await _bookingRepository.AddAsync(booking2);
        await _bookingRepository.SaveChangesAsync();

        // Act
        var result = await _service.GetWeekScheduleAsync(hairdresser.Id, weekStart);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any());
    }
}
