using System;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Data;

public static class BookingSeed
{
    public static void SeedBookings(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var booking = context.Set<Booking>();
        var users = userManager.Users.ToListAsync().Result;
        var treatments = context.Set<Treatment>().ToListAsync().Result;

        List<Booking> bookingSeeds = [
               new Booking()
                    {
                        CustomerId = users[0].Id,
                        HairdresserId = users[1].Id,
                        TreatmentId = treatments[0].Id,
                        Start = DateTime.Parse("2025-05-14T10:00:00"),
                        End = DateTime.Parse("2025-05-14T11:00:00"),
                    },
                    new Booking()
                    {
                        CustomerId = users[2].Id,
                        HairdresserId = users[3].Id,
                        TreatmentId = treatments[1].Id,
                        Start = DateTime.Parse("2025-05-16T10:00:00"),
                        End = DateTime.Parse("2025-05-16T11:30:00"),
                    },
                    new Booking()
                    {
                        CustomerId = users[4].Id,
                        HairdresserId = users[0].Id,
                        TreatmentId = treatments[2].Id,
                        Start = DateTime.Parse("2025-05-17T10:00:00"),
                        End = DateTime.Parse("2025-05-17T12:00:00"),
                    }
               ];


        if (!booking.Any())
        {
            booking.AddRange(bookingSeeds);
            context.SaveChanges();
        }
    }
    public static async Task SeedBookingsAsync(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();


        var booking = context.Set<Booking>();
        var users = await userManager.Users.ToListAsync();
        var treatments = context.Set<Treatment>().ToListAsync().Result;

        List<Booking> bookingSeeds = [
               new Booking()
                    {
                        CustomerId = users[0].Id,
                        HairdresserId = users[1].Id,
                        TreatmentId = treatments[0].Id,
                        Start = DateTime.Parse("2025-05-14T10:00:00"),
                        End = DateTime.Parse("2025-05-14T11:00:00"),
                    },
                    new Booking()
                    {
                        CustomerId = users[2].Id,
                        HairdresserId = users[3].Id,
                        TreatmentId = treatments[1].Id,
                        Start = DateTime.Parse("2025-05-16T10:00:00"),
                        End = DateTime.Parse("2025-05-16T11:30:00"),
                    },
                    new Booking()
                    {
                        CustomerId = users[4].Id,
                        HairdresserId = users[0].Id,
                        TreatmentId = treatments[2].Id,
                        Start = DateTime.Parse("2025-05-17T10:00:00"),
                        End = DateTime.Parse("2025-05-17T12:00:00"),
                    }
               ];


        if (!booking.Any())
        {
            await booking.AddRangeAsync(bookingSeeds);
            await context.SaveChangesAsync();
        }
    }
}
