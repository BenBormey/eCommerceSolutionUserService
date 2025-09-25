using AutoMapper;
using eCommerce.Core.DTO.Category;
using eCommerce.Core.DTO.Service;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eCommerce.Core.Service
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceDTO>> GetAllAsync()
        {
            return await _serviceRepository.GetService();
        }

        public async Task<ServiceDTO?> GetByIdAsync(int serviceId)
        {
            return await _serviceRepository.GetServiceById(serviceId);
        }

        public async Task<ServiceDTO> CreateAsync(ServiceCreateDTO dto)
        {
            var created = await _serviceRepository.Create(dto);
            return created;
        }

        public async Task<ServiceDTO?> UpdateAsync(int serviceId, ServiceUpdateDTO dto)
        {
            return await _serviceRepository.Update(serviceId, dto);
        }

        public async Task<bool> DeleteAsync(int serviceId)
        {
            return await _serviceRepository.Delete(serviceId);
        }

        public async Task<bool> ExistsAsync(int serviceId)
        {
            var service = await _serviceRepository.GetServiceById(serviceId);
            return service != null;
        }

        public async Task<bool> ToggleActiveAsync(int serviceId, bool isActive)
        {
            // optional: check exists before toggling
            var exists = await _serviceRepository.GetServiceById(serviceId);
            if (exists == null) return false;

            return await _serviceRepository.ToggleActiveAsync(serviceId, isActive);
        }

        public async Task<IEnumerable<ServiceDTO>> GetByCategory(Guid? categoryId)
        {
           if(categoryId == null)
            {
                return null;
            }
           return await _serviceRepository.GetByCategory(categoryId);
        }

        public async Task<IEnumerable<topservice>> GetTopservicefour()
        {
             var result  = await _serviceRepository.GetTopservicefour();
            return result;
        }
    }
}
