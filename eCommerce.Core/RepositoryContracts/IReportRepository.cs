using eCommerce.Core.DTO.Report;
using eCommerce.Core.DTO.Report.Customer;
using eCommerce.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.RepositoryContracts
{
    public interface IReportRepository
    {
        Task<IReadOnlyList<CustomerReportRow>> GetCustomerReportAsync(
        DateTime? fromDate,
        DateTime? toDate,
        string? search,
        string? sort,
        int skip,
        int take);

            // 2. Cleaner Report
            Task<IReadOnlyList<CleanerReportRow>> GetCleanerReportAsync(
                DateTime? fromDate,
                DateTime? toDate);

            // 3. Service Usage Report
            Task<IReadOnlyList<ServiceUsageReportRow>> GetServiceUsageReportAsync(
                DateTime? fromDate,
                DateTime? toDate);

            // 4. Revenue Report
            Task<IReadOnlyList<RevenueReportRow>> GetRevenueReportAsync(
                string groupBy); // "day", "month", "year"

            // 5. Booking Trend Report
            Task<IReadOnlyList<BookingTrendReportRow>> GetBookingTrendReportAsync(
                DateTime? fromDate,
                DateTime? toDate);

            // 6. Cancellation / NoShow Report
            Task<IReadOnlyList<CancellationReportRow>> GetCancellationReportAsync(
                DateTime? fromDate,
                DateTime? toDate);

            // 7. Top Customers Report
            Task<IReadOnlyList<TopCustomerReportRow>> GetTopCustomersReportAsync(
                int limitCount);

            // 8. Payment Report
            Task<IReadOnlyList<PaymentReportRow>> GetPaymentReportAsync(
                DateTime? fromDate,
                DateTime? toDate);

            // 9. Location Report
            Task<IReadOnlyList<LocationReportRow>> GetLocationReportAsync();

        Task<IReadOnlyList<overview>> overviews();
        Task<IReadOnlyList<StatusBreakdownDTO>> StatusBreakdown();
        Task<IReadOnlyList<RecentBookingItemDTO>> RecentBooking();
        Task<IReadOnlyList<ServicePopularityDTO>> GetServicePopularity();
        Task<IReadOnlyList<CustomerBookingSummaryDto>> GetReportByCustomer();
        Task<IEnumerable<ServiceReportDto>> GetServiceReportsAsync(DateTime? startDate = null, DateTime? endDate = null);






    }
}
