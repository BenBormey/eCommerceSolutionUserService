using Microsoft.AspNetCore.Mvc;
using eCommerce.Core.ServiceContracts;
using System;
using System.Threading.Tasks;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: api/dashboard/overview?from=2025-09-01&to=2025-09-18
        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var end = to ?? DateTime.UtcNow;              // default = now
            var start = from ?? end.AddDays(-30);
            var result = await _dashboardService.GetOverviewAsync(start, end);
            return Ok(result);
        }

        // GET: api/dashboard/trend?from=2025-09-01&to=2025-09-18
        [HttpGet("trend")]
        public async Task<IActionResult> GetTrend([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _dashboardService.GetTrendAsync(from, to);
            return Ok(result);
        }

        // GET: api/dashboard/status?from=2025-09-01&to=2025-09-18
        [HttpGet("status")]
        public async Task<IActionResult> GetStatus([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _dashboardService.GetStatusAsync(from, to);
            return Ok(result);
        }

        // GET: api/dashboard/recent?take=8
        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent([FromQuery] int take = 8)
        {
            var result = await _dashboardService.GetRecentBookingsAsync(take);
            return Ok(result);
        }
    }
}
