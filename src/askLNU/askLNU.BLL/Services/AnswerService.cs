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
    public class AnswerService : IAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AnswerService> _logger;

        public AnswerService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<AnswerService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

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
                    var message = $"Answer with id {id} not found.";
                    _logger.LogWarning(message);
                    throw new ItemNotFoundException(message);
                }
            }
            else
            {
                throw new ArgumentNullException("id");
            }
        }

        public void Dispose(int answerId)
        {
            _unitOfWork.Answers.Delete(answerId);
            _logger.LogInformation($"Deleted answer with id {answerId}");
            _unitOfWork.Save();
        }

        public IEnumerable<AnswerDTO> GetAnswersByQuestionId(int? id)
        {
            if (id != null)
            {
                var answers = _unitOfWork.Answers.Find(a => a.QuestionId == id);
                var answersDTOs = _mapper.Map<IEnumerable<AnswerDTO>>(answers);
                return answersDTOs;
            }
            else
            {
                throw new ArgumentNullException("id");
            }
        }

        private int Vote(string userId, int answerId, bool voteUp = false, bool voteDown = false)
        {
            var vote = _unitOfWork.AnswerVotes
                .Find(v => v.ApplicationUserId == userId && v.AnswerId == answerId).FirstOrDefault();

            var answer = _unitOfWork.Answers.Get(answerId);

            if (vote == null)
            {
                _unitOfWork.AnswerVotes.Create(new AnswerVote
                {
                    ApplicationUserId = userId,
                    AnswerId = answerId,
                    VotedUp = voteUp,
                    VotedDown = voteDown
                });
                _unitOfWork.Save();

                if (voteUp)
                {
                    answer.Rating++;
                }
                else if (voteDown)
                {
                    answer.Rating--;
                }
            }
            else
            {
                if (!vote.VotedUp && !vote.VotedDown)
                {
                    if (voteUp)
                    {
                        answer.Rating++;
                        vote.VotedUp = true;
                    }
                    else if (voteDown)
                    {
                        answer.Rating--;
                        vote.VotedDown = true;
                    }
                }
                else if (voteDown && vote.VotedUp)
                {
                    answer.Rating--;
                    vote.VotedUp = false;
                }
                else if (voteUp && vote.VotedDown)
                {
                    answer.Rating++;
                    vote.VotedDown = false;
                }

                _unitOfWork.AnswerVotes.Update(vote);
            }

            _unitOfWork.Answers.Update(answer);
            _unitOfWork.Save();

            return answer.Rating;
        }

        public int VoteUp(string userId, int answerId)
        {
            return Vote(userId, answerId, voteUp: true);
        }

        public int VoteDown(string userId, int answerId)
        {
            return Vote(userId, answerId, voteDown: true);
        }
    }
}
