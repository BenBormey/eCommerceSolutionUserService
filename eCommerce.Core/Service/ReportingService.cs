using eCommerce.Core.DTO.Report;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Service
{
    public class ReportingService : IReportingService
    {
        private readonly IReportRepository _reportRepository;

        public ReportingService(IReportRepository repository)
        {
            _reportRepository = repository;
        }

        public async Task<IReadOnlyList<CustomerReportRow>> GetCustomerReportAsync(
            DateTime? fromDate,
            DateTime? toDate,
            string? search,
            string? sort,
            int skip,
            int take)
        {
            // Optional: validation/business rules here
            if (take <= 0) take = 50;
            if (skip < 0) skip = 0;

            return await _reportRepository.GetCustomerReportAsync(fromDate, toDate, search, sort, skip, take);
        }
           public Task<IReadOnlyList<CleanerReportRow>> GetCleanerReportAsync(DateTime? fromDate, DateTime? toDate)
            => _reportRepository.GetCleanerReportAsync(fromDate, toDate);

        public Task<IReadOnlyList<ServiceUsageReportRow>> GetServiceUsageReportAsync(DateTime? fromDate, DateTime? toDate)
            => _reportRepository.GetServiceUsageReportAsync(fromDate, toDate);

        public Task<IReadOnlyList<RevenueReportRow>> GetRevenueReportAsync(string groupBy)
            => _reportRepository.GetRevenueReportAsync(groupBy);

        public Task<IReadOnlyList<BookingTrendReportRow>> GetBookingTrendReportAsync(DateTime? fromDate, DateTime? toDate)
            => _reportRepository.GetBookingTrendReportAsync(fromDate, toDate);

        public Task<IReadOnlyList<CancellationReportRow>> GetCancellationReportAsync(DateTime? fromDate, DateTime? toDate)
            => _reportRepository.GetCancellationReportAsync(fromDate, toDate);

        public Task<IReadOnlyList<TopCustomerReportRow>> GetTopCustomersReportAsync(int limitCount)
            => _reportRepository.GetTopCustomersReportAsync(limitCount);

        public Task<IReadOnlyList<PaymentReportRow>> GetPaymentReportAsync(DateTime? fromDate, DateTime? toDate)
            => _reportRepository.GetPaymentReportAsync(fromDate, toDate);

        public Task<IReadOnlyList<LocationReportRow>> GetLocationReportAsync()
            => _reportRepository.GetLocationReportAsync();
    }
}
