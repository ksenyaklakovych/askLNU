using System;
using Xunit;
using askLNU.BLL.Services;
using Moq;
using Microsoft.AspNetCore.Identity;
using askLNU.DAL.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using askLNU.BLL.DTO;
using askLNU.BLL.Interfaces;
using AutoMapper;
using askLNU.DAL.Interfaces;
using System.Linq;
using System.Collections.Generic;
using askLNU.BLL.Infrastructure.Exceptions;

namespace askLNU.Tests
{
    public class UserServiceTest
    {
        private Mock<UserManager<ApplicationUser>> fakeManager;
        private Mock<ILogger<UserService>> _logger;
        private Mock<IUnitOfWork> fakeIUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapper _mapper2;

        public UserServiceTest()
        {
            fakeManager = new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
            _logger = new Mock<ILogger<UserService>>();
            fakeIUnitOfWork = new Mock<IUnitOfWork>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, UserDTO>());
            _mapper = new Mapper(config);
            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, ApplicationUser>());
            _mapper2 = new Mapper(config2);
        }

        [Fact]
        public void GetByEmailAsync_WithCorrectEmail_ShouldReturnTrue()
        {
            string email = "test";
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, UserDTO>());
            var _mapper = new Mapper(config);

            //Arrange
            var fakeUser = Task.Run(() => new ApplicationUser("TestName", "TestSurname", 1, false, "image.jpg"));
            fakeManager.Setup(m => m.FindByEmailAsync(email)).Returns(fakeUser);

            UserService userService = new UserService(fakeManager.Object, _logger.Object, _mapper);
            //Act

            var result = userService.GetByEmailAsync(email);

            //Assert
            Assert.Equal("TestName", result.Result.Name);
            Assert.Equal("TestSurname", result.Result.Surname);
            Assert.False(result.Result.IsBlocked);
        }

        [Fact]
        public void GetByEmailAsync_WithIncorrectEmail_ShouldReturnTrue()
        {
            string email = "test";
            //Arrange
            var fakeUser = Task.Run(() => new ApplicationUser());
            fakeManager.Setup(m => m.FindByEmailAsync(email)).Returns(Task.FromResult((ApplicationUser)null));

            UserService userService = new UserService(fakeManager.Object, _logger.Object);

            //Assert
            Assert.ThrowsAsync<ItemNotFoundException>(() => userService.GetByEmailAsync(email));
        }

        [Fact]
        public void GetUsersByEmail_WithCorrectEmail_ShouldReturnTrue()
        {
            string testEmail = "test@test.com";
            //Arrange
            var fakeUserSDTOlist = new List<UserDTO> { new UserDTO("name", "name", "surname", 1, false, "image", "test@test.com")};
            IEnumerable < UserDTO > usersDTO= fakeUserSDTOlist;
            var fakeUsers = _mapper2.Map<IEnumerable<ApplicationUser>>(usersDTO).AsQueryable();
            fakeManager.Setup(m => m.Users).Returns(fakeUsers);

            UserService userService = new UserService(fakeManager.Object, _logger.Object, _mapper);
            //Act
            var result = userService.GetUsersByEmail(testEmail);

            //Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetUsersByEmail_WithInCorrectEmail_ShouldReturnTrue()
        {
            string testEmail = "test@test.com";
            //Arrange
            var fakeUserSDTOlist = new List<UserDTO> { new UserDTO("name", "name", "surname", 1, false, "image", "test2@test.com") };
            IEnumerable<UserDTO> usersDTO = fakeUserSDTOlist;
            var fakeUsers = _mapper2.Map<IEnumerable<ApplicationUser>>(usersDTO).AsQueryable();
            fakeManager.Setup(m => m.Users).Returns(fakeUsers);

            UserService userService = new UserService(fakeManager.Object, _logger.Object, _mapper);
            //Act
            var result = userService.GetUsersByEmail(testEmail);

            //Assert
            Assert.Empty(result);
        }
        //TO DO:
        //CheckIfUserHasRole
        //RemoveModeratorRole
        //GiveModeratorRole

        //[Fact]
        //public void CreateUserAsync_ShouldReturnTrue()
        //{
        //    //Arrange
        //    string password = "password";
        //    var fakeUserDTO = new UserDTO("testUsername", "TestName", "TestSurname", 1, false, "image.jpg", "test@gmail.com");
        //    var fakeAppUser = new ApplicationUser()
        //    {
        //        Name = fakeUserDTO.Name,
        //        Surname = fakeUserDTO.Surname,
        //        Course = fakeUserDTO.Course,
        //        FacultyId = fakeUserDTO.FacultyId,
        //        ImageSrc = fakeUserDTO.ImageSrc,
        //        UserName = fakeUserDTO.UserName,
        //        Email = fakeUserDTO.Email
        //    };
        //    fakeManager.Setup(m => m.CreateAsync(fakeAppUser, password)).ReturnsAsync(IdentityResult.Success);
        //    UserService userService = new UserService(fakeManager.Object);

        //    //Act
        //    var result = userService.CreateUserAsync(fakeUserDTO, password);

        //    //Assert
        //    Console.WriteLine(result.Result);
        //}



    }
}
