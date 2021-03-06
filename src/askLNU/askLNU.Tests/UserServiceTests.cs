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
    public class UserServiceTests
    {
        private Mock<UserManager<ApplicationUser>> fakeManager;
        private Mock<ILogger<UserService>> _logger;
        private Mock<IUnitOfWork> fakeIUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapper _mapper2;

        public UserServiceTests()
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

        [Fact]
        public void CheckIfUserHasRole_ShouldReturnTrue()
        {
            string userId = "testId";
            string userRole = "Admin";

            //Arrange
            Task<ApplicationUser> fakeUser = Task.Run(() => new ApplicationUser("TestName", "TestSurname", 1, false, "image.jpg"));
            Task<bool> trueTask = Task.FromResult(true);
            fakeManager.Setup(m => m.FindByIdAsync(userId)).Returns(fakeUser);
            fakeManager.Setup(m => m.IsInRoleAsync(fakeUser.Result, userRole)).Returns(trueTask);

            UserService userService = new UserService(fakeManager.Object, _logger.Object, _mapper);
            //Act
            var result = userService.CheckIfUserHasRole(userId,userRole);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckIfUserHasRole_ShouldReturnFalse()
        {
            string userId = "testId";
            string userRole = "Admin";

            //Arrange
            Task<ApplicationUser> fakeUser = Task.Run(() => new ApplicationUser("TestName", "TestSurname", 1, false, "image.jpg"));
            Task<bool> falseTask = Task.FromResult(false);
            fakeManager.Setup(m => m.FindByIdAsync(userId)).Returns(fakeUser);
            fakeManager.Setup(m => m.IsInRoleAsync(fakeUser.Result, userRole)).Returns(falseTask);

            UserService userService = new UserService(fakeManager.Object, _logger.Object, _mapper);
            //Act
            var result = userService.CheckIfUserHasRole(userId, userRole);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void GiveModeratorRole_ShouldReturnSuccess()
        {
            string userId = "testId";
            string userRole = "Moderator";

            //Arrange
            Task<ApplicationUser> fakeUser = Task.Run(() => new ApplicationUser("TestName", "TestSurname", 1, false, "image.jpg"));
            Task<IdentityResult> trueTask = Task.FromResult(IdentityResult.Success);
            fakeManager.Setup(m => m.FindByIdAsync(userId)).Returns(fakeUser);
            fakeManager.Setup(m => m.AddToRoleAsync(fakeUser.Result, userRole)).Returns(trueTask);

            UserService userService = new UserService(fakeManager.Object, _logger.Object, _mapper);
            //Act
            var result = userService.GiveModeratorRole(userId);

            //Assert
            Assert.Equal(IdentityResult.Success, result);
        }

        [Fact]
        public void RemoveModeratorRole_ShouldReturnFalse()
        {
            string userId = "testId";
            string userRole = "Moderator";

            //Arrange
            Task<ApplicationUser> fakeUser = Task.Run(() => new ApplicationUser("TestName", "TestSurname", 1, false, "image.jpg"));
            Task<IdentityResult> trueTask = Task.FromResult(IdentityResult.Success);
            fakeManager.Setup(m => m.FindByIdAsync(userId)).Returns(fakeUser);
            fakeManager.Setup(m => m.AddToRoleAsync(fakeUser.Result, userRole)).Returns(trueTask);

            UserService userService = new UserService(fakeManager.Object, _logger.Object, _mapper);
            //Act
            var result = userService.RemoveModeratorRole(userId);

            //Assert
            Assert.False( userService.CheckIfUserHasRole(userId,userRole));
        }

        [Fact]
        public void BlockUserById_ShouldReturnTrue()
        {
            string userId = "testId";

            //Arrange
            Task<ApplicationUser> fakeUser = Task.Run(() => new ApplicationUser("TestName", "TestSurname", 1, false, "image.jpg"));
            Task<IdentityResult> trueTask = Task.FromResult(IdentityResult.Success);

            fakeManager.Setup(m => m.FindByIdAsync(userId)).Returns(fakeUser);
            fakeManager.Setup(m => m.UpdateAsync(fakeUser.Result)).Returns(trueTask);

            UserService userService = new UserService(fakeManager.Object, _logger.Object, _mapper);
            //Act
            var result = userService.BlockUserById(userId);

            //Assert
            Assert.Equal(trueTask.Result, result);
        }

        [Fact]
        public void UnblockUserById_ShouldReturnFalse()
        {
            string userId = "testId";

            //Arrange
            Task<ApplicationUser> fakeUser = Task.Run(() => new ApplicationUser("TestName", "TestSurname", 1, false, "image.jpg"));
            Task<IdentityResult> trueTask = Task.FromResult(IdentityResult.Success);

            fakeManager.Setup(m => m.FindByIdAsync(userId)).Returns(fakeUser);
            fakeManager.Setup(m => m.UpdateAsync(fakeUser.Result)).Returns(trueTask);

            UserService userService = new UserService(fakeManager.Object, _logger.Object, _mapper);
            //Act
            var result = userService.UnBlockUserById(userId);

            //Assert
            Assert.False(userService.GetByIdAsync(userId).Result.IsBlocked);
        }

        [Fact]
        public void UpdateImage_ShouldReturnFalse()
        {
            string userId = "testId";
            string newImage = "newImage";

            //Arrange
            Task<ApplicationUser> fakeUser = Task.Run(() => new ApplicationUser("TestName", "TestSurname", 1, false, "image.jpg"));
            Task<IdentityResult> trueTask = Task.FromResult(IdentityResult.Success);

            fakeManager.Setup(m => m.FindByIdAsync(userId)).Returns(fakeUser);
            fakeManager.Setup(m => m.UpdateAsync(fakeUser.Result)).Returns(trueTask);

            UserService userService = new UserService(fakeManager.Object, _logger.Object, _mapper);
            //Act
            var result = userService.UpdateImage(userId, newImage);

            //Assert
            Assert.Equal(newImage, userService.GetByIdAsync(userId).Result.ImageSrc);
        }

        [Fact]
        public void UserProfile_ShouldReturnTrue()
        {
            string userId = "testId";

            //Arrange
            Task<ApplicationUser> fakeUser = Task.Run(() => new ApplicationUser("TestName", "TestSurname", 1, false, "image.jpg"));
            Task<IdentityResult> trueTask = Task.FromResult(IdentityResult.Success);

            fakeManager.Setup(m => m.FindByIdAsync(userId)).Returns(fakeUser);
            fakeManager.Setup(m => m.UpdateAsync(fakeUser.Result)).Returns(trueTask);

            int newCourse = 2;
            fakeUser.Result.Course = newCourse;
            //Act
            fakeManager.Object.UpdateAsync(fakeUser.Result);

            var result =  fakeManager.Object.FindByIdAsync(userId);
            //Assert
            Assert.Equal(newCourse, result.Result.Course);
        }

    }
}
