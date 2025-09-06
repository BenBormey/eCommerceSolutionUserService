using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Notification
{
    public class NotificationUpdateDTO
    {
        [Required]
        public Guid NotificationId { get; set; }

        [MaxLength(500)]
        public string? Message { get; set; }

        public bool? IsRead { get; set; }  // nullable so you can update one field without the other

    }
}
