using eCommerce.Core.DTO.Category;
using eCommerce.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.ServiceContracts
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceDTO>> GetAllAsync();
        Task<ServiceDTO?> GetByIdAsync(int serviceId);

        // Create
        Task<ServiceDTO> CreateAsync(ServiceCreateDTO dto);

        // Update
        Task<ServiceDTO?> UpdateAsync(int serviceId, ServiceUpdateDTO dto);

        // Delete
        Task<bool> DeleteAsync(int serviceId);

        // Optional quality-of-life
        Task<bool> ExistsAsync(int serviceId);
        Task<bool> ToggleActiveAsync(int serviceId, bool isActive);
        Task<IEnumerable<ServiceDTO>> GetByCategory(Guid? categoryId);
        Task<IEnumerable<topservice>> GetTopservicefour();



    }
}
