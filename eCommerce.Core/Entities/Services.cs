using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Entities
{
    public class Services
    {
        [Key] public int ServiceId { get; set; }

        [MaxLength(120)] public string Name { get; set; } = null!;
        [MaxLength(500)] public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        [MaxLength(500)] public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
