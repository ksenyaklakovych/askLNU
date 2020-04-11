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
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _userService;
        private readonly IFacultyService _facultyService;

        private Mapper _mapper;
        private Mapper _mapperFaculty;

        public AdminController(ILogger<AdminController> log,
            IUserService service,
            IFacultyService facultyService)
        {
            _logger = log;
            _userService = service;
            _facultyService = facultyService;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserForAdminViewModel>());
            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<FacultyDTO, FacultyViewModel>());
            _mapper = new Mapper(config);
            _mapperFaculty = new Mapper(config2);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index(string email)
        {
            _logger.LogInformation($"Admin is on page with all users and rights.");
            var usersDTO = _userService.GetUsersByEmail(email);
            var usersViewModel = _mapper.Map<IEnumerable<UserForAdminViewModel>>(usersDTO);
            foreach (var user in usersViewModel)
            {
                user.IsModerator = _userService.CheckIfUserHasRole(user.Id, "Moderator");
            }
            return View(usersViewModel);
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult ChangeRights(string userId, bool remove)
        {
            if (remove)
            {
                _logger.LogInformation($"Admin removes Moderator rights from user wirh id {userId}");
                _userService.RemoveModeratorRole(userId);
            }
            else
            {
                _logger.LogInformation($"Admin gives Moderator rights to user wirh id {userId}");
                _userService.GiveModeratorRole(userId);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AllFaculties()
        {
            _logger.LogInformation("Display all faculties");
            var all_faculties= _mapperFaculty.Map<IEnumerable <FacultyViewModel>>(_facultyService.GetAll());
            return View(all_faculties);
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult CreateNewFaculty()
        {
            return View();
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateNewFaculty(FacultyViewModel faculty)
        {
            FacultyDTO facultyDTO = new FacultyDTO { Title = faculty.Title };
            _facultyService.CreateFaculty(facultyDTO);
            return RedirectToAction("AllFaculties");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteFaculty(int id)
        {
            _facultyService.Dispose(id);
            return RedirectToAction("AllFaculties");
        }
    }
}
