using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askLNU.BLL.DTO;
using askLNU.BLL.Interfaces;
using askLNU.DAL.Entities;
using askLNU.InputModels;
using askLNU.ViewModels;
using AutoMapper;
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
        private Mapper _mapper;

        public AdminController(ILogger<AdminController> log,
            UserManager<ApplicationUser> userManager,
            IUserService service)
        {
            _logger = log;
            _userManager = userManager;
            _userService = service;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserForAdminViewModel>());
            _mapper = new Mapper(config);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index(string email)
        {
            var usersDTO = _userService.GetUsersByEmail(email);
            var usersViewModel = _mapper.Map<IEnumerable<UserForAdminViewModel>>(usersDTO);
            foreach (var user in usersViewModel)
            {
                var userDTO = new UserDTO
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email
                };
                user.IsModerator = _userService.CheckIfUserHasRole(userDTO, "Moderator");
            }
            return View(usersViewModel);
        }

        public ActionResult ChangeRights(string userId, bool remove)
        {
            if (remove)
            {
                _userService.RemoveModeratorRole(userId);
            }
            else
            {
                _userService.GiveModeratorRole(userId);
            }
            return RedirectToAction("Index");
        }


    }
}
