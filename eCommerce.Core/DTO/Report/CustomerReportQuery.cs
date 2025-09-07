using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public sealed class CustomerReportQuery
    {
        // Accept as strings to avoid DateOnly binding issues
        public string? FromDate { get; set; } // "yyyy-MM-dd"
        public string? ToDate { get; set; } // "yyyy-MM-dd"
        public string? Search { get; set; }
        public string? Sort { get; set; } // "revenue" | "last" | null
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
    }
}
