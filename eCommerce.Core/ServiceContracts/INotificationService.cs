using eCommerce.Core.DTO.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.ServiceContracts
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDTO>> GetByUserAsync(Guid userId, int? limit = null, int? offset = null);
        Task<NotificationDTO?> GetByIdAsync(Guid id);
        Task<NotificationDTO> CreateAsync(NotificationCreateDTO dto);
        Task<NotificationDTO?> UpdateAsync(Guid id, NotificationUpdateDTO dto);
        Task<bool> MarkReadAsync(Guid id, bool isRead = true);
        Task<int> MarkAllReadAsync(Guid userId);
        Task<int> GetUnreadCountAsync(Guid userId);
        Task<bool> DeleteAsync(Guid id);
    }
}
