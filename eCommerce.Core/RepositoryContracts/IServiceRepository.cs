using eCommerce.Core.DTO.Category;
using eCommerce.Core.DTO.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.RepositoryContracts
{
    public interface IServiceRepository
    {
      // Get all services
            Task<IEnumerable<ServiceDTO>> GetService();

            // Get service by Id
            Task<ServiceDTO?> GetServiceById(int serviceId);

            // Create new service
            Task<ServiceDTO> Create(ServiceCreateDTO cre);

            // Update service
            Task<ServiceDTO?> Update(int serviceId, ServiceUpdateDTO updateDto);

            // Delete service
            Task<bool> Delete(int serviceId);
        Task<bool> ToggleActiveAsync(int serviceId, bool isActive);
        Task<IEnumerable<ServiceDTO>> GetByCategory(Guid? categoryId);
        Task<IEnumerable<topservice>> GetTopservicefour();





    }
}
