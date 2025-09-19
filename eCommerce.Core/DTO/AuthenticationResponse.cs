using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO;

public record AuthenticationResponse(
     Guid UserId,
    string? Email,
    string? Fullname,
    string? Token,
    bool Success,
    string phone   ,
    string? Role = null
)
{
    public AuthenticationResponse() : this(default,default,default,default,default,default,default)
    {
        
    }
}