using askLNU.BLL.DTO;
using askLNU.BLL.Interfaces;
using askLNU.DAL.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using askLNU.BLL.Infrastructure.Exceptions;

namespace askLNU.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<ApplicationUser> userManager,
            ILogger<UserService> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }
        public UserService(
            UserManager<ApplicationUser> userManager,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;

        }

        public async Task<IdentityResult> CreateUserAsync(UserDTO user, string password)
        {
            var applicationUser = _mapper.Map<ApplicationUser>(user);

            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null && !(await _userManager.IsEmailConfirmedAsync(existingUser)))
            {
                await _userManager.DeleteAsync(existingUser);
            }

            var result = await _userManager.CreateAsync(applicationUser, password);

            if (result.Succeeded)
            {
                await AddUserToRoleAsync(applicationUser, "User");
                _logger.LogInformation("User created a new account with password.");
            }
            return result;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            _logger.LogInformation("Generated email confirmation token.");
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<UserDTO> GetByEmailAsync(string email)
        {
            var appLicationUser = await _userManager.FindByEmailAsync(email);

            if (appLicationUser != null)
            {
               _logger.LogInformation("Got userDTO by email.");
                return _mapper.Map<UserDTO>(appLicationUser);
            }
            else
            {
                var message = $"User with email {email} couldn`t be found.";
                _logger.LogWarning(message);
                throw new ItemNotFoundException(message);
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

        public async Task<UserDTO> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            _logger.LogInformation("Got UserDTO by id.");
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public string GetUserId(ClaimsPrincipal claims)
        {
            return _userManager.GetUserId(claims);
        }

        public async Task<IdentityResult> UpdateImage(string userId, string imageSrc)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.ImageSrc = imageSrc;
            return await _userManager.UpdateAsync(user);
        }

        public IEnumerable<UserDTO> GetUsersByEmail(string email)
        {
            var allUsers = _userManager.Users.Where(u => u.Email == email);
            _logger.LogInformation("Got users DTO by email.");
            return _mapper.Map<IEnumerable<UserDTO>>(allUsers);
        }

        public bool CheckIfUserHasRole(string userID, string roleName)
        {
            var user = _userManager.FindByIdAsync(userID).Result;
            var result = _userManager.IsInRoleAsync(user, roleName);
            _logger.LogInformation($"Checked if userDTO has role {roleName}: {result.Result}.");
            return result.Result;
        }

        public void RemoveModeratorRole(string userId)
        {
            _logger.LogInformation("Removed Moderator rights from user.");
            var user = _userManager.FindByIdAsync(userId).Result;
            var res=_userManager.RemoveFromRoleAsync(user, "Moderator").Result;
        }

        public void GiveModeratorRole(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            _logger.LogInformation("Gave user Moderator rights.");
            var res=_userManager.AddToRoleAsync(user, "Moderator").Result;
        }
    }
}
