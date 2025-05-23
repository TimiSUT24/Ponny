using HairdresserClassLibrary.Models;
using Hairdresser.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hairdresser.DTOs;
using Hairdresser.Repositories.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Hairdresser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {         
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {          

            _bookingService = bookingService; 
        }

        // Get all available times for a hairdresser
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        [HttpGet("Available-times")]
        public async Task<IActionResult> GetAvailableTimes(string hairdresserId, int treatmentId, DateTime day)
        {
            try
            {
                var availableTimes = await _bookingService.GetAllAvailableTimes(hairdresserId, treatmentId, day);
                return Ok(availableTimes);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }
        //Get booking for user
        [ProducesResponseType(typeof(List<BookingResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        [HttpGet("BookingsById")]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User is not logged in.");
                }

                var booking = await _bookingService.GetBookingByIdAsync(bookingId, userId);
                if (booking == null)
                {
                    return NotFound("Booking was not found");
                }
                return Ok(booking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }                                   
        }

        // Book an appointment
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Authorize]
        [HttpPost("Book Appointment")]
        public async Task<IActionResult> BookAppointment([FromBody] BookingRequestDto request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User is not logged in.");
                }                  
                var booking = await _bookingService.BookAppointment(userId, request);
                if (booking == null)
                {
                    return NotFound("Booking was not found"); 
                }
                return CreatedAtAction(nameof(GetBookingById), booking);
                
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Cancel a booking
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        [HttpDelete("Cancel Booking")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
           try
           {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User is not logged in.");
                }
                var booking = await _bookingService.CancelBooking(userId, bookingId);
                if(booking == null)
                {
                    return NotFound("Booking was not cancelled");
                }
                return Ok(booking);
           }
           catch (KeyNotFoundException ex)
           {
                return NotFound(ex.Message);
           }
           catch (UnauthorizedAccessException ex)
           {
                return Forbid(ex.Message);
           }
           catch (Exception ex)
           {
                return BadRequest(ex.Message);
           }
        }

        // Rebook a booking
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPut("Reschedule")]
        public async Task<IActionResult> Rebook(int bookingId, BookingRequestDto request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized("User is not logged in.");
                }

                var booking = await _bookingService.RebookBooking(userId, bookingId, request);

                if (booking == null)
                {
                    return NotFound("Booking was not found");
                }

                return Ok(booking);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
