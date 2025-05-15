using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Data;

public static class UserSeed
{
    public static void SeedUsers(DbContext context)
    {
        var hasher = new PasswordHasher<ApplicationUser>();
        var password = hasher.HashPassword(new ApplicationUser(), "Password123!");

        // modelBuilder.Entity<ApplicationUser>()
        //     .HasData(
        //         new ApplicationUser()
        //         {
        //             Id = "1",
        //             Email = "elin.svensson@example.com",
        //             PasswordHash = password,
        //             EmailConfirmed = true,
        //             PhoneNumber = "0701234567",
        //             PhoneNumberConfirmed = true,
        //         },
        //         new ApplicationUser()
        //         {
        //             Id = "2",
        //             Email = "karl.andersson@example.com",
        //             PasswordHash = password,
        //             EmailConfirmed = true,
        //             PhoneNumber = "0707654321",
        //             PhoneNumberConfirmed = true,
        //         },
        //         new ApplicationUser()
        //         {
        //             Id = "3",
        //             Email = "sofia.l@domain.net",
        //             PasswordHash = password,
        //             EmailConfirmed = true,
        //             PhoneNumber = "0709876543",
        //             PhoneNumberConfirmed = true,

        //         },
        //         new ApplicationUser()
        //         {
        //             Id = "4",
        //             Email = "oskar.b@server.org",
        //             PasswordHash = password,
        //             EmailConfirmed = true,
        //             PhoneNumber = "0706543210",
        //             PhoneNumberConfirmed = true,
        //         },
        //         new ApplicationUser()
        //         {
        //             Id = "5",
        //             Email = "hanna.n@provider.se",
        //             PasswordHash = password,
        //             EmailConfirmed = true,
        //             PhoneNumber = "0701234567",
        //             PhoneNumberConfirmed = true,
        //         }
        //     );

        var user = context.Set<ApplicationUser>();
        List<ApplicationUser> userSeeds = [
            new ApplicationUser()
                {
                    Email = "elin.svensson@example.com",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    PhoneNumber = "0701234567",
                    PhoneNumberConfirmed = true,
                },
                new ApplicationUser()
                {
                    Email = "karl.andersson@example.com",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    PhoneNumber = "0707654321",
                    PhoneNumberConfirmed = true,
                },
                new ApplicationUser()
                {
                    Email = "sofia.l@domain.net",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    PhoneNumber = "0709876543",
                    PhoneNumberConfirmed = true,

                },
                new ApplicationUser()
                {
                    Email = "oskar.b@server.org",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    PhoneNumber = "0706543210",
                    PhoneNumberConfirmed = true,
                },
                new ApplicationUser()
                {
                    Email = "hanna.n@provider.se",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    PhoneNumber = "0701234567",
                    PhoneNumberConfirmed = true,
                }
        ];
        if (!user.Any())
        {
            user.AddRange(userSeeds);
            context.SaveChanges();
        }
    }
    public static async Task SeedUsersAsync(DbContext context)
    {
        var hasher = new PasswordHasher<ApplicationUser>();
        var password = hasher.HashPassword(new ApplicationUser(), "Password123!");
        var user = context.Set<ApplicationUser>();
        List<ApplicationUser> userSeeds = [
            new ApplicationUser()
                {
                    Email = "elin.svensson@example.com",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    PhoneNumber = "0701234567",
                    PhoneNumberConfirmed = true,
                },
                new ApplicationUser()
                {
                    Email = "karl.andersson@example.com",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    PhoneNumber = "0707654321",
                    PhoneNumberConfirmed = true,
                },
                new ApplicationUser()
                {
                    Email = "sofia.l@domain.net",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    PhoneNumber = "0709876543",
                    PhoneNumberConfirmed = true,

                },
                new ApplicationUser()
                {
                    Email = "oskar.b@server.org",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    PhoneNumber = "0706543210",
                    PhoneNumberConfirmed = true,
                },
                new ApplicationUser()
                {
                    Email = "hanna.n@provider.se",
                    PasswordHash = password,
                    EmailConfirmed = true,
                    PhoneNumber = "0701234567",
                    PhoneNumberConfirmed = true,
                }
        ];
        if (!user.Any())
        {
            await user.AddRangeAsync(userSeeds);
            await context.SaveChangesAsync();
        }
    }
}
