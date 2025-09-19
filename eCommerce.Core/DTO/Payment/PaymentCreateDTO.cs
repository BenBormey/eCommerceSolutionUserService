using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Payment
{
    public class PaymentCreateDTO
    {
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = "Cash";
        public string PaymentStatus { get; set; } = "Pending";
        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; } // opt                          // optional at create

    }
}
