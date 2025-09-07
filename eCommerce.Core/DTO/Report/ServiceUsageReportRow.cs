using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public class ServiceUsageReportRow
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = "";
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
