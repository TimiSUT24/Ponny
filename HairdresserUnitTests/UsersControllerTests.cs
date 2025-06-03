using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hairdresser.Controllers;
using Hairdresser.Data;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using HairdresserClassLibrary.DTOs.User;

namespace HairdresserUnitTests
{
    [TestClass]
    public class UsersControllerTests
    {
        private ApplicationDBContext _context = null!;
        private Mock<UserManager<ApplicationUser>> _userManagerMock = null!;
        private UsersController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            // 👇 Set up in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Users")
                .Options;

            _context = new ApplicationDBContext(options);

            // 👇 Mock UserManager with email store
            var store = new Mock<IUserStore<ApplicationUser>>();
            store.As<IUserEmailStore<ApplicationUser>>(); // Required for FindByEmailAsync

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            _controller = new UsersController(_context, _userManagerMock.Object);
        }

        [TestMethod]
        public async Task Register_CreatesUser_ReturnsCreatedAtAction()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                UserName = "testuser",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Password = "Password123!"
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(dto.UserName))
                .ReturnsAsync((ApplicationUser?)null);
            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            var createdUser = createdResult.Value as ApplicationUser;
            Assert.IsNotNull(createdUser);
            Assert.AreEqual(dto.Email, createdUser.Email);
        }

        [TestMethod]
        public async Task GetById_UserExists_ReturnsUser()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "user-1",
                UserName = "user1",
                Email = "user1@example.com",
                PhoneNumber = "111222333"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetById("user-1");

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedUser = okResult.Value as ApplicationUser;
            Assert.IsNotNull(returnedUser);
            Assert.AreEqual(user.Email, returnedUser.Email);
        }

        [TestMethod]
        public async Task Update_UserExists_UpdatesInfo()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "user-2",
                UserName = "oldname",
                Email = "old@example.com",
                PhoneNumber = "000000000"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var updatedUser = new UserDto
            {
                Id = "user-2",
                UserName = "newname",
                Email = "new@example.com",
                PhoneNumber = "999999999"
            };

            // Act
            var result = await _controller.Update("user-2", updatedUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            var fetched = await _context.Users.FindAsync("user-2");
            Assert.AreEqual("newname", fetched!.UserName);
            Assert.AreEqual("new@example.com", fetched.Email);
        }

        [TestMethod]
        public async Task Register_AddsUserRole_User()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                UserName = "newuser",
                Email = "newuser@example.com",
                PhoneNumber = "1234567890",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            var createdUser = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(dto.UserName))
                .ReturnsAsync((ApplicationUser?)null);
            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync((ApplicationUser?)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
                .Callback<ApplicationUser, string>((user, _) =>
                {
                    // Simulate that user gets a ID after registration
                    user.Id = "generated-user-id";
                })
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "User"))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable("AddToRoleAsync should be called with 'User'");

            // Act
            var result = await _controller.Register(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "User"), Times.Once);
        }
    }
}
