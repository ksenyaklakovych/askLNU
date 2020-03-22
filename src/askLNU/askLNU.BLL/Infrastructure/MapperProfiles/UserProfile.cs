using askLNU.BLL.DTO;
using askLNU.DAL.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace askLNU.BLL.Infrastructure.MapperProfiles
{
    class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<UserDTO, ApplicationUser>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
