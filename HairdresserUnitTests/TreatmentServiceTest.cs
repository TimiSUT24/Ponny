using System.Linq.Expressions;
using Hairdresser.Mapping;
using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.DTOs;
using HairdresserClassLibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HairdresserUnitTests;

[TestClass]
public class TreatmentServiceTest
{
    private Mock<IGenericRepository<Treatment>> _treatmentRepo = null!;
    private ITreatmentService _TreatmentService = null!;

    [TestInitialize]
    public void Setup()
    {
        _treatmentRepo = new Mock<IGenericRepository<Treatment>>();
        _TreatmentService = new TreatmentService(_treatmentRepo.Object);
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnAllTreatments()
    {
        // Arrange - Mocking the repository to return a list of treatments
        var treatments = new List<Treatment>
        {
            new Treatment { Id = 1, Name = "Haircut", Duration = 30, Price = 20.0 },
            new Treatment { Id = 2, Name = "Coloring", Duration = 60, Price = 50.0 }
        };

        _treatmentRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(treatments);

        // Act - Fetching all treatments using the service
        var result = await _TreatmentService.GetAllAsync();
        var expected = 2;

        // Assert - Verifying that the result is not null and contains the expected number of treatments
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnTreatment_WhenExists()
    {
        // Arrange - Mocking the repository to return a specific treatment
        var treatment = new Treatment { Id = 1, Name = "Haircut", Duration = 30, Price = 20.0 };
        _treatmentRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(treatment);

        // Act - Fetching the treatment by ID using the service
        var result = await _TreatmentService.GetByIdAsync(1);
        var expected = treatment;

        // Assert - Verifying that the result is not null and matches the expected treatment
        Assert.IsNotNull(result);
        Assert.AreSame(expected, result);
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange - Mocking the repository to return null for a non-existing treatment
        _treatmentRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Treatment?)null);

        // Act - Fetching a treatment by ID using the service
        var idNotExist = 999; // Assuming this ID does not exist
        var result = await _TreatmentService.GetByIdAsync(idNotExist);

        // Assert - Verifying that the result is null
        Assert.IsNull(result);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(int.MinValue)]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdIsZeroOrless(int id)
    {
        // Arrange - Mocking the repository to return null for any ID less than or equal to zero
        var treatment = null as Treatment;
        _treatmentRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(treatment);

        // Act - Fetching a treatment by an invalid ID using the service
        var result = await _TreatmentService.GetByIdAsync(id);

        // Assert - Verifying that the result is null
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task CreateAsync_ShouldAddTreatment()
    {
        // Arrange - Mocking the repository to expect an AddAsync call
        var expected = new CreateTreatmentDTO
        {
            Name = "Shampoo",
            Description = "This shampoo gently cleanses your hair, leaving it fresh and shiny.",
            Duration = 15,
            Price = 10.0,
        };
        // Act - Calling the CreateAsync method of the service
        var result = await _TreatmentService.CreateAsync(expected);

        // Assert - Verifying that the result is not null, matches the expected treatment, and that the repository methods were called
        Assert.IsNotNull(result);
        Assert.AreEqual(expected.Name, result.Name);
        Assert.AreEqual(expected.Description, result.Description);
        Assert.AreEqual(expected.Duration, result.Duration);
        Assert.AreEqual(expected.Price, result.Price);
        _treatmentRepo.Verify(repo => repo.AddAsync(It.IsAny<Treatment>()), Times.Once);
        _treatmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldReturnTrue_WhenTreatmentExists()
    {
        // Arrange
        var id = 1;
        var treatmentToUpdate = new TreatmentUpdateDto
        {
            Name = "Haircut",
            Description = "A stylish haircut",
            Duration = 30,
            Price = 20.0
        };

        _treatmentRepo.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Treatment, bool>>>()))
            .ReturnsAsync(true);

        // Act - Updating the treatment using the service
        var result = await _TreatmentService.UpdateAsync(id, treatmentToUpdate);

        // Assert - Verifying that the result is true and the repository methods were called
        Assert.IsTrue(result);
        _treatmentRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Treatment>()), Times.Once);
        _treatmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldReturnFalse_WhenTreatmentDoesNotExist()
    {
        // Arrange
        var idNotExist = 999; // Assuming this ID does not exist
        var treatmentToUpdate = new TreatmentUpdateDto
        {
            Name = "Haircut",
            Description = "A stylish haircut",
            Duration = 30,
            Price = 20.0
        };

        _treatmentRepo.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Treatment, bool>>>()))
            .ReturnsAsync(false);
        // Act - Attempting to update a treatment that does not exist
        var result = await _TreatmentService.UpdateAsync(idNotExist, treatmentToUpdate);

        // Arranging
        Assert.IsFalse(result);
        _treatmentRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Treatment>()), Times.Never);
        _treatmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(int.MinValue)]
    public async Task UpdateAsync_ShouldReturnFalse_WhenIdIsZeroOrLess(int idNotExist)
    {
        // Arrange - Mocking the repository to return false for any ID less than or equal to zero
        var treatmentToUpdate = new TreatmentUpdateDto
        {
            Name = "Haircut",
            Description = "A stylish haircut",
            Duration = 30,
            Price = 20.0
        };

        // Act - Attempting to update a treatment with an invalid ID
        var result = await _TreatmentService.UpdateAsync(idNotExist, treatmentToUpdate);

        // Assert - Verifying that the result is false and no repository methods were called
        Assert.IsFalse(result);
        _treatmentRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Treatment>()), Times.Never);
        _treatmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        _treatmentRepo.Verify(repo => repo.AnyAsync(It.IsAny<Expression<Func<Treatment, bool>>>()), Times.Never);
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldReturnTrue_WhenTreatmentHaveBeanRemoved()
    {
        // Arrange - Mocking the repository to expect a DeleteAsync call
        var id = 1;
        var treatmentToDelate = new Treatment
        {
            Name = "Haircut",
            Description = "A stylish haircut",
            Duration = 30,
            Price = 20.0
        };

        _treatmentRepo.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Treatment, bool>>>()))
            .ReturnsAsync(true);
        _treatmentRepo.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(treatmentToDelate);

        // Act - Deleting the treatment using the service
        var result = await _TreatmentService.DeleteAsync(id);

        // Assert - Verifying that the result is true and the repository methods were called
        Assert.IsTrue(result);
        _treatmentRepo.Verify(repo => repo.DeleteAsync(It.IsAny<Treatment>()), Times.Once);
        _treatmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldReturnFalse_WhenTreatmentDoesNotExist()
    {
        // Arrange - Mocking the repository to return false for a non-existing treatment
        _treatmentRepo.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Treatment, bool>>>()))
            .ReturnsAsync(false);
        var idNotExist = 999;

        // Act - Attempting to delete a treatment that does not exist
        var result = await _TreatmentService.DeleteAsync(idNotExist);

        // Assert - Verifying that the result is false and no repository methods were called
        Assert.IsFalse(result);
        _treatmentRepo.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _treatmentRepo.Verify(repo => repo.DeleteAsync(It.IsAny<Treatment>()), Times.Never);
        _treatmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    [DataRow(int.MinValue)]
    public async Task DeleteAsync_ShouldReturnFalse_WhenIdIsZeroOrLess(int idNotExist)
    {
        // Act - Attempting to delete a treatment with an invalid ID
        var result = await _TreatmentService.DeleteAsync(idNotExist);

        // Assert - Verifying that the no repository methods were called
        Assert.IsFalse(result);
        _treatmentRepo.Verify(repo => repo.DeleteAsync(It.IsAny<Treatment>()), Times.Never);
        _treatmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        _treatmentRepo.Verify(repo => repo.AnyAsync(It.IsAny<Expression<Func<Treatment, bool>>>()), Times.Never);
    }
}
