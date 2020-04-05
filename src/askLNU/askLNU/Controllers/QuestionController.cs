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

        [HttpGet("{controller}/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var question = _questionService.GetQuestion(id);
            var author = await _userService.GetByIdAsync(question.ApplicationUserId);

            var authorViewModel = new UserShortViewModel
            {
                UserName = author.UserName,
                ImageSrc = author.ImageSrc
            };

            var viewModel = new QuestionViewModel
            {
                Id = question.Id,
                Title = question.Title,
                Text = question.Text,
                Date = question.Date,
                Author = authorViewModel,
                Tags = question.TagsId.Select(id => _tagService.GetTag(id).Text).ToList(),
                Rating = question.Rating,
                Answers = question.Answers.Select(a => new AnswerViewModel
                {
                    Text = a.Text,
                    Rating = a.Rating,
                    IsSolution = a.IsSolution,
                    Date = a.Date,
                    AuthorName = _userService.GetByIdAsync(a.ApplicationUserId).Result.UserName
                }).ToList()
            };

            return View(viewModel);
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

            if (questionInput.Tags != null)
            {
                foreach (var tag in questionInput.Tags)
                {
                    var tagId = _tagService.FindOrCreate(tag);
                    _questionService.AddTag(questionDTO.Id, tagId);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult VoteUp(int questionId)
        {
            var userId = _userService.GetUserId(User);
            _questionService.VoteUp(userId, questionId);

            return RedirectToAction("Details", new { id = questionId });
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult VoteDown(int questionId)
        {
            var userId = _userService.GetUserId(User);
            _questionService.VoteDown(userId, questionId);

            return RedirectToAction("Details", new { id = questionId });
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult AddAnswer(int questionId, string answerText)
        {
            var answer = new AnswerDTO
            {
                Text = answerText,
                ApplicationUserId = _userService.GetUserId(User),
                Date = DateTime.UtcNow
            };

            _questionService.AddAnswer(questionId, answer);

            return RedirectToAction("Details", new { id = questionId });
        }
    }
}