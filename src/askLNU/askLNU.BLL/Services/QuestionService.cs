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
using Microsoft.Extensions.Logging;

namespace askLNU.BLL.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<QuestionService> _logger;


        public QuestionService(
            IUnitOfWork unitOfWork,
            ILogger<QuestionService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
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

        public void Dispose(int questionId)
        {
            var question = _unitOfWork.Questions.Get(questionId);
            var answersToQuestion = _unitOfWork.Answers.Find(a => a.QuestionId == questionId);
            foreach (var answer in answersToQuestion)
            {
                _unitOfWork.Answers.Delete(answer.Id);
            }
            while (true)
            {
                try
                {
                    var favorite = question.ApplicationUserFavoriteQuestions
                                      .Where(fav => fav.QuestionId == questionId).First();
                    question.ApplicationUserFavoriteQuestions.Remove(favorite);
                    _unitOfWork.Questions.Update(question);
                }
                catch (System.InvalidOperationException)
                {
                    break;
                }
                
            }
            _unitOfWork.Questions.Delete(questionId);
            _logger.LogInformation($"Deleted question with id {questionId}");
            _unitOfWork.Save();
        }

        public IEnumerable<QuestionDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<Question>, List<QuestionDTO>>(_unitOfWork.Questions.GetAll());
        }

        public IEnumerable<string> GetTagsByQuestionID(int? id)
        {
            if (id != null)
            {
                var tagsIds = _unitOfWork.QuestionTag.Find(q => q.QuestionId == id.Value).Select(q => q.TagId);
                var tagsTexts = tagsIds.Select(tagId => _unitOfWork.Tags.Get(tagId).Text).ToList();
                return tagsTexts;
            }
            else
            {
                throw new ArgumentNullException("id");
            }
        }

        public void AddToFavorites(string userId, int questionId)
        {
            var userFavoriteQuestion = new ApplicationUserFavoriteQuestion 
            { 
                ApplicationUserId = userId, 
                QuestionId = questionId 
            };
            _unitOfWork.ApplicationUserFavoriteQuestion.Create(userFavoriteQuestion);
            _unitOfWork.Save();
        }
        public void RemoveFromFavorites(string userId, int questionId)
        {
            var question = _unitOfWork.Questions.Get(questionId);
            var favorite = question.ApplicationUserFavoriteQuestions
                .Where(fav => fav.QuestionId == questionId && fav.ApplicationUserId == userId)
                .FirstOrDefault();

            question.ApplicationUserFavoriteQuestions.Remove(favorite);
            _unitOfWork.Questions.Update(question);
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
            var vote = _unitOfWork.QuestionVotes
                .Find(v => v.ApplicationUserId == userId && v.QuestionId == questionId).FirstOrDefault();

            var question = _unitOfWork.Questions.Get(questionId);

            if (vote == null)
            {
                _unitOfWork.QuestionVotes.Create(new QuestionVote
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

                _unitOfWork.QuestionVotes.Update(vote);
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
