using askLNU.BLL.Configs;
using askLNU.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace askLNU.BLL.Infrastructure
{
    public static class IServiceProviderDIExtension
    {
        public static async Task CreateUserRoles(this IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in new string[] { "User", "Moderator", "Admin" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminConfig = serviceProvider.GetRequiredService<IOptions<AdminConfig>>();

            if (await userManager.FindByEmailAsync(adminConfig.Value.Email) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminConfig.Value.Email,
                    Email = adminConfig.Value.Email,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, adminConfig.Value.Password);
                await userManager.AddToRolesAsync(admin, new string[] { "User", "Moderator", "Admin" });
            }
        }
    }
}
