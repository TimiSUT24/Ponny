using Hairdresser.Controllers;
using Hairdresser.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Company.TestProject1;

[TestClass]
public class HairdresserControllerTest
{
    private ApplicationDBContext? _context;
    private HairdresserController? _hairdresserController;

    [TestInitialize]
    public void Setup()
    {
        // Set up in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDBContext(options);
        _hairdresserController = new HairdresserController(_context, null);
    }
}
