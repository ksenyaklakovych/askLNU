using askLNU.BLL.DTO;
using askLNU.BLL.Interfaces;
using askLNU.DAL.Entities;
using AutoMapper;
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
                _logger.LogWarning($"User with email {email} couldn`t be found.");
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
    }
}
