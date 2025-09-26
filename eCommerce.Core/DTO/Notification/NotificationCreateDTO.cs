using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Notification
{
    public class NotificationCreateDTO
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; } = false;
    }
}
