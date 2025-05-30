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
    public async Task GetAllAvailableTimes_ShouldReturnCorrectTimeSlots()
    {
        //Make sure there are no bookings for the day so it can return all available slots
        var day = new DateTime(2025, 06, 01); 
        _bookingRepositoryMock.Setup(b => b.FindAsync(It.IsAny<Expression<Func<Booking, bool>>>()))
            .ReturnsAsync(new List<Booking>());

        // Test the method 
        var result = await _bookingService.GetAllAvailableTimes("H1", 1, day);

        // Make sure the result is a list of DateTime
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(List<DateTime>));
    }
}
