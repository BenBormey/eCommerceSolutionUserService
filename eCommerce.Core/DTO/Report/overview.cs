using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public class overview
    {
        [JsonPropertyName("totalBookings")] public int TotalBookings { get; set; }
        [JsonPropertyName("revenue")] public decimal Revenue { get; set; }
        [JsonPropertyName("aov")] public decimal Aov { get; set; }   // Revenue / Completed
        [JsonPropertyName("pendingToday")] public int PendingToday { get; set; }
        [JsonPropertyName("completed")] public int Completed { get; set; }
        [JsonPropertyName("cancelled")] public int Cancelled { get; set; }
    }

}