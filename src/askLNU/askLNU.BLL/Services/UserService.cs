using askLNU.BLL.DTO;
using askLNU.BLL.Interfaces;
using askLNU.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace askLNU.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<ApplicationUser> userManager,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IdentityResult> CreateUserAsync(UserDTO user, string password)
        {
            var applicationUser = new ApplicationUser
            {
                Name = user.Name,
                Surname = user.Surname,
                Course = user.Course,
                FacultyId = user.FacultyId,
                ImageSrc = user.ImageSrc,
                UserName = user.UserName,
                Email = user.Email
            };

            var result = await _userManager.CreateAsync(applicationUser, password);

            if (result.Succeeded)
            {
                await AddUserToRoleAsync(applicationUser, "User");
                _logger.LogInformation("User created a new account with password.");
            }

            return result;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(UserDTO user)
        {
            var applicationUser = new ApplicationUser
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Course = user.Course,
                FacultyId = user.FacultyId,
                ImageSrc = user.ImageSrc,
                UserName = user.UserName,
                Email = user.Email
            };

            return await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
        }

        public async Task<UserDTO> GetByEmailAsync(string email)
        {
            var appLicationUser = await _userManager.FindByEmailAsync(email);

            if (appLicationUser != null)
            {
                return new UserDTO
                {
                    Id = appLicationUser.Id,
                    UserName = appLicationUser.UserName,
                    Name = appLicationUser.Name,
                    Surname = appLicationUser.Surname,
                    Course = appLicationUser.Course,
                    FacultyId = appLicationUser.FacultyId,
                    ImageSrc = appLicationUser.ImageSrc,
                    IsBlocked = appLicationUser.IsBlocked,
                    Email = appLicationUser.Email
                };
            }
            else
            {
                return null;
            }
        }

        public bool RequireConfirmedAccount()
        {
            return _userManager.Options.SignIn.RequireConfirmedAccount;
        }

        private async Task AddUserToRoleAsync(ApplicationUser user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }
    }
}
