using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hairdresser.Controllers;
using Hairdresser.Data;
using Hairdresser.DTOs;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HairdresserUnitTests
{
    [TestClass]
    public class UsersControllerTests
    {
        private UsersController GetControllerWithContext(out ApplicationDBContext context)
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            context = new ApplicationDBContext(options);

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new UserManager<ApplicationUser>(
                userStore.Object, null!, null!, null!, null!, null!, null!, null!, null!
            );

            return new UsersController(context, userManager);
        }

        [TestMethod]
        public async Task Register_CreatesUser_ReturnsCreatedAtAction()
        {
            var controller = GetControllerWithContext(out var context);
            var dto = new RegisterUserDto
            {
                UserName = "testuser",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
                Password = "Password123!",
            };

            var result = await controller.Register(dto);

            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult); 
            var createdUser = createdResult.Value as ApplicationUser;
            Assert.IsNotNull(createdUser);
            Assert.AreEqual(dto.Email, createdUser.Email);
        }

        [TestMethod]
        public async Task GetById_UserExists_ReturnsUser()
        {
            var controller = GetControllerWithContext(out var context);
            var user = new ApplicationUser
            {
                Id = "user-1",
                UserName = "user1",
                Email = "user1@example.com",
                PhoneNumber = "111222333"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var result = await controller.GetById("user-1");

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedUser = okResult.Value as ApplicationUser;
            Assert.AreEqual(user.Email, returnedUser.Email);
        }

        [TestMethod]
        public async Task Update_UserExists_UpdatesInfo()
        {
            var controller = GetControllerWithContext(out var context);
            var user = new ApplicationUser
            {
                Id = "user-2",
                UserName = "oldname",
                Email = "old@example.com",
                PhoneNumber = "000000000"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var updated = new ApplicationUser
            {
                Id = "user-2",
                UserName = "newname",
                Email = "new@example.com",
                PhoneNumber = "999999999"
            };

            var result = await controller.Update("user-2", updated);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            var updatedUser = await context.Users.FindAsync("user-2");
            Assert.AreEqual("newname", updatedUser.UserName);
            Assert.AreEqual("new@example.com", updatedUser.Email);
        }
    }
}
