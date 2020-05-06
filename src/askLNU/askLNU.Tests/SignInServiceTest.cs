using System;
using Xunit;
using askLNU.BLL.Services;
using Moq;
using Microsoft.AspNetCore.Identity;
using askLNU.DAL.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;
using askLNU.BLL.Interfaces;
using askLNU.BLL.DTO;
using askLNU.BLL.Interfaces;
using AutoMapper;
using askLNU.DAL.Interfaces;
using System.Linq;
using System.Collections.Generic;
using askLNU.BLL.Infrastructure.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace askLNU.Tests
{
    public class LogoutControllerTest
    {
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<SignInManager<ApplicationUser>> _mockSignInManager;

        public LogoutControllerTest()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            _mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object,
                null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(_mockUserManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null);
        }

        public ClaimsPrincipal User { get; private set; }

        [Fact]
        public void Logout_ShouldReturnTrue()
        {
            //Arrange
            var fakeUser = Task.Run(() => new ApplicationUser("TestName", "TestSurname", 1, false, "image.jpg"));
            _mockSignInManager.Setup(m => m.SignOutAsync()).Returns(fakeUser);

            SignInService signInService = new SignInService(_mockSignInManager.Object);
            //Act
            var result = signInService.SignOutAsync();
            //Assert
            Assert.False(signInService.IsSignedIn(User));
        }
    }
}