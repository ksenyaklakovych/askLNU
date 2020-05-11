namespace askLNU.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using askLNU.BLL.DTO;
    using askLNU.BLL.Interfaces;
    using askLNU.DAL.Entities;
    using askLNU.ViewModels;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Logging;

    public class HomeController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly IFacultyService _facultyService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private Mapper _mapper;

        public HomeController(ILogger<HomeController> log, IQuestionService questService, IAnswerService answerService, IFacultyService facultyService, UserManager<ApplicationUser> userManager)
        {
            this._logger = log;
            this._questionService = questService;
            this._answerService = answerService;
            this._facultyService = facultyService;
            this._userManager = userManager;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<QuestionDTO, QuestionShortViewModel>());
            this._mapper = new Mapper(config);
        }

        public async Task<IActionResult> Index(string faculties, string tag, string sortMethod, int page = 1)
        {
            this._logger.LogInformation("User on main page.");

            // checking which user is logged in
            var userCurrent = await this._userManager.GetUserAsync(this.User);

            // get all questions from DataBase
            var questions = this._mapper.Map<IEnumerable<QuestionShortViewModel>>(this._questionService.GetAll()).ToList();

            // filling all class properties from another table from DataBase
            for (int i = 0; i < questions.Count(); i++)
            {
                questions[i].Tags = this._questionService.GetTagsByQuestionID(questions[i].Id).ToList();
                questions[i].NumberOfAnswers = this._answerService.GetAnswersByQuestionId(questions[i].Id).Count();
                if (userCurrent != null)
                {
                    questions[i].IsFavorite = this._questionService.IsQuestionFavorite(userCurrent.Id, questions[i].Id);
                }
            }

            // checking if tag field isn't empty
            if (!string.IsNullOrEmpty(tag))
            {
                this._logger.LogInformation("Filtering by tags");

                var list_of_questions = new List<QuestionShortViewModel> { };
                for (int i = 0; i < questions.Count(); i++)
                {
                    var check = false;
                    for (int j = 0; j < questions[i].Tags.Count(); j++)
                    {
                        if (questions[i].Tags[j].Contains(tag))
                        {
                            check = true;
                            break;
                        }
                    }

                    if (check == true)
                    {
                        list_of_questions.Add(questions[i]);
                    }
                }

                questions = list_of_questions;
            }

            // change list to IEnumerable
            IEnumerable<QuestionShortViewModel> questionsWithTags = questions;

            // create ViewBag to pass sorting methods to View
            this.ViewBag.sortMethod = new SelectList(new List<string> { "Rating", "Date", "Number of answers" });

            // sort depending on sort method
            switch (sortMethod)
            {
                case "Rating":
                    questionsWithTags = questionsWithTags.OrderBy(s => s.Rating);
                    this._logger.LogInformation("Sorting questions by rate.");
                    break;
                case "Date":
                    questionsWithTags = questionsWithTags.OrderBy(s => s.Date);
                    this._logger.LogInformation("Sorting questions by date.");
                    break;
                case "Number of answers":
                    questionsWithTags = questionsWithTags.OrderBy(s => s.NumberOfAnswers);
                    this._logger.LogInformation("Sorting questions by number of answers.");
                    break;
            }

            // get faculty id
            var facultyID = this._facultyService.GetFacultyIdByName(faculties);

            // get list of names of faculties
            var nameFaculties = new SelectList(this._facultyService.GetAll().Select(f => f.Title).ToList());

            // add faculty name to ViewBag to have names in DropDown kist in View
            this.ViewBag.faculties = nameFaculties;

            // filters all guestions by faculty_id
            if (!string.IsNullOrEmpty(faculties))
            {
                this._logger.LogInformation("Filtering by faculty.");
                questionsWithTags = questionsWithTags.Where(s => s.FacultyId == facultyID);
            }

            int maxRows = 3;
            var questionsPerPages = questionsWithTags.Skip((page - 1) * maxRows).Take(maxRows);
            double pageCount = (int)Math.Ceiling((decimal)questionsWithTags.Count() / maxRows);
            PagedViewModel pagedQuestions = new PagedViewModel { PageCount = (int)Math.Ceiling(pageCount), CurrentPageIndex = page, Questions = questionsPerPages };
            return this.View(pagedQuestions);
        }

        public async Task<IActionResult> AddToFavorites(int questionId)
        {
            var userCurrent = await this._userManager.GetUserAsync(this.User);
            if (this._questionService.IsQuestionFavorite(userCurrent.Id, questionId))
            {
                this._logger.LogInformation("Adding question to favorites.");
                this._questionService.RemoveFromFavorites(userCurrent.Id, questionId);
            }
            else
            {
                this._logger.LogInformation("Removing question from favorites.");
                this._questionService.AddToFavorites(userCurrent.Id, questionId);
            }

            return this.RedirectToAction("FavoriteUserQuestions", "Question");
        }

        public IActionResult Privacy()
        {
            this._logger.LogInformation("User on privacy page.");
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "User, Moderator, Admin")]
        public ActionResult Chat()
        {
            return this.View();
        }
    }
}
