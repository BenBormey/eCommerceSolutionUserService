using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Booking
{
    public class BookingItemDTO
    {
        public int ServiceId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal? Price { get; set; } // optional: backend can look up current price
        public string? Remark { get; set; }
    }
}
