using Hairdresser.Enums;
using Hairdresser.Mapping;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.DTOs.User;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using HairdresserClassLibrary.DTOs;

namespace Hairdresser.Services;

public class UserService(IUserRepository userRepository, IBookingRepository bookingRepo, UserManager<ApplicationUser> userManager) : IUserService
{
    private readonly IUserRepository _userRepo = userRepository;
    private readonly IBookingRepository _bookingRepo = bookingRepo;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<IEnumerable<UserResponseDto>> GetAllHairdressersAsync()
    {
        var users = await _userRepo.GetAllAsync();

        return users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Role = "Hairdresser"
        });
    }

    public async Task<IEnumerable<BookingResponseDto>> GetWeekScheduleAsync(string hairdresserId, DateTime weekStart)
    {
        if (string.IsNullOrWhiteSpace(hairdresserId))
        {
            throw new ArgumentException("Id cannot be null or whitespace.", nameof(hairdresserId));
        }

        if (weekStart.Date < DateTime.Now.Date)
        {
            return [];
        }
        var bookings = await _bookingRepo.GetWeekScheduleWithDetailsAsync(hairdresserId, weekStart);
        return ConvertToDtoList(bookings);
    }

    public async Task<IEnumerable<BookingResponseDto>> GetMonthlyScheduleAsync(string hairdresserId, int year, int month)
    {
        var currentYearAndOneMonth = DateTime.Now.AddMonths(1).Year;

        if (string.IsNullOrWhiteSpace(hairdresserId))
        {
            throw new ArgumentException("Id cannot be null or whitespace.", nameof(hairdresserId));
        }
        if (year != currentYearAndOneMonth || month < 1 || month > 12)
        {
            return [];
        }
        var bookings = await _bookingRepo.GetMonthlyScheduleWithDetailsAsync(hairdresserId, year, month);
        return ConvertToDtoList(bookings);
    }
    public async Task<BookingResponseDto?> GetBookingDetailsAsync(int bookingId)
    {
        var booking = await _bookingRepo.GetBookingWithDetailsAsync(bookingId);
        if (booking == null)
        {
            return null;
        }
       
        return new BookingResponseDto
        {
            Id = booking.Id,
            Start = booking.Start,
            End = booking.End,
            Treatment = new TreatmentDto
            {
                Name = booking.Treatment.Name,
                Description = booking.Treatment.Description,
                Duration = booking.Treatment.Duration,
                Price = booking.Treatment.Price,

            },
            Costumer = new UserDto
            {
                UserName = booking.Customer.UserName,
                Email = booking.Customer?.Email,
                PhoneNumber = booking.Customer?.PhoneNumber
            },
        };
    }

    public async Task<UserDto?> UpdateHairdresserAsync(string id, UpdateUserDto userRequest)
    {
        var allHairdresser = await _userManager.GetUsersInRoleAsync(UserRoleEnum.Hairdresser.ToString());
        var hairdresser = allHairdresser.Where(u => u.Id == id).FirstOrDefault();

        if (hairdresser is null)
        {
            return null;
        }

        hairdresser.FirstName = userRequest.FirstName;
        hairdresser.LastName = userRequest.LastName;
        hairdresser.Email = userRequest.Email;
        hairdresser.PhoneNumber = userRequest.PhoneNumber;
        hairdresser.UserName = userRequest.UserName;

        await _userRepo.UpdateAsync(hairdresser);
        await _userRepo.SaveChangesAsync();

        return hairdresser.MapToUserDTO();
    }

    public async Task<UserDto> GetHairdresserWithId(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Id cannot be null or whitespace.", nameof(id));
        }

        var allHairdresser = await _userManager.GetUsersInRoleAsync(UserRoleEnum.Hairdresser.ToString());
        var hairdresser = allHairdresser.FirstOrDefault(u => u.Id == id);

        if (hairdresser == null)
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }

        return hairdresser.MapToUserDTO();
    }

    private List<BookingResponseDto> ConvertToDtoList(IEnumerable<Booking> bookings)
    {
        return bookings.Select(b => new BookingResponseDto
        {
            Id = b.Id,
            Start = b.Start,
            End = b.End,
            Treatment = new TreatmentDto //add more? 
            {
                Name = b.Treatment.Name,
                Description = b.Treatment.Description,
            },
            Costumer = new UserDto
            {
                UserName = b.Customer?.UserName,
                Email = b.Customer?.Email,
                PhoneNumber = b.Customer?.PhoneNumber
            },
            Hairdresser = new UserDto
            {
                Id = b.Hairdresser?.Id,
                FirstName = b.Hairdresser?.FirstName,
                LastName = b.Hairdresser?.LastName,
                UserName = b.Hairdresser?.UserName,
                Email = b.Hairdresser?.Email,
                PhoneNumber = b.Hairdresser?.PhoneNumber
            }
        }).ToList();
    }

    public async Task<IEnumerable<BookingResponseDto>> GetAllBookingsOverviewAsync()
    {
        var bookings = await _bookingRepo.GetAllAsync();

        var bookingResponseDto = ConvertToDtoList(bookings);

        return bookingResponseDto;
    }
}
