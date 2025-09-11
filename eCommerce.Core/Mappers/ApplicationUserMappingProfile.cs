using AutoMapper;
using eCommerce.Core.DTO;
using eCommerce.Core.Entities;

namespace eCommerce.Core.Mappers;

public class ApplicationUserMappingProfile :Profile
{
    public ApplicationUserMappingProfile()
    {
    
        CreateMap<ApplicationUser, UserSummaryDTO>();

        CreateMap<ApplicationUser, AuthenticationResponse>()
            .ForMember(d => d.Fullname, m => m.MapFrom(s => s.FullName ?? ""))
            .ForMember(d => d.Email, m => m.MapFrom(s => s.Email ?? ""))
            .ForMember(d => d.Role, m => m.MapFrom(s => s.Role ?? "Customer"))
            .ForMember(d => d.Token, m => m.Ignore())
            .ForMember(d => d.Success, m => m.MapFrom(_ => true));
           
    }
}

