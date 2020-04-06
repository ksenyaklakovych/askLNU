using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using askLNU.BLL.DTO;
using askLNU.BLL.Interfaces;
using askLNU.InputModels;
using askLNU.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace askLNU.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;
        private readonly ISignInService _signInService;
        private readonly ILogger<RegisterController> _logger;
        private readonly IImageService _imageService;

        public RegisterController(
            IEmailSender emailSender,
            IUserService userService,
            ISignInService signInService, ILogger<RegisterController> logger)
            ISignInService signInService,
            IImageService imageService)
        {
            _emailSender = emailSender;
            _userService = userService;
            _signInService = signInService;
            _logger = logger;
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            var viewModel = new RegisterViewModel
            {
                ExternalLogins = new List<AuthenticationScheme>()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel registerInputModel)
        {
            if (ModelState.IsValid)
            {
                var user = new UserDTO
                {
                    UserName = registerInputModel.UserName,
                    Email = registerInputModel.Email,
                    Name = registerInputModel.Name,
                    Surname = registerInputModel.Surname,
                    Course = registerInputModel.Course
                };

                var result = await _userService.CreateUserAsync(user, registerInputModel.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User registered successfully.");
                    user = await _userService.GetByEmailAsync(user.Email);
                    var imageSrc = await _imageService.SaveImage(registerInputModel.Image);
                    await _userService.UpdateImage(user.Id, imageSrc);

                    var code = await _userService.GenerateEmailConfirmationTokenAsync(user.Id);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Action(
                         "ConfirmEmail", "Register",
                         new { userId = user.Id, code = code },
                         protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(registerInputModel.Email, "Confirm your email",
                        $"Please confirm your registration at askLNU website by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userService.RequireConfirmedAccount())
                    {
                        _logger.LogInformation("Redirected to registration confirmation page.");
                        return RedirectToAction("Confirmation", new { email = registerInputModel.Email });
                    }
                    else
                    {
                        await _signInService.SignInAsync(user, false);
                        _logger.LogInformation("User signed in.");
                        return RedirectToAction("Index");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("Index");
        }

        public async Task<IActionResult> Confirmation(string email)
        {
            if (email == null)
            {
                return RedirectToAction("Index");
            }

            var user = await _userService.GetByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            var viewModel = new RegisterConfirmationViewModel();

            viewModel.DisplayConfirmAccountLink = false;

            return View(viewModel);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userService.ConfirmEmailAsync(userId, code);

            var viewModel = new ConfirmEmailViewModel
            {
                StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.",
                Succeeded = result.Succeeded ? true : false
            };

            return View(viewModel);
        }
    }
}