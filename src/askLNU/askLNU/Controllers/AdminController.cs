using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askLNU.BLL.DTO;
using askLNU.BLL.Interfaces;
using askLNU.DAL.Entities;
using askLNU.InputModels;
using askLNU.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace askLNU.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _userService;


        public AdminController(ILogger<AdminController> log, 
            UserManager<ApplicationUser> userManager,
            IUserService service)
        {
            _logger = log;
            _userManager = userManager;
            _userService = service;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index(string email)
        {

            return View();
        }
        
       
    }
}
