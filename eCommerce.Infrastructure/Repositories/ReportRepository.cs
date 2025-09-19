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

            // បើ function ត្រូវ date-ONLY, ដាក់ Date ទេ (កាត់ Time ចេញ)
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

            // Dapper នឹង map null -> DBNull.Value ដោយស្វ័យប្រវត្តិ
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

       

            // 2. Cleaner Report
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

            // 3. Service Usage Report
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

            // 4. Revenue Report
            public async Task<IReadOnlyList<RevenueReportRow>> GetRevenueReportAsync(string groupBy)
            {
                const string sql = @"SELECT * FROM public.get_revenue_report(@GroupBy);";

                var param = new { GroupBy = groupBy ?? "month" };

                var rows = await _context.DbConnection.QueryAsync<RevenueReportRow>(sql, param);
                return rows.AsList();
            }

            // 5. Booking Trend Report
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

            // 6. Cancellation Report
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

            // 7. Top Customers Report
            public async Task<IReadOnlyList<TopCustomerReportRow>> GetTopCustomersReportAsync(int limitCount)
            {
                const string sql = @"SELECT * FROM public.get_top_customers_report(@LimitCount);";

                var param = new { LimitCount = limitCount };

                var rows = await _context.DbConnection.QueryAsync<TopCustomerReportRow>(sql, param);
                return rows.AsList();
            }

            // 8. Payment Report
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
    }
    
}

