using System;
using Xunit;
using askLNU.BLL.Services;

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
        public void GetByEmailAsync_ShouldReturnTrue()
        {
            //
        }
    }
}
