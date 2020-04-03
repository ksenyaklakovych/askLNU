using System;
using System.Collections.Generic;
using System.Text;
using askLNU.BLL.DTO;
using askLNU.DAL.Entities;
using askLNU.DAL.Interfaces;
using askLNU.BLL.Infrastructure;
using askLNU.BLL.Interfaces;
using AutoMapper;
using askLNU.BLL.Infrastructure.Exceptions;

namespace askLNU.BLL.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void CreateQuestion(QuestionDTO questionDTO)
        {
            if (questionDTO != null)
            {
                Question question = _mapper.Map<Question>(questionDTO);
                _unitOfWork.Questions.Create(question);
                _unitOfWork.Save();
                questionDTO.Id = question.Id;
            }
            else
            {
                throw new ArgumentNullException("questionDTO");
            }
        }

        public QuestionDTO GetQuestion(int? id)
        {
            if (id != null)
            {
                var question = _unitOfWork.Questions.Get(id.Value);
                if (question != null)
                {
                    return _mapper.Map<QuestionDTO>(question);
                }
                else
                {
                    throw new ItemNotFoundException("Question not found.");
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

        public IEnumerable<QuestionDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<Question>, List<QuestionDTO>>(_unitOfWork.Questions.GetAll());
        }
        public int GetIdByFacutyName(string name)
        {
            var allFaculties=_unitOfWork.Faculties.GetAll();
            foreach (var item in allFaculties)
            {
                if (name == item.Title)
                {
                    return item.Id;
                }
            }
            return -1;
        }

        public IEnumerable<FacultyDTO> GetAllFaculties()
        {
            return _mapper.Map<IEnumerable<Faculty>, List<FacultyDTO>>(_unitOfWork.Faculties.GetAll());

        }

        public void AddTag(int questionId, int tagId)
        {
            var question = _unitOfWork.Questions.Get(questionId);
            question.QuestionTags.Add(new QuestionTag { QuestionId = questionId, TagId = tagId });
            _unitOfWork.Save();
        }
    }
}
