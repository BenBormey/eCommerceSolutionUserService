using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Booking
{
    public class BookingUpdateDTO
    {
        [Required] public Guid BookingId { get; set; }

        public Guid? CustomerId { get; set; } 
        public Guid? CleanerId { get; set; }
        public int? ServiceId { get; set; }
        public DateOnly? BookingDate { get; set; }
        public TimeOnly? TimeSlot { get; set; }
        public int? LocationId { get; set; }
        [MaxLength(500)] public string? AddressDetail { get; set; }
        [MaxLength(500)] public string? Notes { get; set; }
        public string? Status { get; set; }      // "Pending","Confirmed","Completed","Cancelled"

    }
}
