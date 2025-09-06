using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Review
{
    public class ReviewUpdateDTO
    {
        [Required] public int ReviewId { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }          // nullable → partial update

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }
}
