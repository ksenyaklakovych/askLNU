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

namespace askLNU.Tests
{
    public class QuestionServiceTest
    {
        private Mock<IUnitOfWork> fakeIUnitOfWork;
        private readonly IMapper _mapper;


        public QuestionServiceTest()
        {
            fakeIUnitOfWork = new Mock<IUnitOfWork>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Question, QuestionDTO>());
            _mapper = new Mapper(config);

        }
        [Fact]
        public void GetAll_ShouldReturnTrue()
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

    }
}
