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




	}


}
