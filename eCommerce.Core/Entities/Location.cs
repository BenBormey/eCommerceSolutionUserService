using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace eCommerce.Core.Entities
{
    [Table("locations")]
    public class Location
    {
        [Key]
        [Column("location_id")]
        public int LocationId { get; set; }

        [MaxLength(120)]
        [Column("city")]
        public string? City { get; set; }

        [MaxLength(120)]
        [Column("district")]
        public string? District { get; set; }

        [MaxLength(20)]
        [Column("postal_code")]
        public string? PostalCode { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // nav
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
