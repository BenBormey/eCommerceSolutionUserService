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
        [Required] public Guid BookingId { get; set; }
        [Required, Range(0.01, double.MaxValue)] public decimal Amount { get; set; }

        [Required, MaxLength(50)] public string Method { get; set; } = null!; // e.g., "cash","card","wallet"
        [MaxLength(120)] public string? TransactionId { get; set; }           // optional at create
        public DateTime? PaidAt { get; set; }                                  // optional at create

    }
}
