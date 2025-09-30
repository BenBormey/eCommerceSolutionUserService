using System.ComponentModel.DataAnnotations;

namespace eCommerce.Core.Entities;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Define the ApplicationUser class which acts as entiry model class to store user details in data store
/// </summary>
public class ApplicationUser
{
    [Key] public Guid UserId { get; set; }

    [MaxLength(150)] public string? FullName { get; set; }
    [MaxLength(150)] public string? Email { get; set; }
    [MaxLength(50)] public string? Phone { get; set; }
    public string PasswordHash { get; set; } = null!;

    [MaxLength(500)] public string? ProfileImage { get; set; }
    public bool IsActive { get; set; } = true;
    [MaxLength(30)] public string? Status { get; set; }   // optional free-text status
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Role { get; set; } = "Customer";


    // Navigations
    public ICollection<Booking> CustomerBookings { get; set; } = new List<Booking>();
    public ICollection<Booking> CleanerBookings { get; set; } = new List<Booking>();
    public ICollection<CleanerAvailability> Availabilities { get; set; } = new List<CleanerAvailability>();
    public ICollection<Review> ReviewsByCustomer { get; set; } = new List<Review>();
    public ICollection<Review> ReviewsForCleaner { get; set; } = new List<Review>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<AdminLog> AdminLogs { get; set; } = new List<AdminLog>();
    public ICollection<Location> Login { get; set; }

}
