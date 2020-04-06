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

namespace askLNU.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISignInService _signInService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ISignInService signInService, ILogger<LoginController> logger)
        {
            _signInService = signInService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var viewModel = new LoginViewModel
            {
                ExternalLogins = new List<AuthenticationScheme>()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Login(LoginInputModel loginInputModel)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInService.PasswordSignInAsync(loginInputModel.UserName, loginInputModel.Password, loginInputModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Home");
                }
                if (result.RequiresTwoFactor)
                {
                    // return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View("Index");
                }
            }

            return View("Index");
        }
    }
}