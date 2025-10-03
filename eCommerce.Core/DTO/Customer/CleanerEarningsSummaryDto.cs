using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Customer
{
    public class CleanerEarningsSummaryDto
    {
        public Guid CleanerId { get; set; }
        public string CleanerName { get; set; }
        public string CleanerEmail { get; set; }

        public decimal? TotalGrossEarnings { get; set; } // Change to nullable
        public decimal? TotalPaidToCleaner { get; set; }  // Change to nullable
        public decimal? TotalPendingPayment { get; set; }
        public int TotalJobsAssigned { get; set; }
    }
}
