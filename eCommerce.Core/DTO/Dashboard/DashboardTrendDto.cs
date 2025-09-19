using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Dashboard
{
    public class DashboardTrendDto
    {
        public string Date { get; set; } // formatted "yyyy-MM-dd"
        public int Bookings { get; set; }
        public int Completed { get; set; }
        public decimal Revenue { get; set; }
    }
}
