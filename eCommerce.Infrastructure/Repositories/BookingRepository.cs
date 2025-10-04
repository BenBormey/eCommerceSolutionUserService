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
    b.booking_id            AS ""BookingId"",
    u.user_id               AS ""CustomerId"",
    u.full_name             AS ""CustomerName"",
    u.phone                 AS ""CustomerPhone"",
    u1.user_id              AS ""CleanerId"",
    u1.full_name            AS ""CleanerName"",
    u1.phone                AS ""CleanerPhone"",
    b.booking_date          AS ""BookingDate"",
    b.time_slot             AS ""TimeSlot"",
    b.status                AS ""Status"",
    b.notes                 AS ""Notes"",
    b.address_detail        AS ""AddressDetail"",
    b.created_at            AS ""CreatedAt"",

    -- Recurring & Discount Fields
    b.is_recurring          AS ""IsRecurring"",
    b.recurrence_plan       AS ""RecurrencePlan"",
    b.end_date              AS ""EndDate"",
    b.discount_percentage   AS ""DiscountPercentage"",
    b.discount_amount       AS ""DiscountAmount"",
    b.amount                AS ""TotalAmountFinal"", 

    -- ✅ Payment Info (UPDATED WITH COALESCE - This is essential)
    p_latest.amount         AS ""AmountPaid"", 
    p_latest.payment_id     AS ""PaymentId"",
    COALESCE(p_latest.payment_status, 'Unpaid') AS ""PaymentStatus"", -- Sets 'Unpaid' if no payment record found

    -- Booking Details (Split starts here)
    d.booking_detail_id     AS ""BookingDetailId"", 
    d.service_id            AS ""ServiceId"",
    s.name                  AS ""ServiceName"",
    d.quantity              AS ""Quantity"",
    d.price                 AS ""Price"",
    d.subtotal              AS ""Subtotal"",
    d.remark                AS ""Remark""
FROM public.bookings b
LEFT JOIN public.users u ON u.user_id = b.customer_id
LEFT JOIN public.users u1 ON u1.user_id = b.cleaner_id
LEFT JOIN public.booking_details d ON d.booking_id = b.booking_id
LEFT JOIN public.services s ON s.service_id = d.service_id 

