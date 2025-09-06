using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.NewFolder
{
   public class PaymentDTO
    {
        public Guid PaymentId { get; set; }
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = null!;
        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
