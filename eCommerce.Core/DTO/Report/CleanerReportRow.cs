using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public class CleanerReportRow
    {
        public Guid CleanerId { get; set; }
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
        public int TotalJobs { get; set; }
        public int CompletedJobs { get; set; }
        public int CanceledJobs { get; set; }
        public decimal AvgRating { get; set; }
    }
}
