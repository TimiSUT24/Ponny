using Hairdresser.Controllers;
using Hairdresser.DTOs;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HairdresseerUnitTests
{
    [TestClass]
    public class TreatmentControllerTests
    {
        private Mock<IGenericRepository<Treatment>> _mockRepo = null!;
        private TreatmentController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IGenericRepository<Treatment>>();
            _controller = new TreatmentController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsAllTreatments()
        {
            // Arrange
            var treatments = new List<Treatment>
            {
                new() { Id = 1, Name = "Cut", Description = "Basic haircut", Duration = 30, Price = 250 },
                new() { Id = 2, Name = "Color", Description = "Hair coloring", Duration = 60, Price = 800 }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(treatments);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as IEnumerable<TreatmentDto>;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(2, returnValue.Count());
        }

        [TestMethod]
        public async Task Create_ValidTreatment_ReturnsCreated()
        {
            // Arrange
            var dto = new TreatmentCreateUpdateDto
            {
                Name = "Wash",
                Description = "Shampoo and conditioning",
                Duration = 15,
                Price = 150
            };

            // Act
            var result = await _controller.Create(dto);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Treatment>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);

            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            var returnValue = createdResult.Value as TreatmentDto;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(dto.Name, returnValue.Name);
        }

        [TestMethod]
        public async Task Update_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Treatment?)null);

            var dto = new TreatmentCreateUpdateDto
            {
                Name = "Update",
                Description = "Updated Desc",
                Duration = 45,
                Price = 500
            };

            // Act
            var result = await _controller.Update(999, dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Delete_ValidId_DeletesTreatment()
        {
            // Arrange
            var treatment = new Treatment { Id = 1, Name = "To be deleted" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(treatment);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            _mockRepo.Verify(r => r.DeleteAsync(treatment), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
