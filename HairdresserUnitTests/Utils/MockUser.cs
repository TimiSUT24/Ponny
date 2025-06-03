using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace HairdresserUnitTests.utils;

public static class MockUser
{
    /// <summary>
    /// Initializes a mock UserManager for ApplicationUser.
    /// </summary>
    /// <returns></returns>
    public static Mock<UserManager<ApplicationUser>> InitializeUserManager()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        store.As<IUserEmailStore<ApplicationUser>>(); // Required for FindByEmailAsync

        return new Mock<UserManager<ApplicationUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );
    }
}
