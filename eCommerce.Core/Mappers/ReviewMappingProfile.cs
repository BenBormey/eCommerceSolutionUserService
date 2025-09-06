using AutoMapper;
using eCommerce.Core.DTO.Review;
using eCommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Mappers
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<Review, ReviewDTO>();
            CreateMap<ReviewCreateDTO, Review>()
                .ForMember(d => d.ReviewId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore());
            CreateMap<ReviewUpdateDTO, Review>()
                .ForAllMembers(opt => opt.Condition((src, dest, val) => val != null));
        }
    }
}
