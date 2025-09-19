using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Payment
{
    public class PaymentMarkPaidDTO
    {

        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }   // if null, server will use now (UTC)
        public string? Method { get; set; }
    }
}
