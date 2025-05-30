using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; } = null!;
        public DbSet<Treatment> Treatments { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Booking ? Customer
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(u => u.CustomerBookings)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking ? Hairdresser
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Hairdresser)
                .WithMany(u => u.HairdresserBookings)
                .HasForeignKey(b => b.HairdresserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking ? Treatment
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Treatment)
                .WithMany(t => t.TreatmentBookings)
                .HasForeignKey(b => b.TreatmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
