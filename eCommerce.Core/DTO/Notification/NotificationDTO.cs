using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Notification
{
    public class NotificationDTO
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
    }
}
