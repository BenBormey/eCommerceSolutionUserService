using System;

namespace eCommerce.Core.DTO.Report
{
    public class TopCustomerReportRow
    {
        public Guid CustomerId { get; set; }
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";        // added phone
        public int TotalBookings { get; set; }         // added number of bookings
        public decimal TotalRevenue { get; set; }      // total spent
    }
}
