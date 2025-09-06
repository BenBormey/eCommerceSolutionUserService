using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Entities
{
    public class Payment
    {
        [Key] public Guid PaymentId { get; set; }

        // FK
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public decimal Amount { get; set; }
        public string Method { get; set; } 

       

        [MaxLength(120)] public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
