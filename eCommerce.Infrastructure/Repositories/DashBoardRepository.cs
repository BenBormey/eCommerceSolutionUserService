using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using eCommerce.Core.DTO.Dashboard;
using eCommerce.Core.DTO.Booking;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;

namespace eCommerce.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DapperDbContext _db;

        public DashboardRepository(DapperDbContext db)
        {
            _db = db;
        }

        public async Task<DashboardOverviewDto> GetOverviewAsync(DateTime from, DateTime to)
        {
  
            var sql = @"
                SELECT 
                    COUNT(*) AS TotalBookings,
                    COALESCE(SUM(CASE WHEN status = 'completed' THEN amount ELSE 0 END),0) AS Revenue,
                    COALESCE(AVG(CASE WHEN status = 'completed' THEN amount END),0) AS AOV,
                    COUNT(*) FILTER (WHERE status = 'Pending' AND booking_date::date = CURRENT_DATE) AS PendingToday,
                    COUNT(*) FILTER (WHERE status = 'completed') AS Completed,
                    COUNT(*) FILTER (WHERE status = 'cancelled') AS Cancelled
                FROM bookings
                WHERE booking_date BETWEEN @From AND @To;
            ";
            return await _db.DbConnection.QueryFirstOrDefaultAsync<DashboardOverviewDto>(sql, new { From = from, To = to });
        }

        public async Task<IEnumerable<DashboardTrendDto>> GetTrendAsync(DateTime from, DateTime to, string groupBy = "day")
        {
            
            var sql = @"
                SELECT 
                    booking_date::date AS Date,
                    COUNT(*) AS Bookings,
                    COUNT(*) FILTER (WHERE status = 'completed') AS Completed,
                    COALESCE(SUM(CASE WHEN status = 'completed' THEN amount ELSE 0 END),0) AS Revenue
                FROM bookings
                WHERE booking_date BETWEEN @From AND @To
                GROUP BY booking_date::date
                ORDER BY booking_date::date;
            ";
            return await _db.DbConnection.QueryAsync<DashboardTrendDto>(sql, new { From = from, To = to });
        }

        public async Task<DashboardStatusDto> GetStatusAsync(DateTime from, DateTime to)
        {
         
            var sql = @"
                SELECT 
                    COUNT(*) FILTER (WHERE status = 'Pending')   AS Pending,
                    COUNT(*) FILTER (WHERE status = 'confirmed') AS Confirmed,
                    COUNT(*) FILTER (WHERE status = 'completed') AS Completed,
                    COUNT(*) FILTER (WHERE status = 'cancelled') AS Cancelled
                FROM bookings
                WHERE booking_date BETWEEN @From AND @To;
            ";
            return await _db.DbConnection.QueryFirstOrDefaultAsync<DashboardStatusDto>(sql, new { From = from, To = to });
        }

        public async Task<IEnumerable<BookingRecentDto>> GetRecentBookingsAsync(int take = 8)
        {
        
            var sql = @"
                SELECT 
                    b.booking_id   AS Id,
                    c.full_name    AS CustomerName,
                    b.booking_date AS Time,
                    l.name         AS Location,
                    b.status       AS Status
                FROM bookings b
                LEFT JOIN customers c ON c.customer_id = b.customer_id
                LEFT JOIN locations l ON l.location_id = b.location_id
                ORDER BY b.created_at DESC
                LIMIT @Take;
            ";
            return await _db.DbConnection.QueryAsync<BookingRecentDto>(sql, new { Take = take });
        }

        public async Task<IEnumerable<TodayScheduleRowDto>> GetSceduleRowDto()
        {
            var sql = @"SELECT
  b.booking_id                                            AS ""bookingId"",
  b.booking_date::date                                    AS ""date"",
  to_char(COALESCE(b.time_slot, (b.booking_date::timestamp)::time), 'HH24:MI')
                                                          AS ""time"",
  cu.full_name                                            AS ""customer"",
  b.status                                                AS ""status"",
  COALESCE(cl.full_name, '—')                             AS ""cleaner"",
  string_agg(DISTINCT s.name, ', ' ORDER BY s.name)       AS ""services"",
  ROUND(SUM(bd.quantity * bd.price)::numeric, 2)          AS ""total""
FROM bookings b
JOIN users cu                 ON cu.user_id = b.customer_id
LEFT JOIN users cl            ON cl.user_id = b.cleaner_id
LEFT JOIN booking_details bd  ON bd.booking_id = b.booking_id
LEFT JOIN services s          ON s.service_id  = bd.service_id
WHERE b.booking_date::date = CURRENT_DATE
  AND b.status IN ('Pending','Confirmed','In-Progress')
GROUP BY
  b.booking_id,
  b.booking_date::date,
  COALESCE(b.time_slot, (b.booking_date::timestamp)::time),
  cu.full_name,
  b.status,
  cl.full_name
ORDER BY ""time"";
";
            return await _db.DbConnection.QueryAsync<TodayScheduleRowDto>(sql);
        }
    }
}
