using System;
using Xunit;
using askLNU.BLL.Services;
using Moq;
using Microsoft.AspNetCore.Identity;
using askLNU.DAL.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace askLNU.Tests
{
    public class UserServiceTest
    {
        readonly string testConnectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-askLNU-6C6E514C-8476-441A-A2AB-C978F0CBD99C;Trusted_Connection=True;MultipleActiveResultSets=true";
        //UserService CreateUserService()
        //{
        //    //
        //}
        [Fact]
        public void GetByEmailAsync_ShouldReturnTrue(string email)
        {
            var fakeUser = new ApplicationUser();
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

        }
    }
}
