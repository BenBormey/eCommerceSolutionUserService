using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Entities
{
    public class Review
    {
        [Key] public int ReviewId { get; set; }

        // FK → Booking
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        // FK → Users
        public Guid CustomerId { get; set; }       // reviewer
        public ApplicationUser Customer { get; set; } = null!;

        public Guid CleanerId { get; set; }        // reviewed cleaner
        public ApplicationUser Cleaner { get; set; } = null!;

        public int Rating { get; set; }            // 1..5
        [MaxLength(1000)] public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
