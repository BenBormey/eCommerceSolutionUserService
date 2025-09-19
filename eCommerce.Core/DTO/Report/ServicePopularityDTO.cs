using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public class ServicePopularityDTO
    {
        public int ServiceId { get; set; }          // ID របស់សេវា
        public string ServiceName { get; set; }     // ឈ្មោះសេវា
        public int TotalBookings { get; set; }      // ចំនួន booking ដែលមានសេវានេះ
        public int TotalQuantity { get; set; }      // ចំនួនសរុបដែលបានជ្រើសរើស
        public decimal TotalRevenue { get; set; }   // ចំណូលសរុប (price * quantity)

    }
}
