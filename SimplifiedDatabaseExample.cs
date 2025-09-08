// Simple User table instead of AspNetUsers
public class User
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } // "Admin" or "Customer"
    public DateTime CreatedDate { get; set; }
}

// Your simplified DbContext
public class SimpleTourismDbContext : DbContext
{
    public DbSet<User> Users { get; set; }          // Simple users table
    public DbSet<Package> Packages { get; set; }    // Your packages
    public DbSet<Booking> Bookings { get; set; }    // Your bookings  
    public DbSet<Payment> Payments { get; set; }    // Your payments
    
    // Only 4 tables total instead of 11!
}