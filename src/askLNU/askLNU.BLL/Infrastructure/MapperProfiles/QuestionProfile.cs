using askLNU.BLL.DTO;
using askLNU.DAL.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace askLNU.BLL.Infrastructure.MapperProfiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionDTO>()
                .AfterMap((q, qDto) =>
                {
                    if (q.QuestionTags != null)
                    {
                        qDto.TagsId = q.QuestionTags.Select(qt => qt.TagId).ToList();
                    }
                });
            CreateMap<QuestionDTO, Question>();
        }
    }
}
