using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askLNU.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace askLNU.Controllers
{
    public class LogoutController : Controller
    {
        private readonly ISignInService _signInService;
        private readonly ILogger<LogoutController> _logger;

        public LogoutController(
            ISignInService signInService,
            ILogger<LogoutController> logger)
        {
            _signInService = signInService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInService.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return RedirectToAction("Index", "Home");
        }
    }
}