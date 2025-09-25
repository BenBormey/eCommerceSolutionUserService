using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerce.Core.DTO.Dashboard;
using eCommerce.Core.DTO.Booking;

namespace eCommerce.Core.RepositoryContracts
{
    public interface IDashboardRepository
    {
    
        Task<DashboardOverviewDto> GetOverviewAsync(DateTime from, DateTime to);

    
        Task<IEnumerable<DashboardTrendDto>> GetTrendAsync(DateTime from, DateTime to, string groupBy = "day");

       
        Task<DashboardStatusDto> GetStatusAsync(DateTime from, DateTime to);

     
        Task<IEnumerable<BookingRecentDto>> GetRecentBookingsAsync(int take = 8);
        Task<IEnumerable<TodayScheduleRowDto>> GetSceduleRowDto();

    }
}
