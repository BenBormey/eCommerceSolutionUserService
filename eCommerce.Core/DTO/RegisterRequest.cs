using eCommerce.Core.DTOl;

namespace eCommerce.Core.DTO;

public record class RegisterRequest(
    
    string? Email,
    string? Password,
    string? PersonName,
    GenderOption Gender
    );