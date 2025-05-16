using Hairdresser.Data;
using Hairdresser.Repositories;
using HairdresserClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairdresserUnitTests
{
	[TestClass]
	public class TreatmentRepositoryTests
	{
		private ApplicationDBContext? _context;
		private TreatmentRepository? _treatmentRepository;

		[TestInitialize]
		public void Setup()
		{
			// Set up in-memory database
			var options = new DbContextOptionsBuilder<ApplicationDBContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			_context = new ApplicationDBContext(options);
			_treatmentRepository = new TreatmentRepository(_context);
		}


		[TestCleanup]
		public void Cleanup()
		{
			_context!.Database.EnsureDeleted();
			_context.Dispose();
		}

		[TestMethod]
		public async Task Add_ShoudAddTreatmentSuccessfully()
		{
			//Arrage
			var treatments = await _treatmentRepository!.GetAllAsync();

			//Act
			var newTreatment = new Treatment
			{
				Id = 1,
				Name = "Haircut",
				Price = 200
			};

			// Act
			await _treatmentRepository!.AddAsync(newTreatment);
			await _treatmentRepository.SaveChangesAsync();
			var result = await _treatmentRepository.GetByIdAsync(1);

			//Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task Delete_ShoudDeleteTreatmentSuccessfully()
		{
			//Arrage
			var newTreatment = new Treatment
			{
				Id = 1,
				Name = "Haircut",
				Price = 200
			};
			await _treatmentRepository!.AddAsync(newTreatment);
			await _treatmentRepository.SaveChangesAsync();



			//Act

			var addedTreatment = await _treatmentRepository.GetByIdAsync(1);
			await _treatmentRepository.DeleteAsync(addedTreatment!);
			await _treatmentRepository.SaveChangesAsync();
			var DeletedTreatment = await _treatmentRepository.GetByIdAsync(1);



			//Assert
			Assert.IsNotNull(addedTreatment);
			Assert.IsNull(DeletedTreatment);
		}

		[TestMethod]
		public async Task GetAllAsync_ShouldReturnAllTreatments()
		{
			// Arrange
			var initialTreatments = await _treatmentRepository!.GetAllAsync();
			Assert.AreEqual(0, initialTreatments.Count());

			// Add multiple treatments
			var treatment1 = new Treatment { Id = 1, Name = "Haircut", Price = 200 };
			var treatment2 = new Treatment { Id = 2, Name = "Hair Coloring", Price = 300 };
			var treatment3 = new Treatment { Id = 3, Name = "Hair Wash", Price = 100 };

			await _treatmentRepository.AddAsync(treatment1);
			await _treatmentRepository.AddAsync(treatment2);
			await _treatmentRepository.AddAsync(treatment3);
			await _treatmentRepository.SaveChangesAsync();

			// Act
			var result = await _treatmentRepository.GetAllAsync();

			// Assert
			Assert.AreEqual(3, result.Count());
			Assert.IsTrue(result.Any(t => t.Name == "Haircut"));
			Assert.IsTrue(result.Any(t => t.Name == "Hair Coloring"));
			Assert.IsTrue(result.Any(t => t.Name == "Hair Wash"));
		}

		[TestMethod]
		public async Task GetByIdAsync_ShouldReturnCorrectTreatment()
		{
			// Arrange
			var treatment1 = new Treatment { Id = 1, Name = "Haircut", Price = 200 };
			var treatment2 = new Treatment { Id = 2, Name = "Hair Coloring", Price = 300 };

			await _treatmentRepository!.AddAsync(treatment1);
			await _treatmentRepository.AddAsync(treatment2);
			await _treatmentRepository.SaveChangesAsync();

			// Act
			var result = await _treatmentRepository.GetByIdAsync(2);
			var nonExistentResult = await _treatmentRepository.GetByIdAsync(999);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result!.Id);
			Assert.AreEqual("Hair Coloring", result.Name);
			Assert.AreEqual(300, result.Price);

			// Non existent ID should return null
			Assert.IsNull(nonExistentResult);
		}

		[TestMethod]
		public async Task UpdateAsync_ShouldUpdateTreatmentSuccessfully()
		{
			// Arrange
			var treatment = new Treatment { Id = 1, Name = "Haircut", Price = 200 };
			await _treatmentRepository!.AddAsync(treatment);
			await _treatmentRepository.SaveChangesAsync();

			// Act

			var treatmentToUpdate = await _treatmentRepository.GetByIdAsync(1);
			Assert.IsNotNull(treatmentToUpdate);

			//update the treatment
			treatmentToUpdate!.Name = "Premium Haircut";
			treatmentToUpdate.Price = 500;

			await _treatmentRepository.UpdateAsync(treatmentToUpdate);
			await _treatmentRepository.SaveChangesAsync();

			var updatedTreatment = await _treatmentRepository.GetByIdAsync(1);

			// Assert
			Assert.IsNotNull(updatedTreatment);
			Assert.AreEqual(1, updatedTreatment!.Id);
			Assert.AreEqual("Premium Haircut", updatedTreatment.Name);
			Assert.AreEqual(500, updatedTreatment.Price);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task AddAsync_ShouldThrowException_WhenEntityIsNull()
		{
			// Act
			await _treatmentRepository!.AddAsync(null);
		}
	}


}
