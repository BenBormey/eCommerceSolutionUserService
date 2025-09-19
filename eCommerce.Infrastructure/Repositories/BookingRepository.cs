using Dapper;
using eCommerce.Core.DTO.Booking;
using eCommerce.Core.DTO.CleanerAvailability;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;
using System.Data;

namespace eCommerce.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DapperDbContext _db;
        public BookingRepository(DapperDbContext db) => _db = db;

        // ---------- Helpers ----------
        private static string BaseSelect(string extraWhere = "")
        {
            // LEFT JOIN detail rows; map later with Dapper multi-mapping
            return $@"
SELECT
  b.booking_id     AS ""BookingId"",
  b.customer_id    AS ""CustomerId"",
  b.cleaner_id     AS ""CleanerId"",
  b.booking_date   AS ""BookingDate"",
  b.time_slot      AS ""TimeSlot"",
  b.location_id    AS ""LocationId"",
  b.address_detail AS ""AddressDetail"",
  b.status         AS ""Status"",
  b.notes          AS ""Notes"",
  b.created_at     AS ""CreatedAt"",
  d.booking_detail_id AS ""BookingDetailId"",
  d.service_id        AS ""ServiceId"",
  d.quantity          AS ""Quantity"",
  d.price             AS ""Price"",
  d.subtotal          AS ""Subtotal"",
  d.remark            AS ""Remark""
FROM public.bookings b
LEFT JOIN public.booking_details d ON d.booking_id = b.booking_id
{extraWhere}
ORDER BY b.booking_date DESC, b.time_slot DESC, b.created_at DESC;";
        }

        private static IEnumerable<BookingDTO> MapBookings(IEnumerable<dynamic> rows)
        {
            // not used (we'll use Dapper multi-mapping directly)
            throw new NotImplementedException();
        }

        // ---------- Queries with multi-mapping ----------
        private async Task<IEnumerable<BookingDTO>> QueryBookingsAsync(string sql, object param)
        {
            using var conn = _db.DbConnection; // property returning IDbConnection
            var lookup = new Dictionary<Guid, BookingDTO>();

            // Multi-mapping: Booking row + Detail row → BookingDTO (with Details list)
            var _ = await conn.QueryAsync<BookingDTO, BookingDetailDTO, BookingDTO>(
                sql,
                (b, d) =>
                {
                    if (!lookup.TryGetValue(b.BookingId, out var agg))
                    {
                        agg = b;
                        agg.Details = new List<BookingDetailDTO>();
                        lookup.Add(agg.BookingId, agg);
                    }

                    if (d != null && d.BookingDetailId != Guid.Empty)
                        agg.Details!.Add(d);

                    return agg;
                },
                param,
                splitOn: "BookingDetailId"
            );

            return lookup.Values;
        }

        public async Task<BookingDTO?> GetById(Guid bookingId)
        {
            var sql = BaseSelect("WHERE b.booking_id = @Id");
            var list = await QueryBookingsAsync(sql, new { Id = bookingId });
            return list.FirstOrDefault();
        }

        public async Task<IEnumerable<BookingDTO>> GetByCustomer(Guid customerId, DateTime? from, DateTime? to)
        {
            var sql = BaseSelect(@"
WHERE b.customer_id = @CustomerId
  AND (@From IS NULL OR b.booking_date::date >= @From::date)
  AND (@To   IS NULL OR b.booking_date::date <= @To::date)");
            return await QueryBookingsAsync(sql, new { CustomerId = customerId, From = from, To = to });
        }

        public async Task<IEnumerable<BookingDTO>> GetByCleaner(Guid cleanerId, DateTime? from, DateTime? to)
        {
            var sql = BaseSelect(@"
WHERE b.cleaner_id = @CleanerId
  AND (@From IS NULL OR b.booking_date::date >= @From::date)
  AND (@To   IS NULL OR b.booking_date::date <= @To::date)");
            return await QueryBookingsAsync(sql, new { CleanerId = cleanerId, From = from, To = to });
        }

        // status logic:
        // - Pending: show open (CleanerId is null) OR mine (CleanerId = @CleanerId), both with Status='Pending'
        // - Else: only mine with that status
        //Guid cleanerId,
        public async Task<IEnumerable<BookingDTO>> ListForCleanerAsync( string status, DateTime? from, DateTime? to)
        {
            var isPending = string.Equals(status, "Pending", StringComparison.OrdinalIgnoreCase);
            var where = isPending
                ? @"
WHERE b.status = 'Pending'
  AND (@From IS NULL OR b.booking_date::date >= @From::date)
  AND (@To   IS NULL OR b.booking_date::date <= @To::date)"
                : @"
WHERE b.status = @Status 
  AND (@From IS NULL OR b.booking_date::date >= @From::date)
  AND (@To   IS NULL OR b.booking_date::date <= @To::date)";

            var sql = BaseSelect(where);
            return await QueryBookingsAsync(sql, new {  Status = status, From = from, To = to });
        }

        // ---------- Commands ----------
        public async Task<BookingDTO> Create(BookingCreateDTO dto)
        {
            // Expect dto.Items (multi-service). If legacy single-service arrives, convert to one item.
            if (dto.Items == null || dto.Items.Count == 0)
            {
                throw new ArgumentException("Items must contain at least one service.");
            }

            const string insertBooking = @"
INSERT INTO public.bookings
(booking_id, customer_id, cleaner_id, booking_date, time_slot, location_id, address_detail, status, notes, created_at,amount)
VALUES
(@Id, @CustomerId, @CleanerId, @BookingDate, @TimeSlot, @LocationId, @AddressDetail, 'Pending', @Notes, timezone('utc', now()),@amount);";

            const string insertDetail = @"
INSERT INTO public.booking_details
(booking_detail_id, booking_id, service_id, quantity, price, remark)
VALUES
(@DetailId, @BookingId, @ServiceId, @Quantity, @Price, @Remark);";


            var id = Guid.NewGuid();

            decimal total = 0m;

            // pre-calc total from items
            foreach (var item in dto.Items)
            {
                var price = item.Price ?? 0m;
                var qty = item.Quantity;
                total += price * qty;
            }

            using var conn = _db.DbConnection;
            if (conn.State != ConnectionState.Open) conn.Open();
            using var tx = conn.BeginTransaction();

            try
            {
                // parent
                await conn.ExecuteAsync(insertBooking, new
                {
                    Id = id,
                    dto.CustomerId,
                    dto.CleanerId,
                    dto.BookingDate,
                    dto.TimeSlot,
                    dto.LocationId,
                    dto.AddressDetail,
                    dto.Notes  ,
                    amount = total

                }, tx);

                // details
                foreach (var item in dto.Items)
                {
                    var price = item.Price ?? 0m; // or lookup from services table
                    var subtotal = price * item.Quantity;

                    await conn.ExecuteAsync(insertDetail, new
                    {
                        DetailId = Guid.NewGuid(),
                        BookingId = id,
                        ServiceId = item.ServiceId,   // int
                        Quantity = item.Quantity,
                        Price = item.Price ?? 0m,
                        Remark = item.Remark
                    }, tx);

                }

                tx.Commit();

                // Return full object
                var created = await GetById(id);
                return created!;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public async Task<BookingDTO?> Update(Guid bookingId, BookingUpdateDTO dto)
        {
            // Update only parent fields here. If you need detail updates, handle separately (delete+reinsert or diff).
            const string sql = @"
UPDATE public.bookings
SET
  customer_id    = COALESCE(@CustomerId,    customer_id),
  cleaner_id     = COALESCE(@CleanerId,     cleaner_id),
  booking_date   = COALESCE(@BookingDate,   booking_date),
  time_slot      = COALESCE(@TimeSlot,      time_slot),
  location_id    = COALESCE(@LocationId,    location_id),
  address_detail = COALESCE(@AddressDetail, address_detail),
  notes          = COALESCE(@Notes,         notes),
  status         = COALESCE(@Status,        status)
WHERE booking_id = @Id;";

            using var conn = _db.DbConnection;
            var rows = await conn.ExecuteAsync(sql, new
            {
                Id = bookingId,
                dto.CustomerId,
                dto.CleanerId,
                dto.BookingDate,
                dto.TimeSlot,
                dto.LocationId,
                dto.AddressDetail,
                dto.Notes,
                dto.Status
            });

            if (rows == 0) return null;
            return await GetById(bookingId);
        }

        public async Task<bool> Delete(Guid bookingId)
        {
            // ON DELETE CASCADE on booking_details recommended.
            const string sql = @"DELETE FROM public.bookings WHERE booking_id = @Id;";
            using var conn = _db.DbConnection;
            var rows = await conn.ExecuteAsync(sql, new { Id = bookingId });
            return rows > 0;
        }

        public async Task<bool> ChangeStatus(Guid bookingId, string status, Guid cleanerId)
        {
            const string sql = @"
UPDATE public.bookings
SET status = @Status,
    cleaner_id = @CleanerId
WHERE booking_id = @Id;";
            using var conn = _db.DbConnection;
            var rows = await conn.ExecuteAsync(sql, new { Id = bookingId, Status = status, CleanerId = cleanerId });
            return rows > 0;
        }

        public async Task<bool> ExistsAsync(Guid bookingId)
        {
            const string sql = @"SELECT EXISTS(SELECT 1 FROM public.bookings WHERE booking_id = @Id);";
            using var conn = _db.DbConnection;
            return await conn.ExecuteScalarAsync<bool>(sql, new { Id = bookingId });
        }

        public async Task<int> CountBooking()
        {
            const string sql = @"SELECT count(*)
FROM public.bookings b
INNER JOIN public.booking_details d 
    ON d.booking_id = b.booking_id
WHERE DATE(b.created_at) = CURRENT_DATE;

";

            return await _db.DbConnection.QueryFirstOrDefaultAsync<int>(sql);

        }
    }
}
