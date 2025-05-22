using Hairdresser.Enums;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;

namespace Hairdresser.Data;

public static class UserRolesSeed
{
    public static async Task SeedUsersRoles(IServiceProvider serviceProvider)
    {
        var roleManger = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var adminEmail = "admin@admin.com";
        var adminPassword = "Admin123!";

        List<ApplicationUser> userSeeds = [
            new ApplicationUser()
                {
                    UserName = "ElinSvensson",
                    Email = "elin.svensson@example.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0701234567",
                    PhoneNumberConfirmed = true,
                },
                new ApplicationUser()
                {
                    UserName = "KarlAndersson",
                    Email = "karl.andersson@example.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0707654321",
                    PhoneNumberConfirmed = true,
                },
                new ApplicationUser()
                {
                    UserName = "SofiaLarsson",
                    Email = "sofia.l@domain.net",
                    EmailConfirmed = true,
                    PhoneNumber = "0709876543",
                    PhoneNumberConfirmed = true,

                },
                new ApplicationUser()
                {
                    UserName = "OskarCarlsson",
                    Email = "oskar.b@server.org",
                    EmailConfirmed = true,
                    PhoneNumber = "0706543210",
                    PhoneNumberConfirmed = true,
                },
                new ApplicationUser()
                {
                    UserName = "HannaNilsson",
                    Email = "hanna.n@provider.se",
                    EmailConfirmed = true,
                    PhoneNumber = "0701234567",
                    PhoneNumberConfirmed = true,
                }
        ];
        var roles = Enum.GetValues<UserRoleEnum>();

        foreach (var role in roles)
        {
            var roleExists = await roleManger.RoleExistsAsync(role.ToString());
            if (!roleExists)
            {
                await roleManger.CreateAsync(new IdentityRole(role.ToString()));
            }
        }

        var adminExists = userManager.Users.Any(u => u.Email == adminEmail);
        if (!adminExists)
        {
            var adminUser = new ApplicationUser()
            {
                Email = adminEmail,
                UserName = "Admin",
                EmailConfirmed = true,
                PhoneNumber = "0701234567",
                PhoneNumberConfirmed = true,
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, UserRoleEnum.Admin.ToString());
            }
            else
            {
                throw new Exception("Failed to create the admin user: " + string.Join(", ", result.Errors));
            }
        }

        foreach (var user in userSeeds)
        {
            var userExists = userManager.Users.Any(u => u.Email == user.Email);

            if (!userExists)
            {
                var result = await userManager.CreateAsync(user, "Password123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
                else
                {
                    throw new Exception("Failed to create the User user: " + string.Join(", ", result.Errors));
                }
            }

        }
    }
}
