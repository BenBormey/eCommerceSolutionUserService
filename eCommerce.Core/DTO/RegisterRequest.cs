using eCommerce.Core.DTOl;
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Core.DTO;

public  class RegisterRequest()
{
 
        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(150, ErrorMessage = "Full name must not exceed 150 characters.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(150, ErrorMessage = "Email must not exceed 150 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

        [MaxLength(50, ErrorMessage = "Phone number must not exceed 50 characters.")]
        public string? Phone { get; set; }

        // Plain password from client (will be hashed in backend)
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [MaxLength(100, ErrorMessage = "Password must not exceed 100 characters.")]
        public string Password { get; set; } = null!;

        [MaxLength(30, ErrorMessage = "Role must not exceed 30 characters.")]
        public string? Role { get; set; } = "Customer";

        [MaxLength(500, ErrorMessage = "Profile image URL must not exceed 500 characters.")]
        public string? ProfileImage { get; set; }
   
}