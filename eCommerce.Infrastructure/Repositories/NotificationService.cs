using Dapper;
using eCommerce.Core.DTO.Notification;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DapperDbContext _db;
        public NotificationRepository(DapperDbContext db) => _db = db;

        public async Task<IEnumerable<NotificationDTO>> GetByUser(Guid userId, int? limit = null, int? offset = null)
        {
            var sql = @"
                SELECT
                    notification_id AS ""NotificationId"",
                    user_id         AS ""UserId"",
                    message         AS ""Message"",
                    is_read         AS ""IsRead"",
                    sent_at         AS ""SentAt""
                FROM public.notifications
                WHERE user_id = @UserId
                ORDER BY sent_at DESC";
            if (limit.HasValue) sql += " LIMIT @Limit";
            if (offset.HasValue) sql += " OFFSET @Offset";

            return await _db.DbConnection.QueryAsync<NotificationDTO>(sql, new { UserId = userId, Limit = limit, Offset = offset });
        }

        public async Task<NotificationDTO?> GetById(Guid notificationId)
        {
            const string sql = @"
                SELECT
                    notification_id AS ""NotificationId"",
                    user_id         AS ""UserId"",
                    message         AS ""Message"",
                    is_read         AS ""IsRead"",
                    sent_at         AS ""SentAt""
                FROM public.notifications
                WHERE notification_id = @Id;";
            return await _db.DbConnection.QueryFirstOrDefaultAsync<NotificationDTO>(sql, new { Id = notificationId });
        }

        public async Task<NotificationDTO> Create(NotificationCreateDTO dto)
        {
            const string sql = @"
                INSERT INTO public.notifications (notification_id, user_id, message, is_read, sent_at)
                VALUES (@Id, @UserId, @Message, @IsRead, NOW())
                RETURNING
                    notification_id AS ""NotificationId"",
                    user_id         AS ""UserId"",
                    message         AS ""Message"",
                    is_read         AS ""IsRead"",
                    sent_at         AS ""SentAt"";";

            var p = new { Id = Guid.NewGuid(), dto.UserId, dto.Message, dto.IsRead };
            return await _db.DbConnection.QuerySingleAsync<NotificationDTO>(sql, p);
        }

        public async Task<NotificationDTO?> Update(Guid notificationId, NotificationUpdateDTO dto)
        {
            const string sql = @"
                UPDATE public.notifications
                SET
                    message = COALESCE(@Message, message),
                    is_read = COALESCE(@IsRead, is_read)
                WHERE notification_id = @Id
                RETURNING
                    notification_id AS ""NotificationId"",
                    user_id         AS ""UserId"",
                    message         AS ""Message"",
                    is_read         AS ""IsRead"",
                    sent_at         AS ""SentAt"";";

            return await _db.DbConnection.QueryFirstOrDefaultAsync<NotificationDTO>(sql,
                new { Id = notificationId, dto.Message, dto.IsRead });
        }

        public async Task<bool> MarkRead(Guid notificationId, bool isRead = true)
        {
            const string sql = @"UPDATE public.notifications SET is_read = @IsRead WHERE notification_id = @Id;";
            var rows = await _db.DbConnection.ExecuteAsync(sql, new { Id = notificationId, IsRead = isRead });
            return rows > 0;
        }

        public async Task<int> MarkAllRead(Guid userId)
        {
            const string sql = @"UPDATE public.notifications SET is_read = TRUE WHERE user_id = @UserId AND is_read = FALSE;";
            return await _db.DbConnection.ExecuteAsync(sql, new { UserId = userId });
        }

        public async Task<int> GetUnreadCount(Guid userId)
        {
            const string sql = @"SELECT COUNT(*) FROM public.notifications WHERE user_id = @UserId AND is_read = FALSE;";
            return await _db.DbConnection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
        }

        public async Task<bool> Delete(Guid notificationId)
        {
            const string sql = @"DELETE FROM public.notifications WHERE notification_id = @Id;";
            var rows = await _db.DbConnection.ExecuteAsync(sql, new { Id = notificationId });
            return rows > 0;
        }
    }
}
