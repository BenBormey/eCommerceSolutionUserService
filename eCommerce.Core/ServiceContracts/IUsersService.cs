using eCommerce.Core.DTO;
using eCommerce.Core.DTO.Customer;
using eCommerce.Core.Entities;
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
    Task<IEnumerable<CustomerDTO>> GetAllCustomer(string role);
    Task<CustomerDTO?> GetCustomerById(Guid userId,string role);
    Task<bool> UpdateCustomer(Guid customerid, EditCustomer customer);
    Task<bool> DeleteCustomer(Guid userId);
    //Task<IEnumerable<CustomerDTO>> GetUserByID(Guid userId);
}
