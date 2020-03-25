using System;
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

namespace askLNU.Controllers
{
    public class HomeController : Controller
    {
       // private readonly ILogger<HomeController> _logger;
        private readonly IQuestionService _questionService;



        public HomeController(IQuestionService service)
        {
            _questionService = service;
            //_questionService = questionService;
        }
        
        public ActionResult Index()
        {
            var allQuestions = _questionService.GetAll();
            var list = new List<QuestionViewModel>();
            foreach (var item in allQuestions)
            {
                var q = new QuestionViewModel
                {
                    ApplicationUserId = item.ApplicationUserId,
                    Id = item.Id,
                    Title = item.Title,
                    Rating = item.Rating,
                    IsSolved = item.IsSolved,
                    Date = item.Date,
                    FacultyId = item.FacultyId
                };
                list.Add(q);
            }
            IEnumerable<QuestionViewModel> en = list;
            return View(en);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
