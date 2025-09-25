using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Customer
{
    public class CustomerDTO
    {
        public Guid UserId { get; set; }


        public string FullName { get; set; } = string.Empty;


        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }


        public string Role { get; set; } = "Customer";  

  
        public string? ProfileImage { get; set; }


        public string Status { get; set; }
    }
}
