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
using Microsoft.Extensions.Logging;

namespace askLNU.Controllers
{
    [Authorize(Roles = "User")]
    public class QuestionController : Controller
    {
        private readonly IFacultyService _facultyService;
        private readonly ITagService _tagService;
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;
        private readonly IAnswerService _answerService;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(
            IFacultyService facultyService,
            ITagService tagService,
            IQuestionService questionService,
            IUserService userService,
            ILogger<QuestionController> logger,
            IAnswerService answerService)
        {
            _facultyService = facultyService;
            _tagService = tagService;
            _questionService = questionService;
            _userService = userService;
            _answerService = answerService;
            _logger = logger;
        }

        [AllowAnonymous]
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
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateQuestionViewModel
            {
                Faculties = _facultyService.GetAll().ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
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

            return RedirectToAction("Details", "Question", new { id = questionDTO.Id });
        }

        [HttpPost]
        public int VoteUp([FromForm] int questionId)
        {
            var userId = _userService.GetUserId(User);
            var rating = _questionService.VoteUp(userId, questionId);
            return rating;
        }

        [HttpPost]
        public int VoteDown([FromForm] int questionId)
        {
            var userId = _userService.GetUserId(User);
            var rating = _questionService.VoteDown(userId, questionId);
            return rating;
        }

        [HttpPost]
        public List<AnswerViewModel> AddAnswer(int? questionId, string answerText)
        {
            if (questionId != null && !string.IsNullOrEmpty(answerText) && !string.IsNullOrWhiteSpace(answerText))
            {
                var answer = new AnswerDTO
                {
                    Text = answerText,
                    ApplicationUserId = _userService.GetUserId(User),
                    Date = DateTime.UtcNow
                };

                _questionService.AddAnswer(questionId.Value, answer);

                return GetLastAnswers(questionId.Value, 10);
            }
            else
            {
                return null;
            }
        }

        [AllowAnonymous]
        public List<AnswerViewModel> GetLastAnswers(int questionId, int amount)
        {
            var answers = _answerService.GetAnswersByQuestionId(questionId);
            var sortedAnswers = from answer in answers
                                orderby answer.Date descending
                                orderby answer.IsSolution descending
                                select new AnswerViewModel
                                {
                                    Text = answer.Text,
                                    Rating = answer.Rating,
                                    IsSolution = answer.IsSolution,
                                    Date = answer.Date.ToLocalTime().ToShortDateString(),
                                    AuthorName = _userService.GetByIdAsync(answer.ApplicationUserId).Result.UserName
                                };

            return sortedAnswers.Take(amount).ToList();
        }

        [Authorize(Roles = "Moderator,Admin")]
        public ActionResult DeleteQuestion(int questionId)
        {
            _logger.LogInformation($"Moderator deleted question with id {questionId}.");
            _questionService.Dispose(questionId);
            return RedirectToAction("Index","Home");
        }

        [Authorize(Roles = "Moderator,Admin")]
        public ActionResult DeleteAnswer(int answerId)
        {
            _logger.LogInformation($"Moderator deleted answer with id {answerId}.");
            _answerService.Dispose(answerId);
            return RedirectToAction("Index", "Home");
        }
    }
}