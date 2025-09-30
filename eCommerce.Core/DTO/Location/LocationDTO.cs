using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Location
{
    public class LocationDTO
    {
        public int LocationId { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? PostalCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public  Guid user_id { get; set; }
        public string full_name { get; set; }
    }
}
