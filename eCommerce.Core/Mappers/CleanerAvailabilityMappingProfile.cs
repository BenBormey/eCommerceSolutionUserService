using AutoMapper;
using eCommerce.Core.DTO.CleanerAvailability;
using eCommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Mappers
{
    public class CleanerAvailabilityMappingProfile : Profile
    {
        public CleanerAvailabilityMappingProfile()
        {
            CreateMap<CleanerAvailability, CleanerAvailabilityDTO>();
            CreateMap<CleanerAvailabilityCreateDTO, CleanerAvailability>();
            CreateMap<CleanerAvailabilityUpdateDTO, CleanerAvailability>();
        }
    }
}
