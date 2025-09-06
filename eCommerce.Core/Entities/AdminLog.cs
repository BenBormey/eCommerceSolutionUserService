using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Entities
{
    public class AdminLog
    {
        [Key] public Guid LogId { get; set; }

        // FK → Users (Admin)
        public Guid AdminId { get; set; }
        public ApplicationUser Admin { get; set; } = null!;

        [MaxLength(120)] public string Action { get; set; } = null!;
        [MaxLength(1000)] public string? Description { get; set; }
        public DateTime LogDate { get; set; } = DateTime.UtcNow;
    }
}
