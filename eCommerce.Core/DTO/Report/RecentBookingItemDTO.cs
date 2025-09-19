using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public class RecentBookingItemDTO
    {
        public Guid Id { get; set; }          // booking_id
        public string? CustomerName { get; set; }
        public string? Category { get; set; }          // e.g., "Cleaning"
        public decimal Price { get; set; }          // numeric(12,2)
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
