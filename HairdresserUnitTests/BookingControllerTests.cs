using Hairdresser.Controllers;
using Hairdresser.Data;
using Hairdresser.DTOs;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairdresserUnitTests
{
	[TestClass]
	public class BookingControllerTests
	{
		private ApplicationDBContext? _context;
		private BookingsController? _controller;

		[TestInitialize]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<ApplicationDBContext>()
				.UseInMemoryDatabase("testdb")
				.Options;

			_context = new ApplicationDBContext(options);

			_context.Treatments.Add(new Treatment { Id = 1, Name = "Test Treatment", Duration = 30, Price = 100 });

			_controller = new BookingsController(_context);
		}

		[TestCleanup]
		public void Cleanup()
		{
			_context.Dispose();
		}

		[TestMethod]
		public async Task BookAppointment_ReturnsOkWithCorrectBookingData()
		{
			// Arrange
			var treatment = new Treatment { Id = 2, Duration = 30 };
			var bookingDTO = new BookingRequestDto
			{
				Start = DateTime.Now.AddDays(2),
				TreatmentId = treatment.Id,
				HairdresserId = "1",
				CustomerId = "1"
			};

			_context.Treatments.Add(treatment);
			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.BookAppointment(bookingDTO);

			// Assert
			if (result is CreatedAtActionResult createdResult)
			{
				BookingResponseDto response = (BookingResponseDto)createdResult.Value;
				Assert.IsNotNull(response.Id);
				Assert.AreEqual(bookingDTO.Start, response.Start);
			}
			else
			{
				Assert.Fail("Unexpected result type: " + result.GetType().Name);
			}
			Cleanup();
		}

		[TestMethod]
		public async Task Book_NonExisting_Treatment_Returns404()
		{
			// Arrange

			_context.Bookings.Add(new Booking
			{
				Id = 1,
				Start = DateTime.Now,
				End = DateTime.Now.AddMinutes(40),
				TreatmentId = 1,
				HairdresserId = "1",
				CustomerId = "1"
			});

			var bookingDTO = new BookingRequestDto
			{
				Start = DateTime.Now,
				TreatmentId = 5, // Non-existing treatment
				HairdresserId = "1",
				CustomerId = "1"
			};

			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.BookAppointment(bookingDTO);

			// Assert
			if (result is ConflictObjectResult conflictResult)
			{
				Assert.AreEqual(409, conflictResult.StatusCode);
			}
			else
			{
				Assert.Fail("Unexpected result type: " + result.GetType().Name);
			}

		}


		[TestMethod]
		public async Task Book_Occupied_Time_ReturnsConflict()
		{
			// Arrange
			var bookingDTO = new BookingRequestDto
			{
				Start = DateTime.Now.AddDays(2),
				TreatmentId = 5, // Non-existing treatment
				HairdresserId = "1",
				CustomerId = "1"
			};

			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.BookAppointment(bookingDTO);

			// Assert
			if (result is NotFoundObjectResult notFoundResult)
			{
				Assert.AreEqual(404, notFoundResult.StatusCode);
			}
			else
			{
				Assert.Fail("Unexpected result type: " + result.GetType().Name);
			}

		}

		[TestMethod]
		public async Task Book_InvalidTime_ReturnsConflict()
		{
			// Arrange
			var bookingDTO = new BookingRequestDto
			{
				Start = DateTime.Parse("2024-01-21"),
				TreatmentId = 1,
				HairdresserId = "1",
				CustomerId = "1"
			};

			await _context.SaveChangesAsync();

			// Act
			var result = await _controller.BookAppointment(bookingDTO);

			// Assert
			if (result is BadRequestObjectResult notFoundResult)
			{
				Assert.AreEqual(400, notFoundResult.StatusCode);
			}
			else
			{
				Assert.Fail("Unexpected result type: " + result.GetType().Name);
			}

		}


	}
}
