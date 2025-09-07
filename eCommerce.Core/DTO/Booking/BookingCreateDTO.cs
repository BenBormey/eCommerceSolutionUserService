using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Booking
{
    public class BookingCreateDTO
    {
        public Guid CustomerId { get; set; }
        public Guid CleanerId { get; set; }
        public int ServiceId { get; set; }

        public DateTime BookingDate { get; set; }   // ✅
        public TimeSpan TimeSlot { get; set; }

        public int? LocationId { get; set; }
        public string? AddressDetail { get; set; }
        public string? Notes { get; set; }
    }
}
