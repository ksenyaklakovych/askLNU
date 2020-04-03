using askLNU.BLL.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace askLNU.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(UserDTO user, string password);
        Task<UserDTO> GetByEmailAsync(string email);
        Task<UserDTO> GetByIdAsync(string id);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        bool RequireConfirmedAccount();
        string GetUserId(ClaimsPrincipal claims);
        Task<IdentityResult> UpdateImage(string userId, string imageSrc);
    }
}
