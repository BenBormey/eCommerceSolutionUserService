using Dapper;
using eCommerce.Core.DTO.Report;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DapperDbContext _context;
        public ReportRepository(DapperDbContext context)
        {
             this._context = context;

        }
        public async Task<IReadOnlyList<CustomerReportRow>> GetCustomerReportAsync(DateTime? fromDate, DateTime? toDate, string? search, string? sort, int skip, int take)
        {
            var sortKey = (sort ?? "last").Trim().ToLower();
            switch (sortKey)
            {
                case "last":
                case "first":
                case "created":
                case "email":
                case "total":
                    break;
                default:
                    sortKey = "last";
                    break;
            }

            DateTime? from = fromDate?.Date;
            DateTime? to = toDate?.Date;

            const string sql = @"
SELECT *
FROM public.get_customer_report_v2(
    @FromDate::date,
    @ToDate::date,
    @Search,
    @Sort,
    @Skip,
    @Take
);";

            var param = new
            {
                FromDate = from,
                ToDate = to,
                Search = string.IsNullOrWhiteSpace(search) ? null : search,
                Sort = sortKey,
                Skip = skip,
                Take = take
            };

         
            var rows = await _context.DbConnection.QueryAsync<CustomerReportRow>(sql, param);
            return rows.AsList();
        }

       

           
            public async Task<IReadOnlyList<CleanerReportRow>> GetCleanerReportAsync(DateTime? fromDate, DateTime? toDate)
            {
                const string sql = @"SELECT * FROM public.get_cleaner_report(@FromDate::date, @ToDate::date);";

                var param = new
                {
                    FromDate = fromDate?.Date,
                    ToDate = toDate?.Date
                };

                var rows = await _context.DbConnection.QueryAsync<CleanerReportRow>(sql, param);
                return rows.AsList();
            }

            public async Task<IReadOnlyList<ServiceUsageReportRow>> GetServiceUsageReportAsync(DateTime? fromDate, DateTime? toDate)
            {
                const string sql = @"SELECT * FROM public.get_service_usage_report(@FromDate::date, @ToDate::date);";

                var param = new
                {
                    FromDate = fromDate?.Date,
                    ToDate = toDate?.Date
                };

                var rows = await _context.DbConnection.QueryAsync<ServiceUsageReportRow>(sql, param);
                return rows.AsList();
            }


            public async Task<IReadOnlyList<RevenueReportRow>> GetRevenueReportAsync(string groupBy)
            {
                const string sql = @"SELECT * FROM public.get_revenue_report(@GroupBy);";

                var param = new { GroupBy = groupBy ?? "month" };

                var rows = await _context.DbConnection.QueryAsync<RevenueReportRow>(sql, param);
                return rows.AsList();
            }


            public async Task<IReadOnlyList<BookingTrendReportRow>> GetBookingTrendReportAsync(DateTime? fromDate, DateTime? toDate)
            {
                const string sql = @"SELECT * FROM public.get_booking_trend_report(@FromDate::date, @ToDate::date);";

                var param = new
                {
                    FromDate = fromDate?.Date,
                    ToDate = toDate?.Date
                };

                var rows = await _context.DbConnection.QueryAsync<BookingTrendReportRow>(sql, param);
                return rows.AsList();
            }


            public async Task<IReadOnlyList<CancellationReportRow>> GetCancellationReportAsync(DateTime? fromDate, DateTime? toDate)
            {
                const string sql = @"SELECT * FROM public.get_cancellation_report(@FromDate::date, @ToDate::date);";

                var param = new
                {
                    FromDate = fromDate?.Date,
                    ToDate = toDate?.Date
                };

                var rows = await _context.DbConnection.QueryAsync<CancellationReportRow>(sql, param);
                return rows.AsList();
            }

    
            public async Task<IReadOnlyList<TopCustomerReportRow>> GetTopCustomersReportAsync(int limitCount)
            {
                const string sql = @"SELECT * FROM public.get_top_customers_report(@LimitCount);";

                var param = new { LimitCount = limitCount };

                var rows = await _context.DbConnection.QueryAsync<TopCustomerReportRow>(sql, param);
                return rows.AsList();
            }

         
            public async Task<IReadOnlyList<PaymentReportRow>> GetPaymentReportAsync(DateTime? fromDate, DateTime? toDate)
            {
                const string sql = @"SELECT * FROM public.get_payment_report(@FromDate::date, @ToDate::date);";

                var param = new
                {
                    FromDate = fromDate?.Date,
                    ToDate = toDate?.Date
                };

                var rows = await _context.DbConnection.QueryAsync<PaymentReportRow>(sql, param);
                return rows.AsList();
            }

            // 9. Location Report
            public async Task<IReadOnlyList<LocationReportRow>> GetLocationReportAsync()
            {
                const string sql = @"SELECT * FROM public.get_location_report();";

                var rows = await _context.DbConnection.QueryAsync<LocationReportRow>(sql);
                return rows.AsList();
            }

        public async Task<IReadOnlyList<ServicePopularityDTO>> GetServicePopularity()
        {
            const string sql = @"SELECT 
    s.service_id AS ServiceId ,
    s.name AS  ServiceName,
    COUNT(d.booking_detail_id) AS TotalBookings,
    SUM(d.quantity) AS TotalQuantity,
    SUM(d.price * d.quantity)::numeric(12,2) AS TotalRevenue
FROM public.booking_details d
JOIN public.services s ON s.service_id = d.service_id
GROUP BY s.service_id, s.name
ORDER BY TotalBookings DESC, TotalRevenue DESC";


            var row = await _context.DbConnection.QueryAsync<ServicePopularityDTO>(sql);
            return row.AsList();
        }

        public async Task<IReadOnlyList<overview>> overviews()
        {
            const string sql = @"

                WITH base AS (
  SELECT
    b.booking_id,
    lower(b.status) AS status,
    b.created_at,
    COALESCE(b.amount, 0) AS amount
  FROM public.bookings b
)
SELECT
  COUNT(*)                                                           AS ""TotalBookings"",
  COALESCE(SUM(CASE WHEN status = 'completed' THEN amount ELSE 0 END), 0)::numeric(12,2) AS ""Revenue"",
  COUNT(*) FILTER (WHERE status = 'completed')                       AS ""Completed"",
  COUNT(*) FILTER (WHERE status IN ('declined','cancelled'))         AS ""Cancelled"",
  COUNT(*) FILTER (WHERE status = 'pending' AND DATE(created_at) = CURRENT_DATE)         AS ""PendingToday"",
  COALESCE(SUM(CASE WHEN status = 'completed' AND DATE(created_at)=CURRENT_DATE
           THEN amount ELSE 0 END), 0)::numeric(12,2)                AS ""RevenueToday"",
  CASE WHEN COUNT(*) FILTER (WHERE status = 'completed') = 0 THEN 0
       ELSE ROUND((
         SUM(CASE WHEN status='completed' THEN amount ELSE 0 END)
         / NULLIF(COUNT(*) FILTER (WHERE status='completed'), 0)
       )::numeric, 2)
  END                                                                AS ""Aov""
FROM base;
";
            var row = await _context.DbConnection.QueryAsync<overview>(sql);
            return row.AsList();
        }

        public async Task<IReadOnlyList<StatusBreakdownDTO>> StatusBreakdown()
        {
            const string sql = @"
SELECT
  COUNT(*) FILTER (WHERE lower(status) = 'pending')                       AS ""Pending"",
  COUNT(*) FILTER (WHERE lower(status) IN ('assigned','confirmed'))       AS ""Confirmed"",
  COUNT(*) FILTER (WHERE lower(status) = 'completed')                     AS ""Completed"",
  COUNT(*) FILTER (WHERE lower(status) IN ('declined','cancelled'))       AS ""Cancelled""
FROM public.bookings;";

            var rows = await _context.DbConnection.QueryAsync<StatusBreakdownDTO>(sql);
            return rows.AsList(); // single row
        }

        public async Task<IReadOnlyList<RecentBookingItemDTO>> RecentBooking()
        {
            const string sql = @"
SELECT
  b.booking_id                                           AS ""Id"",
  COALESCE(c.full_name, c.email, 'Customer')             AS ""CustomerName"",
 sc.name                                           AS ""Category"",
  COALESCE(b.amount, SUM(d.price * d.quantity), 0)::numeric(12,2) AS ""Price"",
  b.status                                               AS ""Status"",
  b.created_at                                           AS ""CreatedAt""
FROM public.bookings b
LEFT JOIN public.booking_details d ON d.booking_id = b.booking_id
LEFT JOIN public.users c       ON c.user_id   = b.customer_id    inner join public.services as s on s.service_id 
= d.service_id inner join public.categories as sc on sc.category_id = s.category_id
GROUP BY b.booking_id, c.full_name, c.email, b.status, b.created_at, b.amount ,sc.name 
ORDER BY b.created_at DESC
LIMIT 5;";

            var rows = await _context.DbConnection.QueryAsync<RecentBookingItemDTO>(sql);
            return rows.AsList();
        }
    }
    
}

