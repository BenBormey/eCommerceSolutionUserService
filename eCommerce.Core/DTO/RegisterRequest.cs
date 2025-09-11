using eCommerce.Core.DTOl;
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Core.DTO;

public  class RegisterRequest()
{
  
    [Required, MaxLength(150)]
    public string FullName { get; set; } = null!;

    [Required, MaxLength(150), EmailAddress]
    public string Email { get; set; } = null!;

    [MaxLength(30)]
    public string? Phone { get; set; }

    // Plain password from client; NEVER store this directly
    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = null!;

    // Optional – default will be "Customer"
    [MaxLength(30)]
    public string? Role { get; set; } = "Customer";
}