using eCommerce.Core.DTO.Report;
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

    }
}
