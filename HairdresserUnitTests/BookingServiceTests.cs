using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

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
    }

    private Mock<UserManager<ApplicationUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(
            store.Object, null, null, null, null, null, null, null, null);
    }

    [TestMethod]
    public void TestMethod1()
    {
       
    }
}
