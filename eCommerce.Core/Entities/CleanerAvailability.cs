using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Entities
{
    public class CleanerAvailability
    {
        [Key] public int AvailabilityId { get; set; }

        // FK → Users (Cleaner)
        public Guid CleanerId { get; set; }
        public ApplicationUser Cleaner { get; set; } = null!;

        public DateOnly AvailableDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
