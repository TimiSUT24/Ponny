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
                        Start = DateTime.Now.AddDays(1).AddHours(1),
                        End = DateTime.Now.AddDays(1).AddHours(1.5),
                    },
                    new Booking()
                    {
                        CustomerId = users[2].Id,
                        HairdresserId = users[3].Id,
                        TreatmentId = treatments[1].Id,
                        Start = DateTime.Now.AddDays(2).AddHours(1.25),
                        End = DateTime.Now.AddDays(2).AddHours(1.5), 
                    },
                    new Booking()
                    {
                        CustomerId = users[4].Id,
                        HairdresserId = users[0].Id,
                        TreatmentId = treatments[2].Id,
                        Start = DateTime.Now.AddDays(3).AddHours(2),
                        End = DateTime.Now.AddDays(3).AddHours(2.5),
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
                        Start = DateTime.Now.AddDays(1).AddHours(1),
                        End = DateTime.Now.AddDays(1).AddHours(1.5),
                    },
                    new Booking()
                    {
                        CustomerId = users[2].Id,
                        HairdresserId = users[3].Id,
                        TreatmentId = treatments[1].Id,
                        Start = DateTime.Now.AddDays(2).AddHours(1.25),
                        End = DateTime.Now.AddDays(2).AddHours(1.5), 
                    },
                    new Booking()
                    {
                        CustomerId = users[4].Id,
                        HairdresserId = users[0].Id,
                        TreatmentId = treatments[2].Id,
                        Start = DateTime.Now.AddDays(3).AddHours(2),
                        End = DateTime.Now.AddDays(3).AddHours(2.5),
                    }
               ];


        if (!booking.Any())
        {
            await booking.AddRangeAsync(bookingSeeds);
            await context.SaveChangesAsync();
        }
    }
}
