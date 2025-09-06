using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Booking
{
    public class BookingCreateDTO
    {
        [Required] public Guid CustomerId { get; set; }
        [Required] public Guid CleanerId { get; set; }
        [Required] public int ServiceId { get; set; }
        [Required] public DateOnly BookingDate { get; set; }
        [Required] public TimeOnly TimeSlot { get; set; }
        public int? LocationId { get; set; }
        [MaxLength(500)] public string? AddressDetail { get; set; }
        [MaxLength(500)] public string? Notes { get; set; }
        // Status defaults to "Pending" on create
    }
}
