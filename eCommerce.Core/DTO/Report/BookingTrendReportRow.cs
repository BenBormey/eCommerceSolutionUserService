using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public class BookingTrendReportRow
    {
        public DateTime BookingDate { get; set; }
        public int TotalBookings { get; set; }
        public int CompletedBookings { get; set; }
        public decimal Revenue { get; set; }
    }
}
