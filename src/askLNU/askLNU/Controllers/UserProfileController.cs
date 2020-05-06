namespace askLNU.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using askLNU.DAL.Entities;
    using askLNU.ViewModels;
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private Mapper _mapper;

        public UserProfileController(
           UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser,UserProfileViewModel>());
            this._mapper = new Mapper(config);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            var userModel = this._mapper.Map<UserProfileViewModel>(user);
            return this.View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserProfileViewModel user)
        {
            var userCurrent = await this._userManager.GetUserAsync(this.User);
            userCurrent.Name = user.Name;
            userCurrent.Surname = user.Surname;
            userCurrent.Email = user.Email;
            userCurrent.Course = user.Course;
            userCurrent.ImageSrc = user.ImageSrc;
            userCurrent.UserName = user.UserName;

            var updatedUser = await this._userManager.UpdateAsync(userCurrent);
            var userModel = this._mapper.Map<UserProfileViewModel>(userCurrent);
            return this.View(userModel);
        }
    }
}