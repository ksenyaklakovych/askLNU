namespace askLNU.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using askLNU.BLL.Interfaces;
    using askLNU.InputModels;
    using askLNU.ViewModels;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class LoginController : Controller
    {
        private readonly ISignInService _signInService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ISignInService signInService, ILogger<LoginController> logger)
        {
            this._signInService = signInService;
            this._logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var viewModel = new LoginViewModel
            {
                ExternalLogins = new List<AuthenticationScheme>(),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Login(LoginInputModel loginInputModel)
        {
            if (this.ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await this._signInService.PasswordSignInAsync(loginInputModel.UserName, loginInputModel.Password, loginInputModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    this._logger.LogInformation("User logged in.");
                    return this.RedirectToAction("Index", "Home");
                }

                if (result.RequiresTwoFactor)
                {
                    // return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    this._logger.LogWarning("User account locked out.");
                    return this.RedirectToPage("./Lockout");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return this.View("Index");
                }
            }

            return this.View("Index");
        }
    }
}