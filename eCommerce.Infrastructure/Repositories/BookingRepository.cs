using Dapper;
using eCommerce.Core.DTO.Booking;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;
using System.Data;

namespace eCommerce.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DapperDbContext _db;
        public BookingRepository(DapperDbContext db) => _db = db;

        public async Task<BookingDTO?> GetById(Guid bookingId)
        {
            const string sql = @"
SELECT
  booking_id     AS ""BookingId"",
  customer_id    AS ""CustomerId"",
  cleaner_id     AS ""CleanerId"",
  service_id     AS ""ServiceId"",
  booking_date   AS ""BookingDate"",
  time_slot      AS ""TimeSlot"",
  location_id    AS ""LocationId"",
  address_detail AS ""AddressDetail"",
  status         AS ""Status"",
  notes          AS ""Notes"",
  created_at     AS ""CreatedAt""
FROM public.bookings
WHERE booking_id = @Id;";

            return await _db.DbConnection.QueryFirstOrDefaultAsync<BookingDTO>(sql, new { Id = bookingId });
        }

        public async Task<IEnumerable<BookingDTO>> GetByCustomer(Guid customerId, DateTime? from, DateTime? to)
        {
            const string sql = @"
SELECT
  booking_id     AS ""BookingId"",
  customer_id    AS ""CustomerId"",
  cleaner_id     AS ""CleanerId"",
  service_id     AS ""ServiceId"",
  booking_date   AS ""BookingDate"",
  time_slot      AS ""TimeSlot"",
  location_id    AS ""LocationId"",
  address_detail AS ""AddressDetail"",
  status         AS ""Status"",
  notes          AS ""Notes"",
  created_at     AS ""CreatedAt""
FROM public.bookings
WHERE customer_id = @CustomerId
  AND (@From IS NULL OR booking_date >= @From)
  AND (@To   IS NULL OR booking_date <= @To)
ORDER BY booking_date DESC, time_slot DESC;";

            return await _db.DbConnection.QueryAsync<BookingDTO>(sql, new { CustomerId = customerId, From = from, To = to });
        }

        public async Task<IEnumerable<BookingDTO>> GetByCleaner(Guid cleanerId, DateTime? from, DateTime? to)
        {
            const string sql = @"
SELECT
  booking_id     AS ""BookingId"",
  customer_id    AS ""CustomerId"",
  cleaner_id     AS ""CleanerId"",
  service_id     AS ""ServiceId"",
  booking_date   AS ""BookingDate"",
  time_slot      AS ""TimeSlot"",
  location_id    AS ""LocationId"",
  address_detail AS ""AddressDetail"",
  status         AS ""Status"",
  notes          AS ""Notes"",
  created_at     AS ""CreatedAt""
FROM public.bookings
WHERE cleaner_id = @CleanerId
  AND (@From IS NULL OR booking_date >= @From)
  AND (@To   IS NULL OR booking_date <= @To)
ORDER BY booking_date DESC, time_slot DESC;";

            return await _db.DbConnection.QueryAsync<BookingDTO>(sql, new { CleanerId = cleanerId, From = from, To = to });
        }

        public async Task<BookingDTO> Create(BookingCreateDTO dto)
        {
            const string sql = @"
INSERT INTO public.bookings
(booking_id, customer_id, cleaner_id, service_id, booking_date, time_slot,
 location_id, address_detail, status, notes, created_at)
VALUES
(@Id, @CustomerId, @CleanerId, @ServiceId, @BookingDate, @TimeSlot,
 @LocationId, @AddressDetail, 'pending', @Notes, NOW())
RETURNING
  booking_id     AS ""BookingId"",
  customer_id    AS ""CustomerId"",
  cleaner_id     AS ""CleanerId"",
  service_id     AS ""ServiceId"",
  booking_date   AS ""BookingDate"",
  time_slot      AS ""TimeSlot"",
  location_id    AS ""LocationId"",
  address_detail AS ""AddressDetail"",
  status         AS ""Status"",
  notes          AS ""Notes"",
  created_at     AS ""CreatedAt"";
";

            //var status = string.IsNullOrWhiteSpace(dto.Status) ? "pending" : dto.Status.Trim().ToLowerInvariant();
            //if (status == "inprogress") status = "in_progress"; // match your CHECK

            var param = new
            {
                Id = Guid.NewGuid(),
                dto.CustomerId,
                dto.CleanerId,
                dto.ServiceId,
                BookingDate = dto.BookingDate,// DateTime → DateTime
                TimeSlot = dto.TimeSlot,                    // TimeSpan → TimeSpan
                                                                          // TimeSpan -> TimeSpan
                dto.LocationId,
                dto.AddressDetail,
                                                          // lowercase to pass CHECK
                dto.Notes
            };

            // connection is NpgsqlConnection
            var result = await _db.DbConnection.QuerySingleAsync<BookingDTO>(sql, param);
            return result;

        }

        public async Task<BookingDTO?> Update(Guid bookingId, BookingUpdateDTO dto)
        {
            const string sql = @"
UPDATE public.bookings
SET
  customer_id    = COALESCE(@CustomerId,    customer_id),
  cleaner_id     = COALESCE(@CleanerId,     cleaner_id),
  service_id     = COALESCE(@ServiceId,     service_id),
  booking_date   = COALESCE(@BookingDate,   booking_date),
  time_slot      = COALESCE(@TimeSlot,      time_slot),
  location_id    = COALESCE(@LocationId,    location_id),
  address_detail = COALESCE(@AddressDetail, address_detail),
  notes          = COALESCE(@Notes,         notes),
  status         = COALESCE(@Status,        status)
WHERE booking_id = @Id
RETURNING
  booking_id     AS ""BookingId"",
  customer_id    AS ""CustomerId"",
  cleaner_id     AS ""CleanerId"",
  service_id     AS ""ServiceId"",
  booking_date   AS ""BookingDate"",
  time_slot      AS ""TimeSlot"",
  location_id    AS ""LocationId"",
  address_detail AS ""AddressDetail"",
  status         AS ""Status"",
  notes          AS ""Notes"",
  created_at     AS ""CreatedAt"";";

            return await _db.DbConnection.QueryFirstOrDefaultAsync<BookingDTO>(sql, new
            {
                Id = bookingId,
                dto.CustomerId,
                dto.CleanerId,
                dto.ServiceId,
                dto.BookingDate,
                dto.TimeSlot,
                dto.LocationId,
                dto.AddressDetail,
                dto.Notes,
                dto.Status
            });
        }

        public async Task<bool> Delete(Guid bookingId)
        {
            const string sql = @"DELETE FROM public.bookings WHERE booking_id = @Id;";
            var rows = await _db.DbConnection.ExecuteAsync(sql, new { Id = bookingId });
            return rows > 0;
        }

        public async Task<bool> ChangeStatus(Guid bookingId, string status)
        {
            const string sql = @"UPDATE public.bookings SET status = @Status WHERE booking_id = @Id;";
            var rows = await _db.DbConnection.ExecuteAsync(sql, new { Id = bookingId, Status = status });
            return rows > 0;
        }

        public async Task<bool> ExistsAsync(Guid bookingId)
        {
            const string sql = @"SELECT EXISTS(SELECT 1 FROM public.bookings WHERE booking_id = @Id);";
            return await _db.DbConnection.ExecuteScalarAsync<bool>(sql, new { Id = bookingId });
        }
    }
}
