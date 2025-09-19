using eCommerce.Core.DTO.Report;
using eCommerce.Core.Entities;
using eCommerce.Core.Service;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportingService _reportingService;

        public ReportController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        // GET api/report/customer-report?fromDate=2025-09-01&toDate=2025-09-07&search=&sort=last&skip=0&take=50
        [HttpGet("customer-report")]
        public async Task<ActionResult<IReadOnlyList<CustomerReportRow>>> GetCustomerReport(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? search,
            [FromQuery] string? sort,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50)
        {
            var result = await _reportingService.GetCustomerReportAsync(fromDate, toDate, search, sort, skip, take);

            return Ok(result);
        }
        [HttpGet("cleaner-report")]
        public async Task<ActionResult<IReadOnlyList<CleanerReportRow>>> GetCleanerReport(
    [FromQuery] DateTime? fromDate,
    [FromQuery] DateTime? toDate)
        {
            var result = await _reportingService.GetCleanerReportAsync(fromDate, toDate);
            return Ok(result);
        }
        [HttpGet("service-usage-report")]
        [ProducesResponseType(typeof(IReadOnlyList<ServiceUsageReportRow>), 200)]
        public async Task<ActionResult<IReadOnlyList<ServiceUsageReportRow>>> GetServiceUsageReport(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var result = await _reportingService.GetServiceUsageReportAsync(fromDate, toDate);
            return Ok(result);
        }
        [HttpGet("revenue-report")]
        [ProducesResponseType(typeof(IReadOnlyList<RevenueReportRow>), 200)]
        public async Task<ActionResult<IReadOnlyList<RevenueReportRow>>> GetRevenueReport(
           [FromQuery] string groupBy = "month") // "day" | "month" | "year"
        {
            var result = await _reportingService.GetRevenueReportAsync(groupBy);
            return Ok(result);
        }
        [HttpGet("booking-trend-report")]
        [ProducesResponseType(typeof(IReadOnlyList<BookingTrendReportRow>), 200)]
        public async Task<ActionResult<IReadOnlyList<BookingTrendReportRow>>> GetBookingTrendReport(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var result = await _reportingService.GetBookingTrendReportAsync(fromDate, toDate);
            return Ok(result);
        }
        [HttpGet("cancellation-report")]
        [ProducesResponseType(typeof(IReadOnlyList<CancellationReportRow>), 200)]
        public async Task<ActionResult<IReadOnlyList<CancellationReportRow>>> GetCancellationReport(
           [FromQuery] DateTime? fromDate,
           [FromQuery] DateTime? toDate)
        {
            var result = await _reportingService.GetCancellationReportAsync(fromDate, toDate);
            return Ok(result);
        }
        [HttpGet("top-customers-report")]
        [ProducesResponseType(typeof(IReadOnlyList<TopCustomerReportRow>), 200)]
        public async Task<ActionResult<IReadOnlyList<TopCustomerReportRow>>> GetTopCustomersReport(
          [FromQuery] int limitCount = 10)
        {
            var result = await _reportingService.GetTopCustomersReportAsync(limitCount);
            return Ok(result);
        }
        [HttpGet("payment-report")]
        [ProducesResponseType(typeof(IReadOnlyList<PaymentReportRow>), 200)]
        public async Task<ActionResult<IReadOnlyList<PaymentReportRow>>> GetPaymentReport(
           [FromQuery] DateTime? fromDate,
           [FromQuery] DateTime? toDate)
        {
            var result = await _reportingService.GetPaymentReportAsync(fromDate, toDate);
            return Ok(result);
        }
        [HttpGet("location-report")]
        [ProducesResponseType(typeof(IReadOnlyList<LocationReportRow>), 200)]
        public async Task<ActionResult<IReadOnlyList<LocationReportRow>>> GetLocationReport()
        {
            var result = await _reportingService.GetLocationReportAsync();
            return Ok(result);
        }
        [HttpGet("Top-Service")]
        public async Task<ActionResult<IReadOnlyList<ServicePopularityDTO>>> GetservicePopularity()
        {
            var result = await _reportingService.GetServicePopularity();
            return Ok(result);  
        }

        [HttpGet("overview")]
        [ProducesResponseType(typeof(overview), StatusCodes.Status200OK)]
        public async Task<IActionResult> Overview()
        {
            var row = await _reportingService.overviews();
            return Ok(row);
        }

        // GET: /api/Dashboard/status?from=...&to=...
        [HttpGet("status")]
        [ProducesResponseType(typeof(StatusBreakdownDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> Status([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            var row = await _reportingService.StatusBreakdown();
            return Ok(row);
        }

        // GET: /api/Dashboard/recent?take=8
        [HttpGet("recent")]
        [ProducesResponseType(typeof(IEnumerable<RecentBookingItemDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Recent([FromQuery] int take = 8)
        {
            var items = await _reportingService.RecentBooking();
            return Ok(items);
        }

    }
    
}

