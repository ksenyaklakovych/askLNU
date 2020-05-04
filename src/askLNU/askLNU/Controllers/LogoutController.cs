namespace askLNU.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using askLNU.BLL.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class LogoutController : Controller
    {
        private readonly ISignInService _signInService;
        private readonly ILogger<LogoutController> _logger;

        public LogoutController(
            ISignInService signInService,
            ILogger<LogoutController> logger)
        {
            this._signInService = signInService;
            this._logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this._signInService.SignOutAsync();
            this._logger.LogInformation("User logged out.");

            return this.RedirectToAction("Index", "Home");
        }
    }
}