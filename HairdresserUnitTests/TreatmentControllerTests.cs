using Hairdresser.Controllers;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hairdresser.Services.Interfaces;

namespace HairdresserUnitTests
{
    [TestClass]
    public class TreatmentControllerTests
    {
        private Mock<ITreatmentService> _mockService = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<ITreatmentService>();
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

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(treatments);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult!.Value as IEnumerable<Treatment>;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(2, returnValue.Count());
        }

        [TestMethod]
        public async Task Create_ValidTreatment_ReturnsCreated()
        {
            // Arrange
            var dto = new Treatment
            {
                Name = "Wash",
                Description = "Shampoo and conditioning",
                Duration = 15,
                Price = 150
            };

            _mockService
                .Setup(s => s.CreateAsync(It.IsAny<Treatment>()))
                .ReturnsAsync((Treatment treatment) => treatment);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            _mockService.Verify(s => s.CreateAsync(It.IsAny<Treatment>()), Times.Once);

            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            var returnValue = createdResult!.Value as Treatment;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(dto.Name, returnValue!.Name);
        }

        [TestMethod]
        public async Task Update_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var dto = new Treatment
            {
                Id = 999,
                Name = "Update",
                Description = "Updated Desc",
                Duration = 45,
                Price = 500
            };

            _mockService.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(999, dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Delete_ValidId_DeletesTreatment()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            _mockService.Verify(s => s.DeleteAsync(1), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(99);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}