using Dapper;
using eCommerce.Core.DTO.NewFolder;
using eCommerce.Core.DTO.Payment;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DapperDbContext _db;
        public PaymentRepository(DapperDbContext db) => _db = db;

        public async Task<PaymentDTO?> GetById(Guid paymentId)
        {
            const string sql = @"
                SELECT
                    payment_id    AS ""PaymentId"",
                    booking_id    AS ""BookingId"",
                    amount        AS ""Amount"",
                    method        AS ""Method"",
                    transaction_id AS ""TransactionId"",
                    paid_at       AS ""PaidAt"",
                    created_at    AS ""CreatedAt""
                FROM public.payments
                WHERE payment_id = @Id;";
            return await _db.DbConnection.QueryFirstOrDefaultAsync<PaymentDTO>(sql, new { Id = paymentId });
        }

        public async Task<IEnumerable<PaymentDTO>> GetByBooking(Guid bookingId)
        {
            const string sql = @"
                SELECT
                    payment_id    AS ""PaymentId"",
                    booking_id    AS ""BookingId"",
                    amount        AS ""Amount"",
                    method        AS ""Method"",
                    transaction_id AS ""TransactionId"",
                    paid_at       AS ""PaidAt"",
                    created_at    AS ""CreatedAt""
                FROM public.payments
                WHERE booking_id = @BookingId
                ORDER BY created_at DESC;";
            return await _db.DbConnection.QueryAsync<PaymentDTO>(sql, new { BookingId = bookingId });
        }

        public async Task<PaymentDTO> Create(PaymentCreateDTO dto)
        {
            const string sql = @"
                INSERT INTO public.payments
                    (payment_id, booking_id, amount, method, transaction_id, paid_at, created_at)
                VALUES
                    (@Id, @BookingId, @Amount, @Method, @TransactionId, @PaidAt, NOW())
                RETURNING
                    payment_id    AS ""PaymentId"",
                    booking_id    AS ""BookingId"",
                    amount        AS ""Amount"",
                    method        AS ""Method"",
                    transaction_id AS ""TransactionId"",
                    paid_at       AS ""PaidAt"",
                    created_at    AS ""CreatedAt"";";
            var p = new
            {
                Id = Guid.NewGuid(),
                dto.BookingId,
                dto.Amount,
                dto.Method,
                dto.TransactionId,
                dto.PaidAt
            };
            return await _db.DbConnection.QuerySingleAsync<PaymentDTO>(sql, p);
        }

        public async Task<PaymentDTO?> Update(Guid paymentId, PaymentUpdateDTO dto)
        {
            const string sql = @"
                UPDATE public.payments
                SET
                    amount         = COALESCE(@Amount, amount),
                    method         = COALESCE(@Method, method),
                    transaction_id = COALESCE(@TransactionId, transaction_id),
                    paid_at        = @PaidAt
                WHERE payment_id = @Id
                RETURNING
                    payment_id    AS ""PaymentId"",
                    booking_id    AS ""BookingId"",
                    amount        AS ""Amount"",
                    method        AS ""Method"",
                    transaction_id AS ""TransactionId"",
                    paid_at       AS ""PaidAt"",
                    created_at    AS ""CreatedAt"";";
            return await _db.DbConnection.QueryFirstOrDefaultAsync<PaymentDTO>(sql, new
            {
                Id = paymentId,
                dto.Amount,
                dto.Method,
                dto.TransactionId,
                dto.PaidAt
            });
        }

        public async Task<bool> Delete(Guid paymentId)
        {
            const string sql = @"DELETE FROM public.payments WHERE payment_id = @Id;";
            var rows = await _db.DbConnection.ExecuteAsync(sql, new { Id = paymentId });
            return rows > 0;
        }

        public async Task<bool> MarkPaid(Guid paymentId, DateTime? paidAt, string? transactionId)
        {
            const string sql = @"
                UPDATE public.payments
                SET paid_at = COALESCE(@PaidAt, NOW()),
                    transaction_id = COALESCE(@TransactionId, transaction_id)
                WHERE payment_id = @Id;";
            var rows = await _db.DbConnection.ExecuteAsync(sql, new { Id = paymentId, PaidAt = paidAt, TransactionId = transactionId });
            return rows > 0;
        }

        public async Task<bool> Exists(Guid paymentId)
        {
            const string sql = @"SELECT EXISTS(SELECT 1 FROM public.payments WHERE payment_id = @Id);";
            return await _db.DbConnection.ExecuteScalarAsync<bool>(sql, new { Id = paymentId });
        }
    }
}
