using askLNU.BLL.DTO;
using askLNU.BLL.Interfaces;
using askLNU.DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace askLNU.BLL.Services
{
    public class SignInService : ISignInService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignInService(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<List<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public bool IsSignedIn(ClaimsPrincipal claimsPrincipal)
        {
            return _signInManager.IsSignedIn(claimsPrincipal);
        }

        public async Task SignInAsync(UserDTO user, bool isPersistent)
        {
            var applicationUser = new ApplicationUser
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Course = user.Course,
                FacultyId = user.FacultyId,
                ImageSrc = user.ImageSrc,
                IsBlocked = user.IsBlocked,
                Email = user.Email
            };

            await _signInManager.SignInAsync(applicationUser, isPersistent: false);
        }
    }
}
