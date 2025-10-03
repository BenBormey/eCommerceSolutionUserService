using Dapper;
using eCommerce.Core.DTO.NewFolder;
using eCommerce.Core.DTO.Payment;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;

namespace eCommerce.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DapperDbContext _db;
        public PaymentRepository(DapperDbContext db) => _db = db;

        private const string SelectBase = @"
SELECT
  payment_id      AS ""PaymentId"",
  booking_id      AS ""BookingId"",
  amount          AS ""Amount"",
  method          AS ""Method"",
  payment_status  AS ""PaymentStatus"",
  transaction_id  AS ""TransactionId"",
  paid_at         AS ""PaidAt"",
  created_at      AS ""CreatedAt""
FROM public.payments
";

        public async Task<PaymentDTO?> GetById(Guid paymentId)
        {
            var sql = SelectBase + " WHERE payment_id = @Id LIMIT 1;";
            return await _db.DbConnection.QuerySingleOrDefaultAsync<PaymentDTO>(sql, new { Id = paymentId });
        }

        public async Task<IEnumerable<PaymentDTO>> GetByBooking(Guid bookingId)
        {
            var sql = SelectBase + @"
WHERE booking_id = @BookingId
ORDER BY created_at DESC;";
            return await _db.DbConnection.QueryAsync<PaymentDTO>(sql, new { BookingId = bookingId });
        }

        public async Task<PaymentDTO> Create(PaymentCreateDTO dto)
        {
            const string sql = @"
INSERT INTO public.payments
(payment_id, booking_id, amount, method, payment_status, transaction_id, paid_at, created_at)
VALUES (@Id, @BookingId, @Amount, @Method, @PaymentStatus, @TransactionId, @PaidAt,
        timezone('utc', now()))
RETURNING
  payment_id      AS ""PaymentId"",
  booking_id      AS ""BookingId"",
  amount          AS ""Amount"",
  method          AS ""Method"",
  payment_status  AS ""PaymentStatus"",
  transaction_id  AS ""TransactionId"",
  paid_at         AS ""PaidAt"",
  created_at      AS ""CreatedAt"";";

            var p = new
            {
                Id = Guid.NewGuid(),
                dto.BookingId,
                dto.Amount,
                dto.Method,
                dto.PaymentStatus,         // default "Pending" if not provided
                dto.TransactionId,
                dto.PaidAt                 // can be null
            };

            return await _db.DbConnection.QuerySingleAsync<PaymentDTO>(sql, p);
        }

        public async Task<PaymentDTO?> Update(Guid paymentId, PaymentUpdateDTO dto)
        {
            const string sql = @"
UPDATE public.payments
SET amount          = COALESCE(@Amount, amount) +amount ,
    method          = COALESCE(@Method, method) ,
    payment_status  = COALESCE(@PaymentStatus, payment_status),
    transaction_id  = COALESCE(@TransactionId, transaction_id)
WHERE booking_id = @Id
RETURNING
  payment_id      AS ""PaymentId"",
  booking_id      AS ""BookingId"",
  amount          AS ""Amount"",
  method          AS ""Method"",
  payment_status  AS ""PaymentStatus"",
  transaction_id  AS ""TransactionId"",
  paid_at         AS ""PaidAt"",
  created_at      AS ""CreatedAt"";";

            return await _db.DbConnection.QuerySingleOrDefaultAsync<PaymentDTO>(sql, new
            {
                Id = paymentId,
                dto.Amount,
                dto.Method,
                dto.PaymentStatus,
                dto.TransactionId
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
SET paid_at        = COALESCE(@PaidAt, timezone('utc', now())),
    payment_status = 'Paid',
    transaction_id = COALESCE(@TransactionId, transaction_id)
WHERE payment_id = @Id;";
            var rows = await _db.DbConnection.ExecuteAsync(sql, new
            {
                Id = paymentId,
                PaidAt = paidAt,
                TransactionId = transactionId
            });
            return rows > 0;
        }

        public async Task<bool> Exists(Guid paymentId)
        {
            const string sql = @"SELECT EXISTS(SELECT 1 FROM public.payments WHERE payment_id = @Id);";
            return await _db.DbConnection.ExecuteScalarAsync<bool>(sql, new { Id = paymentId });
        }
    }
}
