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

namespace askLNU.Controllers
{
    public class HomeController : Controller
    {
       // private readonly ILogger<HomeController> _logger;
        private readonly IQuestionService _questionService;
        private Mapper _mapper;
        public HomeController(IQuestionService service)
        {
            _questionService = service;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<QuestionDTO, QuestionViewModel>());
            _mapper = new Mapper(config);
        }
        
        public ActionResult Index(int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            var questions = _mapper.Map<IEnumerable<QuestionViewModel>>(_questionService.GetAll());
            return View(questions.ToPagedList(pageNumber, pageSize));
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
