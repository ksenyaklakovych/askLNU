using askLNU.BLL.DTO;
using askLNU.DAL.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace askLNU.BLL.Infrastructure.MapperProfiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionDTO>();
            CreateMap<QuestionDTO, Question>();
        }
    }
}
