using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq.Expressions;

namespace HairdresserUnitTests;

[TestClass]
public class BookingServiceTests
{
    private Mock<IBookingRepository> _bookingRepositoryMock;
    private Mock<IGenericRepository<Treatment>> _treatmentRepositoryMock;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private BookingService _bookingService;

    [TestInitialize]
    public void Setup()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _treatmentRepositoryMock = new Mock<IGenericRepository<Treatment>>();
        _userManagerMock = MockUserManager();

        _bookingService = new BookingService(
            _treatmentRepositoryMock.Object,
            _bookingRepositoryMock.Object,
            _userManagerMock.Object
        );

        //Default data setup 
        var defaultHairdresserId = "H1";
        var UserName = "Hair";
        var defaultHairdresser = new ApplicationUser { Id = defaultHairdresserId, UserName = UserName };
        _userManagerMock.Setup(h => h.FindByIdAsync(defaultHairdresserId))
            .ReturnsAsync(defaultHairdresser);

        var defaultTreatmentId = 1;
        var defaultTreatment = new Treatment {Id = defaultTreatmentId, Name = "Haircut", Duration = 60, Price = 300.0 };
        _treatmentRepositoryMock.Setup(t => t.GetByIdAsync(defaultTreatmentId))
            .ReturnsAsync(defaultTreatment);

        var defaultCustomerId = "C1";
        var defaultCustomer = new ApplicationUser { Id = defaultCustomerId, UserName = "Customer1", Email = "Customer1@gmail.com", PasswordHash = "Customer123!" };
        _userManagerMock.Setup(c => c.FindByIdAsync(defaultCustomerId))
            .ReturnsAsync(defaultCustomer); 
    }

    private Mock<UserManager<ApplicationUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(
            store.Object, null, null, null, null, null, null, null, null);
    }

    [TestMethod]
    [TestCategory("Normally")]
    public async Task GetAllAvailableTimes_ShouldReturnCorrectTimeSlots()
    {
        //Make sure there are no bookings for the day so it can return all available slots
        var day = DateTime.Now.AddDays(1).Date;    
        _bookingRepositoryMock.Setup(b => b.FindAsync(It.IsAny<Expression<Func<Booking, bool>>>()))
            .ReturnsAsync(new List<Booking>());

        // Test the method 
        var result = await _bookingService.GetAllAvailableTimes("H1", 1, day);

       // Check if result is not null
        Assert.IsNotNull(result);
        // Make sure the result is a list of DateTime
        Assert.IsInstanceOfType(result, typeof(List<DateTime>));
    }

    [TestMethod]
    [TestCategory("Edge-Case ")]
    public async Task GetAllAvailableTimes_ShouldThrowKeyNotFoundException_WhenHairdresserIsNotFound()
    {
        
        var day = DateTime.Now.AddDays(1).Date; 
        _userManagerMock.Setup(h => h.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser)null); // Simulate hairdresser not found

        // Call the method and expect an exception
        var result = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _bookingService.GetAllAvailableTimes("H1", 1, day));

        // Assert that the exception message is correct
        Assert.AreEqual("Hairdresser was not found", result.Message);         
    }

    [TestMethod]
    [TestCategory("Edge-Case ")]
    public async Task GetAllAvailableTimes_ShouldThrowKeyNowFoundException_WhenTreatmentIsNotFound()
    {
       
        var day = DateTime.Now.AddDays(1).Date;
        _treatmentRepositoryMock.Setup(t => t.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Treatment)null); // Simulate treatment not found

        // Call the method and expect an exception
        var result = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _bookingService.GetAllAvailableTimes("H1", 1, day));

        // Assert that the exception message is correct
        Assert.AreEqual("Treatment was not found", result.Message);
    }

    [TestMethod]
    [TestCategory("Edge-Case ")]
    public async Task GetAllAvailableTimes_ShouldThrowArgumentException_WhenDayIsInvalid()
    {
        // Simulate an invalid day
        var invalidDay = DateTime.Now.AddDays(-2).Date; // Past date
        // Call the method and expect an exception
        var result = await Assert.ThrowsExceptionAsync<ArgumentException>(
            () => _bookingService.GetAllAvailableTimes("H1", 1, invalidDay));
        // Assert that the exception message is correct
        Assert.AreEqual("Can only book from today and up to 4 month in advance.", result.Message);
    }

}
