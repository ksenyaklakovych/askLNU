using askLNU.BLL.DTO;
using askLNU.DAL.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace askLNU.BLL.Infrastructure.MapperProfiles
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile()
        {
            CreateMap<Answer, AnswerDTO>();
            CreateMap<AnswerDTO, Answer>();
        }
    }
}
