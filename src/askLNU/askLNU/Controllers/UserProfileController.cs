using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using askLNU.ViewModels;
using askLNU.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace askLNU.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private Mapper _mapper;
        public UserProfileController(
           UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser,UserProfileViewModel>());
            _mapper = new Mapper(config);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userModel =  _mapper.Map<UserProfileViewModel>(user);
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserProfileViewModel user)
        {
            var userCurrent = await _userManager.GetUserAsync(User);
            userCurrent.Name = user.Name;
            userCurrent.Surname = user.Surname;
            userCurrent.Email = user.Email;
            userCurrent.Course = user.Course;
            userCurrent.ImageSrc = user.ImageSrc;
            userCurrent.UserName = user.UserName;

            var updatedUser = await _userManager.UpdateAsync(userCurrent);
            var userModel = _mapper.Map<UserProfileViewModel>(userCurrent);
            return View(userModel);
        }
    }
}

