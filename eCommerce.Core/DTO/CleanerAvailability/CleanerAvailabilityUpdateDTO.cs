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
        [Required] public DateOnly AvailableDate { get; set; }
        [Required] public TimeOnly StartTime { get; set; }
        [Required] public TimeOnly EndTime { get; set; }
    }
}
