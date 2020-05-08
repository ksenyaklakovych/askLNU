namespace askLNU.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using System.Security.Claims;
    using askLNU.BLL.DTO;
    using askLNU.BLL.Interfaces;
    using askLNU.DAL.Entities;
    using askLNU.InputModels;
    using askLNU.ViewModels;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    public class RegisterController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;
        private readonly ISignInService _signInService;
        private readonly ILogger<RegisterController> _logger;
        private readonly IImageService _imageService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public RegisterController(
            IEmailSender emailSender,
            IUserService userService,
            ISignInService signInService,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterController> logger,
            IImageService imageService,
            UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
            this._emailSender = emailSender;
            this._userService = userService;
            this._signInService = signInService;
            this._logger = logger;
            this._imageService = imageService;
            this._signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var viewModel = new RegisterViewModel
            {
                ExternalLogins = new List<AuthenticationScheme>(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel registerInputModel)
        {
            if (this.ModelState.IsValid)
            {
                var user = new UserDTO
                {
                    UserName = registerInputModel.UserName,
                    Email = registerInputModel.Email,
                    Name = registerInputModel.Name,
                    Surname = registerInputModel.Surname,
                    Course = registerInputModel.Course,
                };

                var result = await this._userService.CreateUserAsync(user, registerInputModel.Password);
                if (result.Succeeded)
                {
                    this._logger.LogInformation("User registered successfully.");
                    user = await this._userService.GetByEmailAsync(user.Email);
                    var imageSrc = await this._imageService.SaveImage(registerInputModel.Image);
                    await this._userService.UpdateImage(user.Id, imageSrc);

                    var code = await this._userService.GenerateEmailConfirmationTokenAsync(user.Id);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = this.Url.Action(
                         "ConfirmEmail", "Register",
                         new { userId = user.Id, code = code, },
                         protocol: this.Request.Scheme);

                    await this._emailSender.SendEmailAsync(registerInputModel.Email, "Confirm your email",
                        $"Please confirm your registration at askLNU website by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (this._userService.RequireConfirmedAccount())
                    {
                        this._logger.LogInformation("Redirected to registration confirmation page.");
                        return this.RedirectToAction("Confirmation", new { email = registerInputModel.Email });
                    }
                    else
                    {
                        await this._signInService.SignInAsync(user, false);
                        this._logger.LogInformation("User signed in.");
                        return this.RedirectToAction("Index");
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return this.View("Index");
        }

        public async Task<IActionResult> Confirmation(string email)
        {
            if (email == null)
            {
                return this.RedirectToAction("Index");
            }

            var user = await this._userService.GetByEmailAsync(email);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with email '{email}'.");
            }

            var viewModel = new RegisterConfirmationViewModel();

            viewModel.DisplayConfirmAccountLink = false;

            return this.View(viewModel);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return this.RedirectToPage("/Index");
            }

            var user = await this._userService.GetByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await this._userService.ConfirmEmailAsync(userId, code);

            var viewModel = new ConfirmEmailViewModel
            {
                StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.",
                Succeeded = result.Succeeded,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(this.ExternalLoginCallback), "Register", new { returnUrl });
            var properties = this._signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return RedirectToAction("Login");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return Redirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction("LogOut");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return Redirect(returnUrl);
                    }
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }
    }
}