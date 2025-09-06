using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Review
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }
        public Guid BookingId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CleanerId { get; set; }
        public int Rating { get; set; }          // 1..5
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
