using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public class LocationReportRow
   {
   public int LocationId { get; set; }
    public string LocationName { get; set; } = "";
    public int TotalBookings { get; set; }
}
}
