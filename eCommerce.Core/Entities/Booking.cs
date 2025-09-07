using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace eCommerce.Core.Entities
{
    public class Booking
    {
        [Key] public Guid BookingId { get; set; }

        // FKs
        public Guid CustomerId { get; set; }
        public ApplicationUser Customer { get; set; } = null!;

        public Guid CleanerId { get; set; }
        public ApplicationUser Cleaner { get; set; } = null!;

        public int ServiceId { get; set; }
        public Services Service { get; set; } = null!;

        public DateTime BookingDate { get; set; }
         public TimeOnly TimeSlot { get; set; }

        public int? LocationId { get; set; }
        public Location? Location { get; set; }

        [MaxLength(500)] public string? AddressDetail { get; set; }

        public string status { get; set; } = "Pending"; // Pending, Confirmed, Completed, Cancelled

        [MaxLength(500)] public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigations
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public Invoice? Invoice { get; set; }

    }
}
