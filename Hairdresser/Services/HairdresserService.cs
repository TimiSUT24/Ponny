using Hairdresser.Enums;
using Hairdresser.Mapping;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.DTOs.User;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using HairdresserClassLibrary.DTOs;

namespace Hairdresser.Services
{
    public class HairdresserService : IHairdresserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public HairdresserService(IUserRepository userRepository, IBookingRepository bookingRepo, UserManager<ApplicationUser> userManager)
        {
            _userRepo = userRepository;
            _bookingRepo = bookingRepo;
            _userManager = userManager;
        }

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
            var bookings = await _bookingRepo.GetWeekScheduleWithDetailsAsync(hairdresserId, weekStart);
            return ConvertToDtoList(bookings);
        }

        public async Task<IEnumerable<BookingResponseDto>> GetMonthlyScheduleAsync(string hairdresserId, int year, int month)
        {
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
                Treatment = booking.Treatment,
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
            hairdresser.UserName = userRequest.Email;

            await _userRepo.UpdateAsync(hairdresser);
            await _userRepo.SaveChangesAsync();

            return hairdresser.MapToUserDTO();
        }

        public async Task<UserDto> GetHairdresserWithId(string id)
        {
            var allHairdresser = await _userManager.GetUsersInRoleAsync(UserRoleEnum.Hairdresser.ToString());
            var hairdresser = allHairdresser.Where(u => u.Id == id).FirstOrDefault();

            if (hairdresser == null)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            return new UserDto
            {
                UserName = hairdresser.UserName,
                Email = hairdresser.Email,
            };
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

        /*private async Task<ApplicationUser?> GetUserByRoleAsync(string id, UserRoleEnum userRole)
        {
           var roleId = await _context.Roles
               .Where(r => r.Name == userRole.ToString())
               .Select(r => r.Id)
               .FirstOrDefaultAsync();
           if (roleId == null)
           {
               return null;
           }
           var userId = await _context.UserRoles
               .Where(ur => ur.RoleId == roleId && ur.UserId == id)
               .Select(ur => ur.UserId)
               .FirstOrDefaultAsync();

           if (userId is null)
           {
               return null;
           }

           return await _userRepo.GetByIdAsync(userId);
       } */ // fix denna 
    }
}