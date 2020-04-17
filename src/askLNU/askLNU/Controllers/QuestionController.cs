namespace askLNU.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using askLNU.BLL.DTO;
    using askLNU.BLL.Infrastructure.Exceptions;
    using askLNU.BLL.Interfaces;
    using askLNU.InputModels;
    using askLNU.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

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
            this._facultyService = facultyService;
            this._tagService = tagService;
            this._questionService = questionService;
            this._userService = userService;
            this._answerService = answerService;
            this._logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("{controller}/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var question = this._questionService.GetQuestion(id);
                var author = await this._userService.GetByIdAsync(question.ApplicationUserId);
                var authorViewModel = new UserShortViewModel
                {
                    UserName = author.UserName,
                    ImageSrc = author.ImageSrc,
                };

                var viewModel = new QuestionViewModel
                {
                    Id = question.Id,
                    Title = question.Title,
                    Text = question.Text,
                    Date = question.Date,
                    Author = authorViewModel,
                    Tags = question.TagsId.Select(id => this._tagService.GetTag(id).Text).ToList(),
                    Rating = question.Rating,
                };

                return this.View(viewModel);
            }
            catch (ItemNotFoundException exception)
            {
                return this.RedirectToAction("ShowError", "CustomError", new { errorMessage = exception.Message });
            }
        }

        public IActionResult Create()
        {
            var viewModel = new CreateQuestionViewModel
            {
                Faculties = this._facultyService.GetAll().ToList(),
            };

            return this.View(viewModel);
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
                ApplicationUserId = this._userService.GetUserId(this.User),
            };

            this._questionService.CreateQuestion(questionDTO);

            if (questionInput.Tags != null)
            {
                foreach (var tag in questionInput.Tags)
                {
                    var tagId = this._tagService.FindOrCreate(tag);
                    this._questionService.AddTag(questionDTO.Id, tagId);
                }
            }

            this._logger.LogInformation($"Question #{questionDTO.Id} created.");

            return this.RedirectToAction("Details", "Question", new { id = questionDTO.Id });
        }

        [HttpPost]
        public int VoteUp([FromForm] int questionId)
        {
            var userId = this._userService.GetUserId(this.User);
            var rating = this._questionService.VoteUp(userId, questionId);
            return rating;
        }

        [HttpPost]
        public int VoteDown([FromForm] int questionId)
        {
            var userId = this._userService.GetUserId(this.User);
            var rating = this._questionService.VoteDown(userId, questionId);
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
                    ApplicationUserId = this._userService.GetUserId(this.User),
                    Date = DateTime.UtcNow,
                };

                this._questionService.AddAnswer(questionId.Value, answer);

                return this.GetLastAnswers(questionId.Value, 10);
            }
            else
            {
                return null;
            }
        }

        [AllowAnonymous]
        public List<AnswerViewModel> GetLastAnswers(int questionId, int amount)
        {
            var answers = this._answerService.GetAnswersByQuestionId(questionId);
            var sortedAnswers = from answer in answers
                                orderby answer.Date descending
                                orderby answer.IsSolution descending
                                select new AnswerViewModel
                                {
                                    Text = answer.Text,
                                    Rating = answer.Rating,
                                    IsSolution = answer.IsSolution,
                                    Date = answer.Date.ToLocalTime().ToShortDateString(),
                                    AuthorName = this._userService.GetByIdAsync(answer.ApplicationUserId).Result.UserName,
                                };

            return sortedAnswers.Take(amount).ToList();
        }

        [Authorize(Roles = "Moderator,Admin")]
        public ActionResult DeleteQuestion(int questionId)
        {
            this._logger.LogInformation($"Moderator deleted question with id {questionId}.");
            this._questionService.Dispose(questionId);
            return this.RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Moderator,Admin")]
        public ActionResult DeleteAnswer(int answerId)
        {
            this._logger.LogInformation($"Moderator deleted answer with id {answerId}.");
            this._answerService.Dispose(answerId);
            return this.RedirectToAction("Index", "Home");
        }
    }
}