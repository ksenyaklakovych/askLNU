using askLNU.BLL.DTO;
using askLNU.DAL.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace askLNU.BLL.Infrastructure.MapperProfiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDTO>();
            CreateMap<TagDTO, Tag>();
        }
    }
}
