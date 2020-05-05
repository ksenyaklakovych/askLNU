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
    public class TagServiceTest
    {
        private readonly IMapper _mapper;
        private readonly IMapper _mapper_2;
        private readonly DbContextOptions<ApplicationDbContext> options;
        private Mock<ILogger<TagService>> _logger;

        public TagServiceTest()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Tag, TagDTO>());
            _mapper = new Mapper(config);

            var config_2 = new MapperConfiguration(cfg => cfg.CreateMap<TagDTO, Tag>());
            _mapper_2 = new Mapper(config_2);

            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FakeDatabaseFaculty")
                .Options;
            _logger = new Mock<ILogger<TagService>>();
        }

        [Fact]
        public void DisposeTag_PassTagId()
        {
            int tagId = 3;
            //Arrange
            using var context = new ApplicationDbContext(options);

            context.Tags.Add(new Tag { Id = tagId, Text = "tag1" });

            context.SaveChanges();
            var unitOfWork = new EFUnitOfWork(context);

            TagService tagService = new TagService(unitOfWork, _mapper, _logger.Object);
            //Act
            tagService.Dispose(tagId);

            //Assert
            var emptyList = new List<Tag>() { };
            Assert.Equal(emptyList, unitOfWork.Tags.Find(e => e.Id == tagId).ToList());
        }

        [Fact]
        public void CreateTag_PassTagDTO()
        {
            var tagDTO = new TagDTO {Id=1, Text="tag1" } ;
            //Arrange
            using var context = new ApplicationDbContext(options);
            context.SaveChanges();
            var unitOfWork = new EFUnitOfWork(context);

            TagService tagService = new TagService(unitOfWork, _mapper_2, _logger.Object);
            //Act
            tagService.CreateTag(tagDTO);

            //Assert
            Assert.Equal(1, unitOfWork.Tags.Find(a=>a.Text== "tag1").Select(i=>i.Id).First());
        }
    }
}
