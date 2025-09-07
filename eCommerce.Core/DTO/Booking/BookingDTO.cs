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
        public Guid CustomerId { get; set; }
        public Guid CleanerId { get; set; }
        public int ServiceId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan TimeSlot { get; set; }              // DB: time without time zone
        public int? LocationId { get; set; }
        public string? AddressDetail { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
