using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Service
{
    public class ServiceReportDto
    {
        public int ServiceId { get; set; }          // ID of the service
        public string Name { get; set; }            // Service name
        public int DurationMinutes { get; set; }    // Duration of service in minutes
        public int TotalBookings { get; set; }      // Total number of bookings
        public decimal TotalRevenue { get; set; }
    }
}
