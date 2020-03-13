using System;
using System.Collections.Generic;
using System.Text;
using askLNU.BLL.DTO;
using askLNU.DAL.Entities;
using askLNU.DAL.Interfaces;
using askLNU.BLL.Infrastructure;
using askLNU.BLL.Interfaces;
using AutoMapper;

namespace askLNU.BLL.Services
{
    public class AnswerService : IAnswerService
    {
        IUnitOfWork Database { get; set; }

        public AnswerService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public void CreateAnswer(AnswerDTO answerDto)
        {
            Answer answer = Database.Answers.Get(answerDto.Id);

            if (answer == null)
                throw new ValidationException("Answer not found", "");
            Answer a = new Answer
            {
                Id=answerDto.Id,
                ApplicationUserId = answerDto.ApplicationUserId,
                QuestionId = answerDto.QuestionId,
                Text = answerDto.Text,
                Rating = answerDto.Rating,
                IsSolution = answerDto.IsSolution,
                Date = answerDto.Date,
            };
            Database.Answers.Create(a);
            Database.Save();
        }

        public IEnumerable<AnswerDTO> GetAll()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Answer, AnswerDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Answer>, List<AnswerDTO>>(Database.Answers.GetAll());
        }

        public AnswerDTO GetAnswer(int? id)
        {
            if (id == null)
                throw new ValidationException("Id not set", "");
            var answer = Database.Answers.Get(id.Value);
            if (answer == null)
                throw new ValidationException("Answer not found", "");

            return new AnswerDTO {
                Id = answer.Id,
                ApplicationUserId = answer.ApplicationUserId,
                QuestionId = answer.QuestionId,
                Text = answer.Text,
                Rating = answer.Rating,
                IsSolution = answer.IsSolution,
                Date = answer.Date,
            };
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
