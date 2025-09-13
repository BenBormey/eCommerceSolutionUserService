using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO
{
    public class AssignRoleRequest
    {
        [Required]
        public Guid UserId { get; set; }

        // If you store roles in a Roles table, you can use RoleId.
        // If not, you can ignore this and use RoleName only.
        public Guid? RoleId { get; set; }

        [MaxLength(30)]
        public string? RoleName { get; set; }
    }
}
