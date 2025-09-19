using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO.Report
{
    public class StatusBreakdownDTO
    {
        [JsonPropertyName("pending")] public int Pending { get; set; }
        [JsonPropertyName("confirmed")] public int Confirmed { get; set; }
        [JsonPropertyName("completed")] public int Completed { get; set; }
        [JsonPropertyName("cancelled")] public int Cancelled { get; set; }
    }
}
