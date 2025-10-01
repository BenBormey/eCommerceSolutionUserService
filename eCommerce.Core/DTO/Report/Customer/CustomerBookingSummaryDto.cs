using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report.Customer
{
    public class CustomerBookingSummaryDto
    {
        public Guid UserId { get; set; }          // u.user_id
        public string FullName { get; set; }      // u.full_name
        public int TotalBookings { get; set; }    // COUNT(bwb.booking_id)
        public decimal TotalRevenue { get; set; } // SUM(bwb.paid_amount)
        public decimal AvgBookingValue { get; set; } // AVG per booking
        public decimal MinBookingValue { get; set; } // MIN(bwb.paid_amount)
        public decimal MaxBookingValue { get; set; }
    }
}
