using System;
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

        public Package Package { get; set; }   // Navigation property

        public DateTime BookingDate { get; set; }

        [Range(1, int.MaxValue)]
        public int NumberOfSeats { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; }  // Booked, Cancelled
    }
}
