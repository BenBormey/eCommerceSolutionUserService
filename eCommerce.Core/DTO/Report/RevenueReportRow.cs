using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public class RevenueReportRow
    {
        public string Period { get; set; } = "";
        public decimal TotalRevenue { get; set; }
    }
}
