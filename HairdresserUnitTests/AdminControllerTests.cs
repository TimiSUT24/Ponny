using Hairdresser.Controllers;
using Hairdresser.DTOs;
using Hairdresser.DTOs.User;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Claims;

namespace HairdresserUnitTests
{
    [TestClass]
    public class AdminControllerTests
    {
        private Mock<IAdminService>? _mockAdminService;
        private Mock<UserManager<ApplicationUser>>? _mockUserManager;
        private AdminController? _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockAdminService = new Mock<IAdminService>();
            _mockUserManager = MockUserManager();

            _controller = new AdminController(_mockAdminService.Object, _mockUserManager.Object);

            // Simulera inloggad admin
            var adminClaims = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "admin-id"),
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = adminClaims }
            };
        }

        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        public async Task GetAllBookingsOverview_ReturnsOkWithData()
        {
            // Arrange
            var mockBookings = new List<BookingResponseDto>
            {
                new BookingResponseDto { Id = 1 }
            };

            _mockAdminService!
                .Setup(s => s.GetAllBookingsOverviewAsync())
                .ReturnsAsync(mockBookings);

            // Act
            var result = await _controller!.GetAllBookingsOverview();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = result as OkObjectResult;
            Assert.AreEqual(200, ok?.StatusCode ?? 200);
            Assert.IsInstanceOfType(ok?.Value, typeof(IEnumerable<BookingResponseDto>));
        }

        [TestMethod]
        public async Task CreateHairdresser_ValidInput_ReturnsOkWithUserDto()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                UserName = "testuser",
                Email = "test@example.com",
                PhoneNumber = "0701234567",
                Password = "StrongPass!123",
                ConfirmPassword = "StrongPass!123"
            };

            var createdUser = new ApplicationUser
            {
                Id = "abc123",
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            _mockUserManager!
                .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Hairdresser"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller!.CreateHairdresser(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = result as OkObjectResult;
            var returned = ok?.Value as UserDto;
            Assert.AreEqual(dto.UserName, returned?.UserName);
        }

        [TestMethod]
        public async Task CreateHairdresser_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var dto = new RegisterUserDto(); // saknar allt
            _controller!.ModelState.AddModelError("Email", "Required");

            // Act
            var result = await _controller.CreateHairdresser(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(400, bad?.StatusCode ?? 400);
        }

        [TestMethod]
        public async Task CreateHairdresser_CreateFails_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                UserName = "testuser",
                Email = "test@example.com",
                PhoneNumber = "0701234567",
                Password = "StrongPass!123",
                ConfirmPassword = "StrongPass!123"
            };

            var identityErrors = new List<IdentityError>
            {
                new IdentityError { Code = "DuplicateEmail", Description = "Email is already taken" }
            };

            _mockUserManager!
                .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

            // Act
            var result = await _controller!.CreateHairdresser(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var bad = result as BadRequestObjectResult;
            Assert.AreEqual(400, bad?.StatusCode ?? 400);
        }

        [TestMethod]
        public async Task CreateHairdresser_AddToRole_CalledWithCorrectRole()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                UserName = "newstylist",
                Email = "stylist@example.com",
                PhoneNumber = "0700000000",
                Password = "Hair123!",
                ConfirmPassword = "Hair123!"
            };

            ApplicationUser? capturedUser = null;

            _mockUserManager!
                .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .Callback<ApplicationUser, string>((user, _) => capturedUser = user)
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Hairdresser"))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable("AddToRoleAsync was not called with 'Hairdresser'.");

            // Act
            var result = await _controller!.CreateHairdresser(dto);

            // Assert
            _mockUserManager.Verify(m => m.AddToRoleAsync(It.Is<ApplicationUser>(u => u.UserName == "newstylist"), "Hairdresser"), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
