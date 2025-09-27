using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Booking
{
    public class BookingDetailDTO
    {
        public Guid BookingDetailId { get; set; }
        public string ServiceName { get; set; }
        public int ServiceId { get; set; }      // keep int to match Services.ServiceId
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
        public string? Remark { get; set; }
    }
}
