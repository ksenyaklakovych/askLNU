using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using askLNU.BLL.DTO;
using askLNU.DAL.Entities;
using askLNU.DAL.Interfaces;
using askLNU.BLL.Infrastructure;
using askLNU.BLL.Interfaces;
using AutoMapper;
using askLNU.BLL.Infrastructure.Exceptions;

namespace askLNU.BLL.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AnswerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void CreateAnswer(AnswerDTO answerDto)
        {
            if (answerDto != null)
            {
                Answer answer = _mapper.Map<Answer>(answerDto);
                _unitOfWork.Answers.Create(answer);
                _unitOfWork.Save();
            }
            else
            {
                throw new ArgumentNullException("answerDto");
            }
        }

        public AnswerDTO GetAnswer(int? id)
        {
            if (id != null)
            {
                var answer = _unitOfWork.Answers.Get(id.Value);
                if (answer != null)
                {
                    return _mapper.Map<AnswerDTO>(answer);
                }
                else
                {
                    throw new ItemNotFoundException($"Answer not found.");
                }
            }
            else
            {
                throw new ArgumentNullException("id");
            }
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public IEnumerable<AnswerDTO> GetAnswersByQuestionId(int? id)
        {
            if (id != null)
            {
                var answers = _unitOfWork.Answers.Find(a => a.QuestionId == id);
                var answersDTOs= _mapper.Map<IEnumerable<AnswerDTO>>(answers);
                return answersDTOs;
            }
            else
            {
                throw new ArgumentNullException("id");
            }
        }
    }
}
