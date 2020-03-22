using askLNU.BLL.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace askLNU.BLL.Interfaces
{
    public interface ISignInService
    {
        Task SignInAsync(UserDTO user, bool isPersistent);
        Task<List<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
        bool IsSignedIn(ClaimsPrincipal claimsPrincipal);
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignOutAsync();
    }
}
