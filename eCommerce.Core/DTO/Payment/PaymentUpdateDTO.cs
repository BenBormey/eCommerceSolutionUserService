using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Payment
{
    public class PaymentUpdateDTO
    {
        [Required] public Guid PaymentId { get; set; }
        public decimal? Amount { get; set; }
        [MaxLength(50)] public string? Method { get; set; }
        [MaxLength(120)] public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }   // set/clear paid time

    }
}
