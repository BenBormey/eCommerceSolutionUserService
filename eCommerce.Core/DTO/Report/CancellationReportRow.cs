using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public class CancellationReportRow
    {
        public string Reason { get; set; } = "";
        public int Count { get; set; }
    }
}
