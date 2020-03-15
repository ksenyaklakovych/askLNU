using askLNU.BLL.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace askLNU.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(UserDTO user, string password);
        Task<UserDTO> GetByEmailAsync(string email);
        Task<string> GenerateEmailConfirmationTokenAsync(UserDTO user);
        bool RequireConfirmedAccount();
    }
}
