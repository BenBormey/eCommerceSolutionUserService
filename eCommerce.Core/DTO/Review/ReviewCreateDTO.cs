using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Review
{
    public class ReviewCreateDTO
    {
        [Required] public Guid BookingId { get; set; }
        [Required] public Guid CustomerId { get; set; }
        [Required] public Guid CleanerId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }
}
