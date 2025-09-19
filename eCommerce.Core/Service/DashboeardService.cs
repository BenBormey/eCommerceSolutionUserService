using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerce.Core.DTO.Dashboard;
using eCommerce.Core.DTO.Booking;
using eCommerce.Core.ServiceContracts;
using eCommerce.Core.RepositoryContracts;

namespace eCommerce.Core.Service
{
    // NOTE: should be public so it can be injected via DI
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public Task<DashboardOverviewDto> GetOverviewAsync(DateTime from, DateTime to)
        {
            return _repository.GetOverviewAsync(from, to);
        }

        public Task<IEnumerable<DashboardTrendDto>> GetTrendAsync(DateTime from, DateTime to, string groupBy = "day")
        {
            return _repository.GetTrendAsync(from, to, groupBy);
        }

        public Task<DashboardStatusDto> GetStatusAsync(DateTime from, DateTime to)
        {
            return _repository.GetStatusAsync(from, to);
        }

        public Task<IEnumerable<BookingRecentDto>> GetRecentBookingsAsync(int take = 8)
        {
            return _repository.GetRecentBookingsAsync(take);
        }
    }
}
