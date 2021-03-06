﻿namespace askLNU.Controllers
{
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

    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _userService;
        private readonly IFacultyService _facultyService;
        private readonly ITagService _tagService;

        private Mapper _mapper;
        private Mapper _mapperFaculty;

        public AdminController(
            ILogger<AdminController> log,
            IUserService service,
            IFacultyService facultyService,
            ITagService tagService)
        {
            this._logger = log;
            this._userService = service;
            this._facultyService = facultyService;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserForAdminViewModel>());
            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<FacultyDTO, FacultyViewModel>());
            this._mapper = new Mapper(config);
            this._tagService = tagService;
            this._mapperFaculty = new Mapper(config2);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index(string email)
        {
            this._logger.LogInformation($"Admin is on page with all users and rights.");
            var usersDTO = this._userService.GetUsersByEmail(email);
            var usersViewModel = this._mapper.Map<IEnumerable<UserForAdminViewModel>>(usersDTO);
            foreach (var user in usersViewModel)
            {
                user.IsModerator = this._userService.CheckIfUserHasRole(user.Id, "Moderator");
            }

            return this.View(usersViewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ChangeRights(string userId, bool remove)
        {
            if (remove)
            {
                this._logger.LogInformation($"Admin removes Moderator rights from user with id {userId}");
                var res = this._userService.RemoveModeratorRole(userId);
            }
            else
            {
                this._logger.LogInformation($"Admin gives Moderator rights to user with id {userId}");
                var res = this._userService.GiveModeratorRole(userId);
            }

            return this.RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult BlockUser(string userId, bool block)
        {
            if (block)
            {
                this._logger.LogInformation($"Admin blocked user with id {userId}");
                var result = this._userService.BlockUserById(userId);
            }
            else
            {
                this._logger.LogInformation($"Admin unblocked user with id {userId}");
                var result = this._userService.UnBlockUserById(userId);
            }

            return this.RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AllFaculties()
        {
            this._logger.LogInformation("Admin opened page with all faculties");
            var all_faculties = this._mapperFaculty.Map<IEnumerable<FacultyViewModel>>(this._facultyService.GetAll());
            return this.View(all_faculties);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AllTags()
        {
            this._logger.LogInformation("Admin opened page with all tags");
            var all_tags = this._tagService.GetAll();
            return this.View(all_tags);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateNewFaculty()
        {
            return this.View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateNewTag()
        {
            return this.View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateNewFaculty(FacultyViewModel faculty)
        {
            this._logger.LogInformation("Admin created new faculty");
            FacultyDTO facultyDTO = new FacultyDTO { Title = faculty.Title };
            this._facultyService.CreateFaculty(facultyDTO);
            return this.RedirectToAction("AllFaculties");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateNewTag(TagDTO tag)
        {
            this._logger.LogInformation("Admin created new tag");
            TagDTO tagDTO = new TagDTO { Text = tag.Text };
            this._tagService.CreateTag(tagDTO);
            return this.RedirectToAction("AllTags");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteFaculty(int id)
        {
            this._logger.LogInformation("Admin deleted faculty");
            this._facultyService.Dispose(id);
            return this.RedirectToAction("AllFaculties");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteTag(int id)
        {
            this._logger.LogInformation("Admin deleted tag");
            this._tagService.Dispose(id);
            return this.RedirectToAction("AllTags");
        }
    }
}
