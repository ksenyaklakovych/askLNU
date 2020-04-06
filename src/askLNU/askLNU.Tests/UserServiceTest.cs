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

namespace askLNU.Tests
{
    public class UserServiceTest
    {
        private Mock<UserManager<ApplicationUser>> fakeManager;
        private Mock<ILogger<UserService>> _logger;

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
        }
   
        [Fact]
        public void GetByEmailAsync_WithCorrectEmail_ShouldReturnTrue()
        {
            string email = "test";
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, UserDTO>());
            var _mapper = new Mapper(config);

            //Arrange
            var fakeUser = Task.Run(() => new ApplicationUser("TestName","TestSurname",1,false,"image.jpg"));
            fakeManager.Setup(m => m.FindByEmailAsync(email)).Returns(fakeUser);
            
            UserService userService = new UserService(fakeManager.Object,_mapper);
            //Act
            var result = userService.GetByEmailAsync(email);

            //Assert
            Assert.Equal("TestName",result.Result.Name);
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

            UserService userService = new UserService(fakeManager.Object);
            //Act
            var result = userService.GetByEmailAsync(email);

            //Assert
            Assert.Null(result.Result);
        }

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
