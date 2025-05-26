using Hairdresser.Controllers;
using Hairdresser.Data;
using Hairdresser.Repositories;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Company.TestProject1;

[TestClass]
public class HairdresserControllerTest
{
    private ApplicationDBContext? _context;
    private HairdresserController? _hairdresserController;
    private Mock<UserManager<ApplicationUser>> _userManagerMock = null!;

    [TestInitialize]
    public void Setup()
    {
        // Set up in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var userStore = new Mock<IUserStore<ApplicationUser>>();
        var userManager = new UserManager<ApplicationUser>(
                userStore.Object, null!, null!, null!, null!, null!, null!, null!, null!
            );

        _context = new ApplicationDBContext(options);
        var userRepository = new UserRepository(_context, userManager);
        var bookingRepository = new Mock<IGenericRepository<Booking>>().Object;
        _hairdresserController = new HairdresserController(_context, userRepository, bookingRepository);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context!.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public async Task GetAll_ShouldReturnAllHairdressers_Users()
    {
        // Arrange
        var hairdresser1 = new ApplicationUser { FirstName = "John", LastName = "Doe" };
        var hairdresser2 = new ApplicationUser { FirstName = "Jane", LastName = "Smith" };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        hairdresser1.PasswordHash = passwordHasher.HashPassword(hairdresser1, "password123");
        hairdresser2.PasswordHash = passwordHasher.HashPassword(hairdresser2, "password123");

        _context!.ApplicationUser.Add(hairdresser1);
        _context.ApplicationUser.Add(hairdresser2);
        await _context.SaveChangesAsync();

        // Act
        var hairdressers = await GetAllHairdressersAsync();

        // Assert
        Assert.IsNotNull(hairdressers);
        Assert.AreEqual(2, hairdressers.Count());
    }

    [TestMethod]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoHairdressers()
    {
        // Act
        var hairdressers = await GetAllHairdressersAsync();
        // Assert
        Assert.IsNotNull(hairdressers);
        Assert.AreEqual(0, hairdressers.Count());
    }

    [TestMethod]
    public async Task GetHairdresserById_ShudlReturnHairdresserIdWhenFound()
    {
        // Arrange
        var hairdresser = new ApplicationUser { FirstName = "John", LastName = "Doe" };
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        hairdresser.PasswordHash = passwordHasher.HashPassword(hairdresser, "password123");

        _context!.ApplicationUser.Add(hairdresser);
        await _context.SaveChangesAsync();

        // Act
        var result = await _hairdresserController!.GetHairdresserById(hairdresser.Id) as OkObjectResult;
        var hairdresserResult = result?.Value as ApplicationUser;

        // Assert
        Assert.IsNotNull(hairdresserResult);
    }

    private async Task<IEnumerable<ApplicationUser>?> GetAllHairdressersAsync()
    {

        var result = await _hairdresserController!.GetAll();

        if (result.Result is not OkObjectResult)
        {
            Assert.Fail("Expected OkObjectResult");
        }
        var okResult = result.Result as OkObjectResult;
        if (okResult == null)
        {
            Assert.Fail("Expected OkObjectResult");
        }
        return okResult.Value as IEnumerable<ApplicationUser>;
    }

}
