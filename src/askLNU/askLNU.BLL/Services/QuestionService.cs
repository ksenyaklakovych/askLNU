﻿using System;
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
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void CreateQuestion(QuestionDTO answerDto)
        {
            if (answerDto != null)
            {
                Question answer = _mapper.Map<Question>(answerDto);
                _unitOfWork.Questions.Create(answer);
                _unitOfWork.Save();
            }
            else
            {
                throw new ArgumentNullException("answerDto");
            }
        }

        public QuestionDTO GetQuestion(int? id)
        {
            if (id != null)
            {
                var answer = _unitOfWork.Questions.Get(id.Value);
                if (answer != null)
                {
                    return _mapper.Map<QuestionDTO>(answer);
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

        public IEnumerable<QuestionDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<Question>, List<QuestionDTO>>(_unitOfWork.Questions.GetAll());
        }
        public int GetFacultyIdByName(string name)
        {
            var allFaculties=_unitOfWork.Faculties.GetAll().Where(f=>f.Title==name).Select(f=>f.Id);
            return -1;
        }

        public IEnumerable<FacultyDTO> GetAllFaculties()
        {
            return _mapper.Map<IEnumerable<Faculty>, List<FacultyDTO>>(_unitOfWork.Faculties.GetAll());

        }

        public IEnumerable<string> GetTagsByQuestionID(int? id)
        {
            if (id != null)
            {
                var tagsIds = _unitOfWork.QuestionTag.GetAll().Where(q=>q.QuestionId==id.Value).Select(q=>q.TagId).ToList();
                var tagsTexts = _unitOfWork.Tags.GetAll().Where(t => tagsIds.Contains(t.Id)).Select(t => t.Text);
                return tagsTexts;
            }
            else
            {
                throw new ArgumentNullException("id");
            }
        }
    }
}
