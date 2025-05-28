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
	public class BookingRepositoryTests
	{
		private ApplicationDBContext? _context;
		private BookingRepository? _bookingRepository;

		[TestInitialize]
		public void Setup()
		{
			// Set up in-memory database
			var options = new DbContextOptionsBuilder<ApplicationDBContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			_context = new ApplicationDBContext(options);
			_bookingRepository = new BookingRepository(_context);

			// Seed Application Users (Customer and Hairdresser)
			var customer = new ApplicationUser { Id = "1", UserName = "Kund" };
			var hairdresser = new ApplicationUser { Id = "2", UserName = "Frisör" };
			_context.Users.Add(customer);
			_context.Users.Add(hairdresser);

			// Seed Treatment
			var treatment = new Treatment { Id = 1, Name = "Haircut", Price = 200 };
			_context.Treatments.Add(treatment);

			// Seed a Booking
			_context.Bookings.Add(new Booking
			{
				Id = 1,
				CustomerId = customer.Id,
				HairdresserId = hairdresser.Id,
				TreatmentId = treatment.Id,
				Start = DateTime.Now,
				End = DateTime.Now.AddHours(1),
			});

			// Save all changes
			_context.SaveChanges();
		}


		[TestCleanup]
		public void Cleanup()
		{
			_context!.Database.EnsureDeleted();
			_context.Dispose();
		}


		[TestMethod]
		public async Task Add_ShouldAddBookingSuccessfully()
		{
			// Arrange
			var treatment = _context!.Treatments.ToList()[0];

			var customer = _context.Users.ToList()[0];
			var hairdresser = _context.Users.ToList()[1];

			var newBooking = new Booking
			{
				Id = 2,
				Customer = customer,
				Hairdresser = hairdresser,
				Treatment = treatment,
				Start = DateTime.Now,
				End = DateTime.Now.AddHours(1),
			};

			// Act
			await _bookingRepository!.AddAsync(newBooking);
			await _bookingRepository.SaveChangesAsync();
			var result = await _bookingRepository.GetByIdAsync(2);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(result.Customer, newBooking.Customer);
			Assert.AreEqual(result.Hairdresser, newBooking.Hairdresser);
			Assert.AreEqual(result.Treatment, newBooking.Treatment);
		}


		[TestMethod]
		public async Task Delete_ShouldDeleteBookingSuccessfully()
		{
			// Arrange
			var treatment = _context!.Treatments.ToList()[0];
			var customer = _context.Users.ToList()[0];
			var hairdresser = _context.Users.ToList()[1];

			var newBooking = new Booking
			{
				Id = 2,
				Customer = customer,
				Hairdresser = hairdresser,
				Treatment = treatment,
				Start = DateTime.Now,
				End = DateTime.Now.AddHours(1),
			};

			// Act
			await _bookingRepository!.AddAsync(newBooking);
			await _bookingRepository.SaveChangesAsync();
			var addedBooking = await _bookingRepository.GetByIdAsync(2);

			//Delete booking and save
			await _bookingRepository.DeleteAsync(newBooking);
			await _bookingRepository.SaveChangesAsync();

			var deletedBooking = await _bookingRepository.GetByIdAsync(2);

			// Assert
			// Make sure the booking is added successfully
			Assert.IsNotNull(addedBooking);			

			//Make sure the booking is deleted successfully
			Assert.IsNull(deletedBooking);
		}

	}
}
;