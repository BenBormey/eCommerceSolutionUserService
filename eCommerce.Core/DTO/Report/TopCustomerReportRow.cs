using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public    class TopCustomerReportRow
    {
        public Guid CustomerId { get; set; }
        public string FullName { get; set; } = "";
        public decimal TotalRevenue { get; set; }
    }
}
