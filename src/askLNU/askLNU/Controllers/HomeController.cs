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
        private readonly IMapper _mapper;



        public HomeController(IQuestionService service, IMapper mapper)
        {
            _questionService = service;
            //_questionService = questionService;
            _mapper = mapper;
        }
        
        public ActionResult Index()
        {
            var allQuestions = _questionService.GetAll();
            var quests = _mapper.Map<IEnumerable<QuestionDTO>, List<QuestionViewModel>>(allQuestions);
            return View(quests);
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
