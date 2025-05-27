using Hairdresser.Controllers;
using Hairdresser.Data;
using Hairdresser.Repositories;
using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HairdresserUnitTests;

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
        var bookingRepository = new BookingRepository(_context);

        var hairdresser = new HairdresserService(userRepository, bookingRepository, userManager);
        _hairdresserController = new HairdresserController(hairdresser);
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
        var hairdressers = await _hairdresserController!.GetAllHairdressersAsync();

        // Assert
        Assert.IsNotNull(hairdressers);
        Assert.AreEqual(2, hairdressers.Count());
    }

    [TestMethod]
    public async Task GetAll_ShalldReturnEmptyList_WhenNoHairdressers()
    {
        // Act
        var hairdressers = await _hairdresserController!.GetAllHairdressersAsync();
        // Assert
        Assert.IsNotNull(hairdressers);
        Assert.AreEqual(0, hairdressers.Count());
    }

    [TestMethod]
    public async Task GetHairdresserById_ShouldReturnHairdresserIdWhenFound()
    {
        // Arrange
        var hairdresser = new ApplicationUser { FirstName = "John", LastName = "Doe" };
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        hairdresser.PasswordHash = passwordHasher.HashPassword(hairdresser, "password123");

        _context!.ApplicationUser.Add(hairdresser);
        await _context.SaveChangesAsync();

        // Act
        var actionResult = await _hairdresserController!.GetHairdresserById(hairdresser.Id);
        var okResult = actionResult as OkObjectResult;

        Assert.IsNull(actionResult as NotFoundResult);

        Assert.IsNotNull(okResult, "Expected OkObjectResult but got null or different type.");

        var hairdresserResult = okResult.Value as ApplicationUser;
        Assert.IsNotNull(hairdresserResult, "Expected ApplicationUser but got null or different type.");

        // Assert
        Assert.AreEqual(hairdresser.Id, hairdresserResult.Id);
    }
}

