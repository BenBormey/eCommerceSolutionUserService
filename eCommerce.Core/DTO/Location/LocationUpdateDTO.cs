using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Location
{
    public class LocationUpdateDTO
    {
        [Required]
        public int LocationId { get; set; }

        [MaxLength(120)]
        public string? City { get; set; }

        [MaxLength(120)]
        public string? District { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }
    }
}
