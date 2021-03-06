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
    public class QuestionServiceTests
    {
        private Mock<IUnitOfWork> fakeIUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapper _mapperToQuestion;
        private readonly DbContextOptions<ApplicationDbContext> options;
        private Mock<ILogger<QuestionService>> _logger;

        public QuestionServiceTests()
        {
            fakeIUnitOfWork = new Mock<IUnitOfWork>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Question, QuestionDTO>());
            _mapper = new Mapper(config);
            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<QuestionDTO, Question>());
            _mapperToQuestion = new Mapper(config2);
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FakeDatabase")
                .Options;
            _logger = new Mock<ILogger<QuestionService>>();
        }

        [Fact]
        public void GetAllQuestions_ShouldReturnTrue()
        {
            //Arrange
            DateTime date1 = DateTime.Parse("5/1/2008 8:30:52 AM",
                          System.Globalization.CultureInfo.InvariantCulture);
            var fakeQuestionsList = new List<Question> { new Question("1", "question1", "text1",date1) ,
                            new Question("1", "question1", "text1", date1) ,
                            new Question("1", "question1", "text1", date1)  };
            IEnumerable<Question> questionsIEnum = fakeQuestionsList;

            fakeIUnitOfWork.Setup(m => m.Questions.GetAll()).Returns(questionsIEnum);

            QuestionService questionService = new QuestionService(fakeIUnitOfWork.Object,_mapper);
            //Act
            var result = questionService.GetAll();

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count());
            Assert.Equal(fakeQuestionsList.First().Text, result.First().Text);
        }

        [Fact]
        public void GetTagsByQuestionID_passNullId_ShouldReturnTrue()
        {
            int? id = 0;
            if (id == 0)
            {
                id = null;
            }
            //Arrange
            fakeIUnitOfWork.Setup(m => m.QuestionTag.GetAll()).Returns((IEnumerable<QuestionTag>)null);

            //Act
            QuestionService questionService = new QuestionService(fakeIUnitOfWork.Object, _mapper);

            //Assert
            Action act = () => questionService.GetTagsByQuestionID(id);
            var exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'id')", exception.Message);
        }

        [Fact]
        public void GetTagsByQuestionID_passCorrectId_ShouldReturnTrue()
        {
            using var context = new ApplicationDbContext(options);

            context.QuestionTag.Add(new QuestionTag(1, 1));
            context.QuestionTag.Add(new QuestionTag(1, 2));

            context.Tags.Add(new Tag(1, "tag1"));
            context.Tags.Add(new Tag(2, "tag2"));

            context.SaveChanges();

            var unitOfWork = new EFUnitOfWork(context);
            var fakeQuestionService = new QuestionService(unitOfWork, _mapper);

            var id = 1;
            var result = fakeQuestionService.GetTagsByQuestionID(id);
            Assert.Equal(2, result.Count());
        }
        
        [Fact]
        public void IsQuestionFavorite_ShoulReturnFalse()
        {
            string userId = "1";
            int questionId = 1;
            IEnumerable<ApplicationUserFavoriteQuestion> userFavoriteQuestions = new List<ApplicationUserFavoriteQuestion> { new ApplicationUserFavoriteQuestion("1",2) };

            //Arrange
            fakeIUnitOfWork.Setup(m => m.ApplicationUserFavoriteQuestion.GetAll()).Returns(userFavoriteQuestions);

            //Act
            QuestionService questionService = new QuestionService(fakeIUnitOfWork.Object, _mapper);

            //Assert
            var result = questionService.IsQuestionFavorite(userId, questionId);
            Assert.False(result);
        }

        [Fact]
        public void RemoveFromFavorites_ShouldReturnEmpty()
        {
            using var context = new ApplicationDbContext(options);

            string userId = "0000";
            int questionId = 5;

            context.Questions.Add(new Question 
            {
                Id = questionId
            });
            context.ApplicationUserFavoriteQuestion.Add(new ApplicationUserFavoriteQuestion(userId, questionId));

            context.SaveChanges();

            var unitOfWork = new EFUnitOfWork(context);
            var fakeQuestionService = new QuestionService(unitOfWork, _mapper);

            Assert.NotEmpty(unitOfWork.Questions.Get(questionId).ApplicationUserFavoriteQuestions);
            fakeQuestionService.RemoveFromFavorites(userId, questionId);
            var question = fakeQuestionService.GetQuestion(questionId);
            Assert.Empty(unitOfWork.Questions.Get(questionId).ApplicationUserFavoriteQuestions);
        }

        [Fact]
        public void AddToFavorites_ShouldReturnSingle()
        {
            string userId = "0000";
            int questionId = 153;

            using var context = new ApplicationDbContext(options);

            context.Questions.Add(new Question
            {
                Id = questionId
            });
            context.SaveChanges();
            var unitOfWork = new EFUnitOfWork(context);

            var fakeQuestionService = new QuestionService(unitOfWork, _logger.Object, _mapper);
            fakeQuestionService.AddToFavorites(userId, questionId);

            Assert.Single(unitOfWork.ApplicationUserFavoriteQuestion.Find(e=>e.QuestionId==questionId && e.ApplicationUserId==userId).ToList());
        }

        [Fact]
        public void CreateQuestion_ShouldReturnTrue()
        {
            var questionDTO = new QuestionDTO { Id=6, Text = "someText", Title = "someTitle" };
            using var context = new ApplicationDbContext(options);
            var unitOfWork = new EFUnitOfWork(context);
            var fakeQuestionService = new QuestionService(unitOfWork,_logger.Object,_mapperToQuestion);

            fakeQuestionService.CreateQuestion(questionDTO);

            Assert.NotNull(unitOfWork.Questions.Find(e=>e.Id==6).First());
        }

        [Fact]
        public void VoteUp_ShouldReturnOne()
        {
            var userId = "userId";
            var questionId = 166;

            using var context = new ApplicationDbContext(options);

            context.QuestionVotes.Add(new QuestionVote { ApplicationUserId=userId, QuestionId=questionId});
            context.Questions.Add(new Question{Id=questionId, Text="question1",Rating=0});

            context.SaveChanges();

            var unitOfWork = new EFUnitOfWork(context);

            var fakeQuestionService = new QuestionService(unitOfWork, _logger.Object, _mapperToQuestion);

            int result = fakeQuestionService.VoteUp(userId, questionId);

            Assert.Equal(1,result);
        }

        [Fact]
        public void VoteDown_ShouldReturnZero()
        {
            var userId = "userId";
            var questionId = 60;

            using var context = new ApplicationDbContext(options);

            context.QuestionVotes.Add(new QuestionVote { ApplicationUserId = userId, QuestionId = questionId });
            context.Questions.Add(new Question { Id = questionId, Text = "question1", Rating = 1 });

            context.SaveChanges();

            var unitOfWork = new EFUnitOfWork(context);

            var fakeQuestionService = new QuestionService(unitOfWork, _logger.Object, _mapperToQuestion);

            int result = fakeQuestionService.VoteDown(userId, questionId);

            Assert.Equal(0, result);
        }

        [Fact]
        public void DisposeQuestion_ShouldReturnTrue()
        {
            int questionId = 80;

            //Arrange
            using var context = new ApplicationDbContext(options);
            
            context.Questions.Add(new Question { Id = questionId, Text = "Question1" });

            context.Answers.Add(new Answer { Id = 216, Text = "Answer3" , QuestionId=70});
            context.Answers.Add(new Answer { Id = 217, Text = "Answer4" , QuestionId=70});

            context.SaveChanges();

            var unitOfWork = new EFUnitOfWork(context);

            //Act
            QuestionService questionService = new QuestionService(unitOfWork, _logger.Object);
            questionService.Dispose(questionId);

            //Assert
            var emptyList = new List<Question>() { };
            Assert.Equal(emptyList, unitOfWork.Questions.Find(e => e.Id == questionId).ToList());
        }

    }
}
