using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Service
{
    public class ServiceUpdateDTO
    {
        [Required]
        public int ServiceId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(0, 999999.99)]
        public decimal Price { get; set; }

        public Guid CategoryId { get; set; }
        public int DurationMinutes { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
