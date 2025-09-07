using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.CleanerAvailability
{
    public class CleanerAvailabilityUpdateDTO
    {
        [Required] public int AvailabilityId { get; set; }
        [Required] public Guid CleanerId { get; set; }
        [Required] public DateTime AvailableDate { get; set; }
        [Required] public TimeSpan StartTime { get; set; }
        [Required] public TimeSpan EndTime { get; set; }
    }
}
