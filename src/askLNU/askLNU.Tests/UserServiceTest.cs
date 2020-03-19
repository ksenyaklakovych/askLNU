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

namespace askLNU.Tests
{
    public class UserServiceTest
    {
        [Fact]
        public void GetByEmailAsync_ShouldReturnTrue()
        {
            string email = "test";
            //Arrange
            var fakeUser = Task.Run(() => new ApplicationUser("TestName","TestSurname",1,false,"image.jpg"));
            var fakeManager = new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
            fakeManager.Setup(m => m.FindByEmailAsync(email)).Returns(fakeUser);
            
            UserService userService = new UserService(fakeManager.Object);
            //Act
            var result = userService.GetByEmailAsync(email);
            //Assert
            Assert.Equal("TestName",result.Result.Name);
            Assert.Equal("TestSurname", result.Result.Surname);
            Assert.False(result.Result.IsBlocked);
        }
    }
}
