using eCommerce.Core.DTO;
using eCommerce.Core.DTO.Customer;
using eCommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.RepositoryContracts
{
    public interface IUsersRepository
    {
        /// <summary>
        /// Method to add a user to the data store and return the added user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ApplicationUser?> AddUser(ApplicationUser user);
        Task<IEnumerable<string>> GetRolesAsync(Guid userId);
        Task<Guid?> GetRoleIdByNameAsync(string roleName);
        Task<bool> AddUserRoleAsync(Guid userId, Guid roleId);
        Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password);
        Task<IEnumerable<CustomerDTO>> GetAllCustomer(string role);
        Task<CustomerDTO?> GetCustomerById(Guid roleId ,string role);
        Task<bool> UpdateCustomer(Guid custId, EditCustomer customer);    
        Task<bool> DeleteCustomer(Guid userId);
        //Task<IEnumerable<CustomerDTO>> GetUserByID(Guid userId );
       

    }
}
