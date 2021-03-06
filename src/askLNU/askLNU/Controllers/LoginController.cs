﻿namespace askLNU.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using askLNU.BLL.Interfaces;
    using askLNU.DAL.Entities;
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
        private readonly UserManager<ApplicationUser> _userManager;


        public LoginController(ISignInService signInService, ILogger<LoginController> logger, UserManager<ApplicationUser> userManager)
        {
            this._signInService = signInService;
            this._logger = logger;
            this._userManager = userManager;

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
                var user = this._userManager.FindByNameAsync(loginInputModel.UserName);
                if (user.Result != null && user.Result.IsBlocked)
                {
                    this.ModelState.AddModelError("RememberMe", "You are blocked.");
                    return this.View("Index");
                }

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