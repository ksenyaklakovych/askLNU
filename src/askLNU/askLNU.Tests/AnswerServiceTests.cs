using System;
using Xunit;
using askLNU.BLL.Services;
using Moq;
using Microsoft.AspNetCore.Identity;
using askLNU.DAL.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using askLNU.BLL.DTO;
using askLNU.BLL.Interfaces;
using askLNU.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using askLNU.DAL.EF;

namespace askLNU.Tests
{
    public class AnswerServiceTests
    {
        private readonly IMapper _mapper;
        private readonly IMapper _mapperFromDTO;
        private readonly DbContextOptions<ApplicationDbContext> options;
        private Mock<ILogger<AnswerService>> _logger;

        public AnswerServiceTests()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Answer, AnswerDTO>());
            _mapper = new Mapper(config);

            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<AnswerDTO, Answer>());
            _mapperFromDTO = new Mapper(config2);
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FakeDatabase")
                .Options;
            _logger = new Mock<ILogger<AnswerService>>();
        }

        [Fact]
        public void GetAnswersByQuestionId_ShouldReturnTrue()
        {
            int questionId = 44;

            //Arrange
            using var context = new ApplicationDbContext(options);

            context.Questions.Add(new Question { Id = questionId });

            context.Answers.Add(new Answer { QuestionId = questionId, Text = "Answer1" });
            context.Answers.Add(new Answer { QuestionId = questionId, Text = "Answer2" });
            context.Answers.Add(new Answer { QuestionId = questionId, Text = "Answer3" });
            context.Answers.Add(new Answer { QuestionId = questionId+1, Text = "Answer4" });

            context.SaveChanges();

            var unitOfWork = new EFUnitOfWork(context);

            //Act
            AnswerService answerService = new AnswerService(unitOfWork, _mapper, _logger.Object);
            var result = answerService.GetAnswersByQuestionId(questionId);

            //Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetAnswersByQuestionId_passNullId_ShouldReturnTrue()
        {
            //Arrange
            int? id = 0;
            if (id == 0)
            {
                id = null;
            }

            using var context = new ApplicationDbContext(options);

            context.Questions.Add(new Question { Id = 100 });
            context.Answers.Add(new Answer { QuestionId = 1, Text = "Answer1" });

            context.SaveChanges();

            var unitOfWork = new EFUnitOfWork(context);
            //Act
            AnswerService answerService = new AnswerService(unitOfWork, _mapper, _logger.Object);

            //Assert
            Action act = () => answerService.GetAnswersByQuestionId(id);
            var exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'id')", exception.Message);
        }

        [Fact]
        public void DisposeAnswer_ShouldReturnTrue()
        {
            int answerId = 10;

            //Arrange
            using var context = new ApplicationDbContext(options);

            context.Answers.Add(new Answer { Id = 10, Text = "Answer1" });
            context.Answers.Add(new Answer { Id = 20, Text = "Answer2" });
            context.Answers.Add(new Answer { Id = 30, Text = "Answer3" });
            context.Answers.Add(new Answer { Id = 40, Text = "Answer4" });

            context.SaveChanges();

            var unitOfWork = new EFUnitOfWork(context);

            //Act
            AnswerService answerService = new AnswerService(unitOfWork, _mapper, _logger.Object);
            answerService.Dispose(answerId);

            //Assert
            var emptyList = new List<Answer>() { };
            Assert.Equal(emptyList, unitOfWork.Answers.Find(e => e.Id == answerId).ToList());
        }

        [Fact]
        public void CreateAnswer_ShouldReturnTrue()
        {
            int answerId = 124;
            var answerDTO = new AnswerDTO { Id = answerId, Text = "answer1" };

            //Arrange
            using var context = new ApplicationDbContext(options);
            var unitOfWork = new EFUnitOfWork(context);

            //Act
            AnswerService answerService = new AnswerService(unitOfWork, _mapperFromDTO);
            answerService.CreateAnswer(answerDTO);

            //Assert
            Assert.Equal(answerDTO.Text,unitOfWork.Answers.Find(e => e.Id == answerId).FirstOrDefault().Text);
        }

        [Fact]
        public void VoteUp_ShouldReturnOne()
        {
            var userId = "userId";
            var answerId = 100;

            using var context = new ApplicationDbContext(options);

            context.AnswerVotes.Add(new AnswerVote { ApplicationUserId = userId, AnswerId = answerId });
            context.Answers.Add(new Answer { Id = answerId, Text = "answer1", Rating = 0 });

            context.SaveChanges();

            var unitOfWork = new EFUnitOfWork(context);

            var fakeAnswerService = new AnswerService(unitOfWork, null, null);

            int result = fakeAnswerService.VoteUp(userId, answerId);

            Assert.Equal(1, result);
        }

        [Fact]
        public void VoteDown_ShouldReturnZero()
        {
            var userId = "userId";
            var answerId = 166;

            using var context = new ApplicationDbContext(options);

            //context.AnswerVotes.Add(new AnswerVote { ApplicationUserId = userId, AnswerId = answerId });
            context.Answers.Add(new Answer { Id = answerId, Text = "answer1", Rating = 1 });

            context.SaveChanges();

            var unitOfWork = new EFUnitOfWork(context);

            var fakeAnswerService = new AnswerService(unitOfWork, null, null);

            int result = fakeAnswerService.VoteDown(userId, answerId);

            Assert.Equal(0, result);
        }
    }
}
