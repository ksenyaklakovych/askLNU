using askLNU.BLL.DTO;
using Microsoft.AspNetCore.Authentication;
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
    }
}
