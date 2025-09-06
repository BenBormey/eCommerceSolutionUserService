using AutoMapper;
using eCommerce.Core.DTO.Location;
using eCommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Mappers
{
    public class LocationMappingProfile : Profile
    {
        public LocationMappingProfile()
        {
            CreateMap<Location, LocationDTO>();
            CreateMap<LocationCreateDTO, Location>();
            CreateMap<LocationUpdateDTO, Location>();
        }
    }
}
