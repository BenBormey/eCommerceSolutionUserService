using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Entities
{
    public class Booking
    {
        [Key] public Guid BookingId { get; set; }

        // FKs
        public Guid CustomerId { get; set; }
        public ApplicationUser Customer { get; set; } = null!;

        // Cleaner អាចនៅតែមិនបាន assign ពេលបង្កើត => nullable
        public Guid? CleanerId { get; set; }
        public ApplicationUser? Cleaner { get; set; }

        // REMOVE ServiceId/Service from parent (ត្រូវស្រាប់លើ details)
        // public int ServiceId { get; set; }
        // public Services Service { get; set; } = null!;

        // Date/Time
        [Column(TypeName = "date")]
        public DateOnly BookingDate { get; set; }

        // Use TimeOnly → maps to PostgreSQL 'time'
        [Column(TypeName = "time")]
        public TimeOnly TimeSlot { get; set; }

        public int? LocationId { get; set; }
        public Location? Location { get; set; }

        [MaxLength(500)]
        public string? AddressDetail { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Completed, Cancelled

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigations
        public ICollection<BookingDetail> Details { get; set; } = new List<BookingDetail>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public Invoice? Invoice { get; set; }

    }
}
