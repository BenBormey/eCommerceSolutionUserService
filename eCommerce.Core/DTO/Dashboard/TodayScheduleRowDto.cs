using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Dashboard
{
    public class TodayScheduleRowDto
    {
        public Guid BookingId { get; set; }
        public string Date { get; set; } = "";     // "YYYY-MM-DD" (optional on UI)
        public string Time { get; set; } = "";     // "HH:mm"
        public string Customer { get; set; } = "";
        public string Status { get; set; } = "";
        public string Cleaner { get; set; } = "";
        public string? Services { get; set; }
        public decimal Total { get; set; }
    }
}
