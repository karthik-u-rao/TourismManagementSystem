namespace TourismManagementSystem.ViewModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }

        // Package Info
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }   // added

        // Booking Info
        public int NumberOfSeats { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }

        // Customer Info
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Payment Info
        public string PaymentStatus { get; set; }
        public decimal Amount { get; set; }
    }
}
