using Hairdresser.DTOs;
using Hairdresser.DTOs.User;
using Hairdresser.Mapping.Interfaces;
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
    private Mock<IBookingMapper> _bookingMapperMock;
    private BookingService _bookingService;

    [TestInitialize]
    public void Setup()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _treatmentRepositoryMock = new Mock<IGenericRepository<Treatment>>();
        _bookingMapperMock = new Mock<IBookingMapper>();
        _userManagerMock = MockUserManager();

        _bookingService = new BookingService(
            _treatmentRepositoryMock.Object,
            _bookingRepositoryMock.Object,
            _userManagerMock.Object,
            _bookingMapperMock.Object
        );

        //Default data setup 
        var defaultHairdresserId = "H1";
        var UserName = "Hair";
        var defaultHairdresser = new ApplicationUser { Id = defaultHairdresserId, UserName = UserName };
        _userManagerMock.Setup(h => h.FindByIdAsync(defaultHairdresserId))
            .ReturnsAsync(defaultHairdresser);

        var defaultTreatmentId = 1;
        var defaultTreatment = new Treatment {Id = defaultTreatmentId, Name = "Haircut", Description = "CutHair", Duration = 60, Price = 300.0 };
        _treatmentRepositoryMock.Setup(t => t.GetByIdAsync(defaultTreatmentId))
            .ReturnsAsync(defaultTreatment);

        var defaultCustomerId = "C1";
        var defaultCustomer = new ApplicationUser { Id = defaultCustomerId, UserName = "Customer1", Email = "Customer1@gmail.com", PasswordHash = "Customer123!" };
        _userManagerMock.Setup(c => c.FindByIdAsync(defaultCustomerId))
            .ReturnsAsync(defaultCustomer);

        //Default booking setup
        var defaultBookingId = 1;
        var defaultBooking = new Booking
        {
            Id = defaultBookingId,
            CustomerId = defaultCustomerId,
            HairdresserId = defaultHairdresserId,
            TreatmentId = defaultTreatmentId,
            Start = DateTime.Now.AddDays(1),
            End = DateTime.Now.AddDays(1).AddHours(1),
            Customer = defaultCustomer,
            Treatment = defaultTreatment,
            Hairdresser = defaultHairdresser
        };
        _bookingRepositoryMock.Setup(b => b.GetByIdWithDetailsAsync(defaultBookingId, defaultCustomerId))
            .ReturnsAsync(defaultBooking);

        // Mocking the booking mapper to return a BookingResponseDto based on the default booking
        var expectedDto = new BookingResponseDto
        {
            Id = defaultBooking.Id,
            Start = defaultBooking.Start,
            End = defaultBooking.End,
            Costumer = new UserDto
            {
                Id = defaultBooking.CustomerId,
                UserName = defaultBooking.Customer.UserName,
                Email = defaultBooking.Customer.Email,             
            },
            Treatment = new TreatmentDto
            {
                Name = defaultBooking.Treatment.Name,
                Description = defaultBooking.Treatment.Description,
                Duration = defaultBooking.Treatment.Duration,
                Price = defaultBooking.Treatment.Price
            },
            Hairdresser = new UserDto
            {
                UserName = defaultBooking.Hairdresser.UserName,
            }
        };

        _bookingMapperMock
            .Setup(m => m.MapToBookingReponse2Dto(It.IsAny<Booking>()))
            .Returns(expectedDto);
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

    [TestMethod]
    [TestCategory("Normally")]
    public async Task GetBookingByIdAsync_ShouldReturnRightCustomerBookingWithDetails()
    {
        // Setup the expected booking details
        var expectedBooking = new Booking
        {
            Id = 1,
            CustomerId = "C1",
            HairdresserId = "H1",
            TreatmentId = 1,
        };
        // Test the method 
        var result = await _bookingService.GetBookingByIdAsync(1, "C1");
        

        // Check if result is not null
        Assert.IsNotNull(result);
        // Check if the booking ID and customer ID are correct
        Assert.AreEqual(expectedBooking.Id, result.Id);
        Assert.AreEqual(expectedBooking.CustomerId, result.Costumer.Id);

    }

    [TestMethod]
    [TestCategory("Edge-Case ")]
    public async Task GetBookingByIdAsync_ShouldThrowKeyNotFoundException_WhenBookingIsNotFound()
    {
        // Setup the booking repository to return null for a non-existing booking
        _bookingRepositoryMock.Setup(b => b.GetByIdWithDetailsAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((Booking)null);
        // Call the method and expect an exception
        var result = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _bookingService.GetBookingByIdAsync(1, "C1"));
        // Assert that the exception message is correct
        Assert.AreEqual("Booking was not found.", result.Message);
        
    }

    [TestMethod]
    [TestCategory("Normally")]
    public async Task BookAppointment_ShouldReturnBookingResponseDto_WhenBookingIsSuccessful()
    {
        // Requested booking details
        var request = new BookingRequestDto
        {
            HairdresserId = "H1",
            TreatmentId = 1,
            Start = DateTime.Now.AddDays(1),           
        };
        // Expected response after booking 
        var expectedResponse = new BookingResponseDto
        {
            Id = 1,
            Start = request.Start,         
            Costumer = new UserDto { Id = "C1", UserName = "Customer1" },
            Treatment = new TreatmentDto { Name = "Haircut", Description = "CutHair", Duration = 60, Price = 300.0 },
            Hairdresser = new UserDto { UserName = "Hair" }
        };
        _bookingMapperMock.Setup(m => m.MapToBookingReponse2Dto(It.IsAny<Booking>()))
            .Returns(expectedResponse);
        // Act
        var result = await _bookingService.BookAppointment("C1", request);
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResponse.Id, result.Id);
        Assert.AreEqual(expectedResponse.Start, result.Start);
        Assert.AreEqual(expectedResponse.End, result.End);
    }

    [TestMethod]
    [TestCategory("Edge-Case")]
    public async Task BookAppointment_ShouldThrowKeyNotFoundException_WhenTreatmentNotFound()
    {       
        // Requested booking details
        var request = new BookingRequestDto
        {
            HairdresserId = "H1",
            TreatmentId = 134,
            Start = DateTime.Now.AddDays(1),
        };

        // Call the method and expect an exception
        var result = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _bookingService.BookAppointment("C1", request));

        // Assert that the exception message is correct
        Assert.AreEqual("Treatment was not found.", result.Message);
    }

    [TestMethod]
    [TestCategory("Edge-Case")]
    public async Task BookAppointment_ShouldThorKeyNotFoundexception_WhenHairdresserNotFound()
    {
        // Requested booking details
        var request = new BookingRequestDto
        {
            HairdresserId = "H134",
            TreatmentId = 1,
            Start = DateTime.Now.AddDays(1),
        };
        // Call the method and expect an exception
        var result = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _bookingService.BookAppointment("C1", request));
        // Assert that the exception message is correct
        Assert.AreEqual("Hairdresser was not found", result.Message);
    }
    [TestMethod]
    [TestCategory("Noramally")]
    public async Task CancelBooking_ShouldReturnBookingDto_WhenCancellationIsSuccessful()
    {
        // Setup the expected booking details
        var expectedBooking = new Booking
        {
            Id = 1,
            CustomerId = "C1",
            HairdresserId = "H1",
            TreatmentId = 1,
            Start = DateTime.Now.AddDays(1),
            End = DateTime.Now.AddDays(1).AddHours(1)
        };
        _bookingRepositoryMock.Setup(b => b.GetByIdWithDetailsAsync(expectedBooking.Id, expectedBooking.CustomerId))
            .ReturnsAsync(expectedBooking);
        // Act
        var result = await _bookingService.CancelBooking("C1", 1);
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedBooking.Id, result.Id);
    }

    [TestMethod]
    [TestCategory("Edge-Case")]
    public async Task CancelBooking_ShouldReturnKeyNotFound_WhenBookingIsNotFoundForThatCustomer()
    {
        // Call the method and expect an exception
        var result = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
            () => _bookingService.CancelBooking("C1", 14333));
        // Assert that the exception message is correct
        Assert.AreEqual("Booking was not found.", result.Message);
    }

}
