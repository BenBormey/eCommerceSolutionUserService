using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Entities
{
    public class BookingDetail
    {
        [Key] public Guid BookingDetailId { get; set; }

        // Parent
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        // Service FK → keep type = int if your Services.ServiceId is int
        public int ServiceId { get; set; }
        public Services Service { get; set; } = null!;

        // Cleaner per line (optional)
        public Guid? CleanerId { get; set; }
        public ApplicationUser? Cleaner { get; set; }

        public int Quantity { get; set; } = 1;

        [Column(TypeName = "numeric(10,2)")]
        public decimal Price { get; set; }

        // Stored computed subtotal (optional) or compute in query
        [Column(TypeName = "numeric(10,2)")]
        public decimal Subtotal { get; set; }

        [MaxLength(300)]
        public string? Remark { get; set; }
    }
}
