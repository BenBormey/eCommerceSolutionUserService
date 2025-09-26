using eCommerce.Core.DTO;
using eCommerce.Core.DTO.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.ServiceContracts;

/// <summary>
/// 
/// </summary>
public interface IUsersService
{
 Task<AuthenticationResponse?> Login(LoginRequest loginRequest);
 Task<AuthenticationResponse?> Register(RegisterRequest registerRequest);
    Task<IEnumerable<CustomerDTO>> GetAllCustomer();
    Task<CustomerDTO?> GetCustomerById(Guid userId);
    Task<bool> UpdateCustomer(CustomerDTO customer);
    Task<bool> DeleteCustomer(Guid userId);
}