-- LATERAL JOIN to find the latest payment record (if any)
LEFT JOIN LATERAL (
    SELECT amount, payment_id, payment_status
    FROM public.payments p_inner
    WHERE p_inner.booking_id = b.booking_id
    ORDER BY p_inner.created_at DESC 
    LIMIT 1
) AS p_latest ON TRUE
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
            using var conn = _db.DbConnection; // IDbConnection

            var lookup = new Dictionary<Guid, BookingDTO>();

            var list = await conn.QueryAsync<BookingDTO, BookingDetailDTO, BookingDTO>(
                sql,
                (booking, detail) =>
                {
          
                    if (!lookup.TryGetValue(booking.BookingId, out var agg))
                    {
                        agg = booking;
                        agg.Details = new List<BookingDetailDTO>();
                        lookup.Add(agg.BookingId, agg);
                    }

                    // Add detail if not null
                    if (detail != null && detail.BookingDetailId != Guid.Empty)
                        agg.Details.Add(detail);

                    return agg;
                },
                param,
                splitOn: "BookingDetailId" // must match the alias of BookingDetailDTO columns
            );

            return lookup.Values;
        }


        public async Task<BookingDTO?> GetById(Guid bookingId)
        {
            var sql = BaseSelect("WHERE b.booking_id = @Id or b.customer_id = @Id");
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
        public async Task<IEnumerable<BookingDTO>> ListForCleanerAsync( string status, DateTime? from, DateTime? to,Guid? cleaid)
        {
            var isPending = string.Equals(status, "Pending", StringComparison.OrdinalIgnoreCase);
            var where = isPending
                ? @"
WHERE b.status = 'Pending'
  AND b.booking_date::date >= COALESCE(@From::date, b.booking_date::date)
  AND b.booking_date::date <= COALESCE(@To::date,   b.booking_date::date)
"
                : $@"
WHERE b.status = @Status 
AND b.booking_date::date >= COALESCE(@From::date, b.booking_date::date)
  AND b.booking_date::date <= COALESCE(@To::date,   b.booking_date::date)
AND 
   b.cleaner_id = '{cleaid}'

";

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

            // 1. UPDATED SQL: Includes all new recurring and discount columns
            const string insertBooking = @"
INSERT INTO public.bookings
(booking_id, customer_id, cleaner_id, booking_date, time_slot, location_id, address_detail, status, notes, created_at, amount,
 is_recurring, recurrence_plan, end_date, discount_percentage, discount_amount)
VALUES
(@Id, @CustomerId, @CleanerId, @BookingDate, @TimeSlot, @LocationId, @AddressDetail, 'Pending', @Notes, timezone('utc', now()), @TotalAmount,
 @IsRecurring, @RecurrencePlan, @EndDate, @DiscountPercentage, @DiscountAmount);";

            const string insertDetail = @"
INSERT INTO public.booking_details
(booking_detail_id, booking_id, service_id, quantity, price, remark, subtotal)
VALUES
(@DetailId, @BookingId, @ServiceId, @Quantity, @Price, @Remark, @subtotal);";


            var id = Guid.NewGuid();
            decimal subtotal = 0m;

            // 2. Calculate Subtotal (Pre-Discount Total)
            foreach (var item in dto.Items)
            {
                var price = item.Price ?? 0m;
                var qty = item.Quantity;
                subtotal += price * qty;
            }

            // 3. Calculate Discount and Final Total
            decimal discountPct = dto.DiscountPercentage ?? 0m;
            decimal discountAmount = subtotal * (discountPct / 100m);
            decimal finalTotalAmount = subtotal - discountAmount;

            // Ensure negative discount (credit) is not applied if percentage is negative/invalid
            if (discountAmount < 0) discountAmount = 0m;
            if (finalTotalAmount < 0) finalTotalAmount = 0m;

            // Use the final calculated amount as the "amount" stored in the bookings table
            // (Note: Your original column was called 'amount', we map finalTotalAmount to it)

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
                    dto.Notes,
                    TotalAmount = finalTotalAmount, // Mapped to 'amount' column in SQL

                    // 4. NEW PARAMETERS
                    IsRecurring = dto.IsRecurring ?? false, // Default to false
                    RecurrencePlan = dto.RecurrencePlan,
                    EndDate = dto.EndDate,
                    DiscountPercentage = discountPct,
                    DiscountAmount = discountAmount // The calculated amount
                }, tx);

                // details
                foreach (var item in dto.Items)
                {
                    var price = item.Price ?? 0m;
                    var itemSubtotal = price * item.Quantity; // Renamed to itemSubtotal for clarity

                    await conn.ExecuteAsync(insertDetail, new
                    {
                        DetailId = Guid.NewGuid(),
                        BookingId = id,
                        ServiceId = item.ServiceId,
                        Quantity = item.Quantity,
                        Price = item.Price ?? 0m,
                        Remark = item.Remark,
                        subtotal = itemSubtotal // Subtotal for the individual line item
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



        public async Task<IReadOnlyList<BookingDTO>> GetMyBooking(Guid? customerId)
        {
            var sql = @"
SELECT
    b.booking_id            AS ""BookingId"",
    u.user_id               AS ""CustomerId"",
    u.full_name             AS ""CustomerName"",
    u.phone                 AS ""CustomerPhone"",
    u1.user_id              AS ""CleanerId"",
    u1.full_name            AS ""CleanerName"",
    u1.phone                AS ""CleanerPhone"",
    b.booking_date          AS ""BookingDate"",
    b.time_slot             AS ""TimeSlot"",
    b.status                AS ""Status"",
    b.notes                 AS ""Notes"",
    b.address_detail        AS ""AddressDetail"",
    b.created_at            AS ""CreatedAt"",

    -- Recurring & Discount Fields
    b.is_recurring          AS ""IsRecurring"",
    b.recurrence_plan       AS ""RecurrencePlan"",
    b.end_date              AS ""EndDate"",
    b.discount_percentage   AS ""DiscountPercentage"",
    b.discount_amount       AS ""DiscountAmount"",
    b.amount                AS ""TotalAmountFinal"", 

    -- ✅ Payment Info (UPDATED WITH COALESCE - This is essential)
    p_latest.amount         AS ""AmountPaid"", 
    p_latest.payment_id     AS ""PaymentId"",
    COALESCE(p_latest.payment_status, 'Unpaid') AS ""PaymentStatus"", -- Sets 'Unpaid' if no payment record found

    -- Booking Details (Split starts here)
    d.booking_detail_id     AS ""BookingDetailId"", 
    d.service_id            AS ""ServiceId"",
    s.name                  AS ""ServiceName"",
    d.quantity              AS ""Quantity"",
    d.price                 AS ""Price"",
    d.subtotal              AS ""Subtotal"",
    d.remark                AS ""Remark""
FROM public.bookings b
LEFT JOIN public.users u ON u.user_id = b.customer_id
LEFT JOIN public.users u1 ON u1.user_id = b.cleaner_id
LEFT JOIN public.booking_details d ON d.booking_id = b.booking_id
LEFT JOIN public.services s ON s.service_id = d.service_id 

-- LATERAL JOIN to find the latest payment record (if any)
LEFT JOIN LATERAL (
    SELECT amount, payment_id, payment_status
    FROM public.payments p_inner
    WHERE p_inner.booking_id = b.booking_id
    ORDER BY p_inner.created_at DESC 
    LIMIT 1
) AS p_latest ON TRUE
WHERE b.customer_id = @CustomerId  or @CustomerId IS NULL
ORDER BY b.booking_date DESC, b.time_slot DESC, b.created_at DESC;";

            var bookingDictionary = new Dictionary<Guid, BookingDTO>();

            // Assuming _db is your database context/factory
            using var conn = _db.DbConnection;
            if (conn.State != ConnectionState.Open) conn.Open();

            var result = await conn.QueryAsync<BookingDTO, BookingDetailDTO, BookingDTO>(
                sql,
                (booking, detail) =>
                {
                    if (!bookingDictionary.TryGetValue(booking.BookingId, out var existingBooking))
                    {
                        // Dapper successfully mapped the non-split part (including payment info) 
                        // into this 'booking' object. Use it directly.
                        existingBooking = booking;
                        existingBooking.Details = new List<BookingDetailDTO>();

                        bookingDictionary.Add(existingBooking.BookingId, existingBooking);
                    }

                    // Add details
                    if (detail != null)
                    {
                        if (!existingBooking.Details.Any(d => d.BookingDetailId == detail.BookingDetailId))
                        {
                            existingBooking.Details.Add(detail);
                        }
                    }

                    return existingBooking;
                },
                new { CustomerId = customerId },
                splitOn: "BookingDetailId"
            );

            return bookingDictionary.Values.ToList();
        }
    }
}
