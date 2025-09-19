using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public class TrendRowDTO
    {
        [JsonPropertyName("bookingDate")] public DateTime BookingDate { get; set; }   // e.g. 2025-09-20
        [JsonPropertyName("totalBookings")] public int TotalBookings { get; set; }
        [JsonPropertyName("completed")] public int Completed { get; set; }   // <-- frontend expects "completed"
        [JsonPropertyName("revenue")] public decimal Revenue { get; set; }

    }
}
