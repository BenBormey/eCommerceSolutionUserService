using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Booking
{
    public class BookingDTO
    {
        public Guid BookingId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? CleanerId { get; set; }
        public string? CleanerName { get; set; }
        public string? CustomerName { get; set; }

        public DateTime BookingDate { get; set; }
        public TimeSpan TimeSlot { get; set; }   // maps to PostgreSQL "time"

        public int? LocationId { get; set; }
        public string? AddressDetail { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public double Amount { get; set; }
        public Guid PayMentId { get; set; }

        // 👉 List of details (instead of single ServiceId)
        public List<BookingDetailDTO> Details { get; set; }
    }
}
