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
        public Guid? CleanerId { get; set; }

        // Consider DateOnly/TimeOnly with Npgsql, but DateTime/TimeSpan works if your entity uses them.
        public DateTime BookingDate { get; set; }
        public TimeSpan TimeSlot { get; set; }

        public int? LocationId { get; set; }
        public string? AddressDetail { get; set; }
        public string? Notes { get; set; }

        // NEW: multiple services
        [Required, MinLength(1)]
        public List<BookingItemDTO> Items { get; set; } = new();
    }
}
