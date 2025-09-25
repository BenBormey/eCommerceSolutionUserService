

namespace eCommerce.Core.DTO.Service
{
    public class topservice
    {

        public int ServiceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? CategoryName { get; set; }

        // metrics
        public int BookingCount { get; set; }      // DISTINCT bookings count
        public int TotalQuantity { get; set; }     // SUM(quantity)
        public decimal Revenue { get; set; }
    }
}
