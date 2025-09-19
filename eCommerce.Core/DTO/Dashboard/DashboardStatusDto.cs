using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Dashboard
{

    public class DashboardStatusDto
    {
        public int Pending { get; set; }
        public int Confirmed { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
    }
}
