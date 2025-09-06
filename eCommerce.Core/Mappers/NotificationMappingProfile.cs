using AutoMapper;
using eCommerce.Core.DTO.Notification;
using eCommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Mappers
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<Notification, NotificationDTO>();
            CreateMap<NotificationCreateDTO, Notification>()
                .ForMember(d => d.NotificationId, opt => opt.Ignore())
                .ForMember(d => d.SentAt, opt => opt.Ignore());
            CreateMap<NotificationUpdateDTO, Notification>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
