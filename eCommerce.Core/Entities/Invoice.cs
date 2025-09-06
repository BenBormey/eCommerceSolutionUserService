using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace eCommerce.Core.Entities
{
   public class Invoice
    {
        [Key] public Guid InvoiceId { get; set; }

        // FK → Booking
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        [MaxLength(50)] public string InvoiceNumber { get; set; } = null!;
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        [MaxLength(500)] public string? PdfUrl { get; set; }

    }
}
