using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Company.TestProject1;

[TestClass]
public class TreatmentServiceTest
{
    private Mock<IGenericRepository<Treatment>> _treatmentRepo = null!;
    private ITreatmentService _TreatmentService;

    [TestInitialize]
    public void Setup()
    {
        _treatmentRepo = new Mock<IGenericRepository<Treatment>>();
        _TreatmentService = new TreatmentService(_treatmentRepo.Object);
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnAllTreatments()
    {
        // Arrange
        var treatments = new List<Treatment>
        {
            new Treatment { Id = 1, Name = "Haircut", Duration = 30, Price = 20.0 },
            new Treatment { Id = 2, Name = "Coloring", Duration = 60, Price = 50.0 }
        };

        _treatmentRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(treatments);

        // Act
        var result = await _TreatmentService.GetAllAsync();
        var expected = 2;
        
        // Assert

        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());
    }
}
