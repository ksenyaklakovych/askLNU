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
    public class FacultyServiceTest
    {
        private readonly IMapper _mapper;
        private readonly IMapper _mapper_2;
        private readonly DbContextOptions<ApplicationDbContext> options;
        private Mock<ILogger<FacultyService>> _logger;

        public FacultyServiceTest()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Faculty, FacultyDTO>());
            _mapper = new Mapper(config);

            var config_2 = new MapperConfiguration(cfg => cfg.CreateMap<FacultyDTO, Faculty>());
            _mapper_2 = new Mapper(config_2);

            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FakeDatabaseFaculty")
                .Options;
            _logger = new Mock<ILogger<FacultyService>>();
        }

        [Fact]
        public void GetAll_ShouldReturnTrue()
        {
            //Arrange
            using var context = new ApplicationDbContext(options);

            context.Faculties.Add(new Faculty { Id = 1, Title = "AMI" });
            context.Faculties.Add(new Faculty { Id = 2, Title = "Biology" });

            context.SaveChanges();
            var unitOfWork = new EFUnitOfWork(context);

            FacultyService facultyService = new FacultyService(unitOfWork, _mapper, _logger.Object);
            //Act
            var result = facultyService.GetAll();

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(unitOfWork.Faculties.GetAll().Count(), result.Count());
            Assert.Equal(context.Faculties.First().Title, result.First().Title);
        }

        [Fact]
        public void Dispose_PassFacultyId()
        {
            int facultyId = 3;
            //Arrange
            using var context = new ApplicationDbContext(options);

            context.Faculties.Add(new Faculty { Id = facultyId, Title = "AMI" });

            context.SaveChanges();
            var unitOfWork = new EFUnitOfWork(context);

            FacultyService facultyService = new FacultyService(unitOfWork, _mapper, _logger.Object);
            //Act
            facultyService.Dispose(facultyId);

            //Assert
            var emptyList = new List<Answer>() { };
            Assert.Equal(emptyList, unitOfWork.Answers.Find(e => e.Id == facultyId).ToList());
        }

        [Fact]
        public void GetFacultyIdByName_PassCorrectName()
        {
            var facultyName = "facultyName";
            var facultyId = 4;
            //Arrange
            using var context = new ApplicationDbContext(options);

            context.Faculties.Add(new Faculty { Id = facultyId, Title = facultyName });

            context.SaveChanges();
            var unitOfWork = new EFUnitOfWork(context);

            FacultyService facultyService = new FacultyService(unitOfWork, _mapper, _logger.Object);
            //Act
            int result = facultyService.GetFacultyIdByName(facultyName);

            //Assert
            Assert.Equal(facultyId, result);
        }

        [Fact]
        public void GetFacultyIdByName_PassIncorrectName()
        {
            var facultyName = "facultyName";
            var facultyId = 5;
            //Arrange
            using var context = new ApplicationDbContext(options);

            context.Faculties.Add(new Faculty { Id = facultyId, Title = facultyName });

            context.SaveChanges();
            var unitOfWork = new EFUnitOfWork(context);

            FacultyService facultyService = new FacultyService(unitOfWork, _mapper, _logger.Object);
            //Act
            int result = facultyService.GetFacultyIdByName(facultyName+"a");

            //Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void CreateFaculty_PassCorrectName()
        {
            var facultyDTO = new FacultyDTO {Id=7,Title="Pravo5" } ;
            //Arrange
            using var context = new ApplicationDbContext(options);

            context.SaveChanges();
            var unitOfWork = new EFUnitOfWork(context);

            FacultyService facultyService = new FacultyService(unitOfWork, _mapper_2, _logger.Object);
            //Act
            facultyService.CreateFaculty(facultyDTO);

            //Assert
            Assert.Equal(7, unitOfWork.Faculties.Find(a=>a.Title=="Pravo5").Select(i=>i.Id).First());
        }
    }
}
