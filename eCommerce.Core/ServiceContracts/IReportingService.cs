using eCommerce.Core.DTO.Report;
using eCommerce.Core.DTO.Report.Customer;
using eCommerce.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.ServiceContracts
{
    public interface IReportingService
    {
        Task<IReadOnlyList<CustomerReportRow>> GetCustomerReportAsync(
          DateTime? fromDate,
          DateTime? toDate,
          string? search,
          string? sort,
          int skip,
          int take);
      Task<IReadOnlyList<CleanerReportRow>> GetCleanerReportAsync(DateTime? fromDate, DateTime? toDate);
        Task<IReadOnlyList<ServiceUsageReportRow>> GetServiceUsageReportAsync(DateTime? fromDate, DateTime? toDate);
        Task<IReadOnlyList<RevenueReportRow>> GetRevenueReportAsync(string groupBy);
        Task<IReadOnlyList<BookingTrendReportRow>> GetBookingTrendReportAsync(DateTime? fromDate, DateTime? toDate);
        Task<IReadOnlyList<CancellationReportRow>> GetCancellationReportAsync(DateTime? fromDate, DateTime? toDate);
        Task<IReadOnlyList<TopCustomerReportRow>> GetTopCustomersReportAsync(int limitCount);
        Task<IReadOnlyList<PaymentReportRow>> GetPaymentReportAsync(DateTime? fromDate, DateTime? toDate);
        Task<IReadOnlyList<LocationReportRow>> GetLocationReportAsync();
        Task<IReadOnlyList<ServicePopularityDTO>> GetServicePopularity();
        Task<IReadOnlyList<overview>> overviews();
        Task<IReadOnlyList<StatusBreakdownDTO>> StatusBreakdown();
        Task<IReadOnlyList<RecentBookingItemDTO>> RecentBooking();
        Task<IReadOnlyList<CustomerBookingSummaryDto>> GetReportByCustomer();
        Task<IEnumerable<ServiceReportDto>> GetServiceReportsAsync(DateTime? startDate = null, DateTime? endDate = null);




    }
}
