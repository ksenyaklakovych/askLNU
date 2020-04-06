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


        public IEnumerable<FacultyDTO> GetAllFaculties()
        {
            return _mapper.Map<IEnumerable<Faculty>, List<FacultyDTO>>(_unitOfWork.Faculties.GetAll());

        }

        public IEnumerable<string> GetTagsByQuestionID(int? id)
        {
            if (id != null)
            {
                var tagsIds = _unitOfWork.QuestionTag.GetAll().Where(q => q.QuestionId == id.Value).Select(q => q.TagId).ToList();
                var tagsTexts = _unitOfWork.Tags.GetAll().Where(t => tagsIds.Contains(t.Id)).Select(t => t.Text);
                return tagsTexts;
            }
            else
            {
                throw new ArgumentNullException("id");
            }
        }

        public void AddToFavorites(string userId, int questionId)
        {
            ApplicationUserFavoriteQuestion userFavoriteQuestion = new ApplicationUserFavoriteQuestion { ApplicationUserId = userId, QuestionId = questionId };
            _unitOfWork.ApplicationUserFavoriteQuestion.Create(userFavoriteQuestion);
            _unitOfWork.Save();
        }
        public void RemoveFromFavorites(string userId, int questionId)
        {
            _unitOfWork.ApplicationUserFavoriteQuestion.Remove(userId,questionId);
            _unitOfWork.Save();
        }

        public bool IsQuestionFavorite(string userId, int questionId)
        {
            bool result = _unitOfWork.ApplicationUserFavoriteQuestion.GetAll().Any(q => q.ApplicationUserId == userId && q.QuestionId == questionId);
            return result;
        }
        public void AddTag(int questionId, int tagId)
        {
            var question = _unitOfWork.Questions.Get(questionId);
            question.QuestionTags.Add(new QuestionTag { QuestionId = questionId, TagId = tagId });
            _unitOfWork.Save();
        }

        private int Vote(string userId, int questionId, bool voteUp = false, bool voteDown = false)
        {
            var vote = _unitOfWork.ApplicationUserVotedQuestions
                .Find(v => v.ApplicationUserId == userId && v.QuestionId == questionId).FirstOrDefault();

            var question = _unitOfWork.Questions.Get(questionId);

            if (vote == null)
            {
                _unitOfWork.ApplicationUserVotedQuestions.Create(new ApplicationUserVotedQuestion
                {
                    ApplicationUserId = userId,
                    QuestionId = questionId,
                    VotedUp = voteUp,
                    VotedDown = voteDown
                });
                _unitOfWork.Save();

                if (voteUp)
                {
                    question.Rating++;
                }
                else if (voteDown)
                {
                    question.Rating--;
                }
            }
            else
            {
                if (!vote.VotedUp && !vote.VotedDown)
                {
                    if (voteUp)
                    {
                        question.Rating++;
                        vote.VotedUp = true;
                    }
                    else if (voteDown)
                    {
                        question.Rating--;
                        vote.VotedDown = true;
                    }
                }
                else if (voteDown && vote.VotedUp)
                {
                    question.Rating--;
                    vote.VotedUp = false;
                }
                else if (voteUp && vote.VotedDown)
                {
                    question.Rating++;
                    vote.VotedDown = false;
                }

                _unitOfWork.ApplicationUserVotedQuestions.Update(vote);
            }

            _unitOfWork.Questions.Update(question);
            _unitOfWork.Save();

            return question.Rating;
        }

        public int VoteUp(string userId, int questionId)
        {
            return Vote(userId, questionId, voteUp: true);
        }

        public int VoteDown(string userId, int questionId)
        {
            return Vote(userId, questionId, voteDown: true);
        }

        public void AddAnswer(int questionId, AnswerDTO answer)
        {
            var question = _unitOfWork.Questions.Get(questionId);
            question.Answers.Add(_mapper.Map<Answer>(answer));
            _unitOfWork.Questions.Update(question);
            _unitOfWork.Save();
        }
    }
}
