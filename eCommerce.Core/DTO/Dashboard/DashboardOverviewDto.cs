using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Dashboard
{
    public class DashboardOverviewDto
    {
        public int TotalBookings { get; set; }
        public decimal Revenue { get; set; }
        public decimal Aov { get; set; }
        public int PendingToday { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
        public int ActiveCleaners { get; set; }
    }
}
