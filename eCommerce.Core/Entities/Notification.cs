using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace eCommerce.Core.Entities
{
    public class Notification
    {
        [Key] public Guid NotificationId { get; set; }

        // FK → User
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        [MaxLength(500)] public string Message { get; set; } = null!;
        public bool IsRead { get; set; } = false;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
