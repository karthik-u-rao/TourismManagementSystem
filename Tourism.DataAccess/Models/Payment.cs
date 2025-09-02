using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism.DataAccess.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        public Booking Booking { get; set; }

        [Column(TypeName = "decimal(18,2)")] 
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; } // Success, Failed, Refunded
    }
}
