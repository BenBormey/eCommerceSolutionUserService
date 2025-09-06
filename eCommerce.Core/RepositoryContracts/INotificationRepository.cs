using eCommerce.Core.DTO.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.RepositoryContracts
{
    public interface INotificationRepository
    {
        Task<IEnumerable<NotificationDTO>> GetByUser(Guid userId, int? limit = null, int? offset = null);
        Task<NotificationDTO?> GetById(Guid notificationId);

        // Create
        Task<NotificationDTO> Create(NotificationCreateDTO dto);

        // Update (message / isRead)
        Task<NotificationDTO?> Update(Guid notificationId, NotificationUpdateDTO dto);

        // Mark read/unread
        Task<bool> MarkRead(Guid notificationId, bool isRead = true);
        Task<int> MarkAllRead(Guid userId);           // returns affected rows
        Task<int> GetUnreadCount(Guid userId);

        // Delete
        Task<bool> Delete(Guid notificationId);
    }
}
