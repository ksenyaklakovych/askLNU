using System;
using PagedList;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using askLNU.ViewModels;
using askLNU.BLL.Interfaces;
using askLNU.BLL.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using askLNU.DAL.Entities;
using System.Linq;

namespace askLNU.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly IFacultyService _facultyService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        private Mapper _mapper;
        public HomeController(ILogger<HomeController> log,IQuestionService questService, IAnswerService answerService, IFacultyService facultyService, UserManager<ApplicationUser> userManager)
        {
            _logger = log;
            _questionService = questService;
            _answerService = answerService;
            _facultyService = facultyService;
            _userManager = userManager;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<QuestionDTO, QuestionViewModel>());
            _mapper = new Mapper(config);
        }
        
        public async Task<IActionResult> Index(int? page, string Faculties, string tag,string sortMethod)
        {
            _logger.LogInformation("User on main page.");

            //checking which user is logged in
            var userCurrent = await _userManager.GetUserAsync(User);
            // get all questions from DataBase 
            var questions = _mapper.Map<IEnumerable<QuestionViewModel>>(_questionService.GetAll()).ToList();
            //filling all class properties from another table from DataBase
            for (int i = 0; i < questions.Count(); i++)
            {
                questions[i].Tags = _questionService.GetTagsByQuestionID(questions[i].Id).ToList();
                questions[i].numberOfAnswers = _answerService.GetAnswersByQuestionId(questions[i].Id).Count();
                if (userCurrent!=null)
                {
                    questions[i].IsFavorite = _questionService.IsQuestionFavorite(userCurrent.Id, questions[i].Id);

                }
            }
            //checking if tag field isn't empty
            if (!String.IsNullOrEmpty(tag))
            {
                _logger.LogInformation("Filtering by tags");

                var list_of_questions = new List<QuestionViewModel> { };
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
            //change list to IEnumerable
            IEnumerable<QuestionViewModel> questionsWithTags = questions;
            //create ViewBag to pass sorting methods to View 
            ViewBag.sortMethod = new SelectList(new List<string> {"Rating","Date","Number of answers" });
            //sort depending on sort method
            switch (sortMethod)
            {
                case "Rating":
                    questionsWithTags = questionsWithTags.OrderBy(s => s.Rating);
                    _logger.LogInformation("Sorting questions by rate.");
                    break;
                case "Date":
                    questionsWithTags = questionsWithTags.OrderBy(s => s.Date);
                    _logger.LogInformation("Sorting questions by date.");
                    break;
                case "Number of answers":
                    questionsWithTags = questionsWithTags.OrderBy(s => s.numberOfAnswers);
                    _logger.LogInformation("Sorting questions by number of answers.");
                    break;
            }
            //get faculty id
            var facultyID =_facultyService.GetFacultyIdByName(Faculties);
            // get list of names of faculties
            var nameFaculties = new SelectList(_questionService.GetAllFaculties().Select(f=>f.Title).ToList());
            //add faculty name to ViewBag to have names in DropDown kist in View
            ViewBag.Faculties = nameFaculties;
            // filters all guestions by faculty_id
            if (!String.IsNullOrEmpty(Faculties))
            {
                _logger.LogInformation("Filtering by faculty.");
                questionsWithTags = questionsWithTags.Where(s=>s.FacultyId==facultyID);
            }

           // paganation of mainpage
            int pageSize = 4;
            int pageNumber = (page ?? 1);
           
            return View(questionsWithTags.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> AddToFavorites(int questionId)
        {
            var userCurrent = await _userManager.GetUserAsync(User);
            if (_questionService.IsQuestionFavorite(userCurrent.Id,questionId))
            {
                _logger.LogInformation("Adding question to favorites.");
                _questionService.RemoveFromFavorites(userCurrent.Id, questionId);
            }
            else
            {
                _logger.LogInformation("Removing question from favorites.");
                _questionService.AddToFavorites(userCurrent.Id, questionId);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            _logger.LogInformation("User on privacy page.");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
