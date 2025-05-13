using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Data;

public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
{
	public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
		: base(options)
	{
	}
	public DbSet<Treatment> Treatments { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;
	public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Add any additional configuration here
    }
}