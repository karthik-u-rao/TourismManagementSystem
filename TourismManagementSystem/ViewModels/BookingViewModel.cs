using System.ComponentModel.DataAnnotations;

namespace TourismManagementSystem.ViewModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }

        // Package Info
        [Required]
        public int PackageId { get; set; }
        
        public string PackageName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        // Booking Info
        [Required]
        [Range(1, 10, ErrorMessage = "Number of seats must be between 1 and 10")]
        public int NumberOfSeats { get; set; }
        
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = string.Empty;

        // Customer Info
        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        // Payment Info
        public string PaymentStatus { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
