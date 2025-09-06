using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism.DataAccess.Models
{
    public class Package
    {
        [Key]
        public int PackageId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int AvailableSeats { get; set; }

        public string? ImageUrl { get; set; }

        // Computed duration (not stored in DB)
        [NotMapped]
        public int Duration => (EndDate - StartDate).Days;
    }
}
