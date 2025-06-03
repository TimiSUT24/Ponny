using System;
using Hairdresser.Data;
using Microsoft.EntityFrameworkCore;

namespace HairdresserUnitTests.Utils;

public static class Database
{
    /// <summary>
    /// Creates a new in-memory database connection for testing purposes.
    /// This method is used to set up a fresh database context for each test,
    /// ensuring that tests do not interfere with each other.
    /// </summary>
    /// <returns></returns>
    public static ApplicationDBContext? Connect()
    {
        var options = new DbContextOptionsBuilder<ApplicationDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDBContext(options);
    }
}
