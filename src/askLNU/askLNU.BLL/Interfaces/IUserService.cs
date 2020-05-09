using askLNU.BLL.DTO;
using askLNU.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace askLNU.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(UserDTO user, string password);
       
        Task<UserDTO> GetByEmailAsync(string email);

        Task AddUserToRoleAsync(ApplicationUser user, string role);

        Task<UserDTO> GetByIdAsync(string id);
        
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        
        bool RequireConfirmedAccount();
        
        string GetUserId(ClaimsPrincipal claims);
        
        Task<IdentityResult> UpdateImage(string userId, string imageSrc);
        
        IEnumerable<UserDTO> GetUsersByEmail(string email);
        
        bool CheckIfUserHasRole(string userId, string roleName);
        
        void RemoveModeratorRole(string userId);
        
        void GiveModeratorRole(string userId);

        void BlockUserById(string userId);
        void UnBlockUserById(string userId);

    }
}
