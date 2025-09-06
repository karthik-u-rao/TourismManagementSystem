using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tourism.DataAccess.Models;

namespace Tourism.DataAccess   //must match what you used in Program.cs
{
    public class TourismDbContext : IdentityDbContext<ApplicationUser>
    {
        public TourismDbContext(DbContextOptions<TourismDbContext> options) : base(options) { }

        public DbSet<Package> Packages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // required for Identity tables

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Package)
                .WithMany()
                .HasForeignKey(b => b.PackageId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany()
                .HasForeignKey(p => p.BookingId);
        }
    }
}
