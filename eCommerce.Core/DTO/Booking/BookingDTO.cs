using System;
using System.Collections.Generic;

namespace eCommerce.Core.DTO.Booking
{
    // This DTO holds all necessary data for a booking record, 
    // often used for Read (GET) operations like My Bookings History.
    public class BookingDTO
    {
        // Core Identifiers
        public Guid BookingId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? CleanerId { get; set; }

        // User Information
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CleanerName { get; set; }
        public string? CleanerPhone { get; set; }

        // Schedule and Location
        public DateTime BookingDate { get; set; }
        public TimeSpan TimeSlot { get; set; }
        public int? LocationId { get; set; }
        public string? AddressDetail { get; set; }

        // Status and Metadata
        public string Status { get; set; } = "Pending";
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // Recurrence Fields
        public bool? IsRecurring { get; set; }
        public string? RecurrencePlan { get; set; }
        public DateTime? EndDate { get; set; }

        // Financial Fields
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TotalAmountFinal { get; set; } // The total final cost of the booking

        // ✅ Payment Properties (Mapped from LATERAL JOIN)
        public decimal? AmountPaid { get; set; } // The actual amount successfully paid
        public Guid? PaymentId { get; set; }
        public string? PaymentStatus { get; set; } // e.g., 'Paid', 'Pending', 'Failed', 'Unpaid'

        // Details (Nested list of services/items)
        public List<BookingDetailDTO> Details { get; set; } = new();
    }
}