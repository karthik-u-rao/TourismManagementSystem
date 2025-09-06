using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism.DataAccess.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [ForeignKey("Package")]
        public int PackageId { get; set; }
        public Package Package { get; set; } = null!;

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public int NumberOfSeats { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Booked"; // default value

        // 👤 Customer Info (nullable fixes applied)
        [Required, MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        // 💳 Navigation to Payments (plural, not Payment)
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
