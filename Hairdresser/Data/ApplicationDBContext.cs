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

		modelBuilder.Entity<Booking>()
			.HasOne(b => b.Customer)
			.WithMany()
			.HasForeignKey(b => b.CustomerId);

		modelBuilder.Entity<Booking>()
			.HasOne(b => b.Hairdresser)
			.WithMany()
			.HasForeignKey(b => b.HairdresserId);
		modelBuilder.Entity<Booking>()
			.HasOne(b => b.Treatment)
			.WithMany(t => t.Bookings)
			.HasForeignKey(b => b.TreatmentId);
	}
}