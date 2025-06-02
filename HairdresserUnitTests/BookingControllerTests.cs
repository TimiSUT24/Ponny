using Hairdresser.Controllers;
using Hairdresser.Data;
using Hairdresser.DTOs;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.DTOs;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using Hairdresser.Services.Interfaces;

namespace HairdresserUnitTests
{
    [TestClass]
    public class BookingControllerTests
    {
        private ApplicationDBContext? _context;
        private BookingsController? _controller;
        private Mock<IBookingService>? _mockBookingService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // uniqe name
                .Options;

            _context = new ApplicationDBContext(options);

            _mockBookingService = new Mock<IBookingService>();
            _controller = new BookingsController(_mockBookingService.Object);
        }


        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [TestMethod]
        public async Task BookAppointment_ReturnsCreatedWithCorrectBookingData()
        {
            // Arrange
            var customerId = "customer-1";
            var hairdresserId = "hairdresser-1";
            var treatmentId = 2;
            var startTime = DateTime.Now.AddDays(2);

            var bookingDTO = new BookingRequestDto
            {
                Start = startTime,
                TreatmentId = treatmentId,
                HairdresserId = hairdresserId
            };

            var expectedResponse = new BookingResponseDto
            {
                Id = 1,
                Start = startTime,
                End = startTime.AddMinutes(30)
            };


            // Setup service mock
            _mockBookingService!
                .Setup(s => s.BookAppointment(customerId, It.IsAny<BookingRequestDto>()))
                .ReturnsAsync(expectedResponse);

            // Mock authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, customerId)
    }, "mock"));

            _controller!.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.BookAppointment(bookingDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            var createdResult = result as CreatedAtActionResult;

            Assert.IsNotNull(createdResult);
            var response = createdResult!.Value as BookingResponseDto;

            Assert.IsNotNull(response);
            Assert.AreEqual(expectedResponse.Id, response!.Id);
            Assert.AreEqual(expectedResponse.Start, response.Start);
            Assert.AreEqual(expectedResponse.End, response.End);

        }


        [TestMethod]
        public async Task Book_NonExisting_Treatment_Returns404()
        {
            // Arrange
            var customerId = "customer-1";
            var hairdresserId = "hairdresser-1";
            var treatmentId = 235; // non-existing treatment
            var startTime = DateTime.Now.AddDays(2);

            var bookingDTO = new BookingRequestDto
            {
                Start = startTime,
                TreatmentId = treatmentId,
                HairdresserId = hairdresserId
            };

            // Mock authenticated user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, customerId)
    }, "mock"));

            _controller!.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };


            // Act
            var result = await _controller.BookAppointment(bookingDTO);

            // Assert
            if (result is NotFoundObjectResult notFoundResult)
            {
                Assert.AreEqual(404, notFoundResult.StatusCode);
            }
            else
            {
                Assert.Fail("Unexpected result type: " + result.GetType().Name);
            }

        }

        [TestMethod]
        public async Task Book_Occupied_Time_ReturnsConflict()
        {
            // Arrange
            var dto = new BookingRequestDto
            {
                HairdresserId = "h1",
                TreatmentId = 1,
                Start = DateTime.Now.AddHours(1)
            };

            var fakeUserId = "u1";

            _mockBookingService!
                .Setup(s => s.BookAppointment(fakeUserId, dto))
                .ThrowsAsync(new InvalidOperationException("Any message"));

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, fakeUserId)
            }, "mock"));

            _controller!.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.BookAppointment(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ConflictObjectResult));
            var conflict = result as ConflictObjectResult;
            Assert.AreEqual(StatusCodes.Status409Conflict, conflict?.StatusCode);
        }


        [TestMethod]
        public async Task Book_InvalidTime_ReturnsConflict()
        {
            // Arrange
            var customerId = "customer-1";
            var bookingDTO = new BookingRequestDto
            {
                Start = DateTime.Now.AddDays(2),
                TreatmentId = 1,
                HairdresserId = "hairdresser-1"
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, customerId)
    }, "mock"));

            _controller!.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _mockBookingService!
                .Setup(s => s.BookAppointment(customerId, bookingDTO))
                .ThrowsAsync(new InvalidOperationException("Some error"));

            // Act
            var result = await _controller.BookAppointment(bookingDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ConflictObjectResult));
            var conflict = result as ConflictObjectResult;
            Assert.AreEqual(StatusCodes.Status409Conflict, conflict?.StatusCode);
        }

        [TestMethod]
        public async Task Rebook_ValidRequest_ReturnsOkWithUpdatedBooking()
        {
            // Arrange
            var userId = "customer-1";
            var bookingId = 42;

            var rebookRequest = new BookingRequestDto
            {
                HairdresserId = "hairdresser-1",
                TreatmentId = 2,
                Start = DateTime.Now.AddDays(3)
            };

            var expectedBooking = new BookingResponseDto
            {
                Id = bookingId,
                Start = rebookRequest.Start,
                End = rebookRequest.Start.AddMinutes(45),
                Treatment = new TreatmentDto { Id = 2, Name = "Färgning" }
            };

            _mockBookingService!
                .Setup(s => s.RebookBooking(userId, bookingId, rebookRequest))
                .ReturnsAsync(expectedBooking);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId)
    }, "mock"));

            _controller!.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.Rebook(bookingId, rebookRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedBooking = okResult!.Value as BookingResponseDto;
            Assert.IsNotNull(returnedBooking);
            Assert.AreEqual(expectedBooking.Id, returnedBooking!.Id);
            Assert.AreEqual(expectedBooking.Start, returnedBooking.Start);
        }
    }
}
