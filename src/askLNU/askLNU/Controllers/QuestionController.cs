using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askLNU.BLL.DTO;
using askLNU.BLL.Interfaces;
using askLNU.InputModels;
using askLNU.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace askLNU.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IFacultyService _facultyService;
        private readonly ITagService _tagService;
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;

        public QuestionController(
            IFacultyService facultyService,
            ITagService tagService,
            IQuestionService questionService,
            IUserService userService)
        {
            _facultyService = facultyService;
            _tagService = tagService;
            _questionService = questionService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            var viewModel = new CreateQuestionViewModel
            {
                Faculties = _facultyService.GetAll().ToList(),
                Tags = _tagService.GetAll().ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(QuestionInputModel questionInput)
        {
            var questionDTO = new QuestionDTO
            {
                Title = questionInput.Title,
                Text = questionInput.Text,
                FacultyId = questionInput.FacultyId,
                Date = DateTime.Now,
                ApplicationUserId = _userService.GetUserId(User)
            };

            _questionService.CreateQuestion(questionDTO);

            foreach (var tag in questionInput.Tags)
            {
                var tagId = _tagService.FindOrCreate(tag);
                _questionService.AddTag(questionDTO.Id, tagId);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}