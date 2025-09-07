using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.CleanerAvailability
{
    public class CleanerAvailabilityDTO
    {
        public int AvailabilityId { get; set; }
        public Guid CleanerId { get; set; }
        public DateTime AvailableDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
