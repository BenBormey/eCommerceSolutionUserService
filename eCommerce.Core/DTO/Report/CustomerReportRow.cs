using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public class CustomerReportRow
    {
        public Guid  CustomerId { get; set; }   // nullable is safer than all-zero GUID
        public string FullName { get; set; } = "";
        public string ls{ get; set; } = "";
     public string Email { get; set; } = "";
        public DateTime? FirstBookingDate { get; set; }
        public DateTime? LastBookingDate { get; set; }
        public int TotalBookings { get; set; }
        public int CompletedBookings { get; set; }
        public int CanceledBookings { get; set; }
        public int NoShowBookings { get; set; }
        public int DistinctServicesUsed { get; set; }
        public string LastServiceName { get; set; } = "";
        public string LastBookingStatus { get; set; } = "";
        public decimal TotalRevenue { get; set; }
        public decimal AvgOrderValue { get; set; }
    }
}
