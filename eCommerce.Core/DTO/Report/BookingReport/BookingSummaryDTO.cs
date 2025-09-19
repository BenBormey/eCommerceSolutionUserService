using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report.BookingReport
{
    public class BookingSummaryDTO
    {
        public DateTime Day { get; set; }
        public int TotalBookings { get; set; }
    }
}
