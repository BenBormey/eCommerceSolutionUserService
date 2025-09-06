using AutoMapper;
using eCommerce.Core.DTO.Service;
using eCommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Mappers
{
 
        public class ServiceMappingProfile : Profile
        {
            public ServiceMappingProfile()
            {
                // Entity -> DTO
                CreateMap<Services, ServiceDTO>();

                // Create DTO -> Entity
                CreateMap<ServiceCreateDTO, Services>();

                // Update DTO -> Entity
                CreateMap<ServiceUpdateDTO, Services>();
            }
        }
}
