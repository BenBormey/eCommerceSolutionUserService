using AutoMapper;
using eCommerce.Core.DTO.Category;
using eCommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Mappers
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            // Entity -> DTOs (read)
            CreateMap<Category, CategoryDTO>();


            // Create DTO -> Entity
            CreateMap<CategoryCreateDTO, Category>()
                // If DB generates defaults, you can leave these out
                .ForMember(d => d.CategoryId, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(d => d.CreateAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(d => d.UpdateAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Update DTO -> Entity
            CreateMap<CategoryUpdateDTO, Category>()
                // keep CreatedAt unchanged when updating
                .ForMember(d => d.CreateAt, opt => opt.Ignore())
                .ForMember(d => d.UpdateAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
