using eCommerce.Core.DTO.Notification;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        public NotificationService(INotificationRepository repo) => _repo = repo;

        public Task<IEnumerable<NotificationDTO>> GetByUserAsync(Guid userId, int? limit = null, int? offset = null)
            => _repo.GetByUser(userId, limit, offset);

        public Task<NotificationDTO?> GetByIdAsync(Guid id)
            => _repo.GetById(id);

        public Task<NotificationDTO> CreateAsync(NotificationCreateDTO dto)
            => _repo.Create(dto);

        public Task<NotificationDTO?> UpdateAsync(Guid id, NotificationUpdateDTO dto)
            => _repo.Update(id, dto);

        public Task<bool> MarkReadAsync(Guid id, bool isRead = true)
            => _repo.MarkRead(id, isRead);

        public Task<int> MarkAllReadAsync(Guid userId)
            => _repo.MarkAllRead(userId);

        public Task<int> GetUnreadCountAsync(Guid userId)
            => _repo.GetUnreadCount(userId);

        public Task<bool> DeleteAsync(Guid id)
            => _repo.Delete(id);
    }
}
