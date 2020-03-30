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
        
        public ActionResult Index(int? page, string Faculties)
        {
            var questions = _mapper.Map<IEnumerable<QuestionViewModel>>(_questionService.GetAll()).ToList();
            for (int i = 0; i < questions.Count(); i++)
            {
                questions[i].Tags = _questionService.GetTagsByQuestionID(questions[i].Id).ToList();
            }

            IEnumerable<QuestionViewModel> questionsWithTags = questions;

            var facultyID =_questionService.GetIdByFacutyName(Faculties);
            var nameFaculties = new SelectList(_questionService.GetAllFaculties().Select(f=>f.Title).ToList());
            ViewBag.Faculties = nameFaculties;
           
            if (!String.IsNullOrEmpty(Faculties))
            {
                questionsWithTags = questionsWithTags.Where(s=>s.FacultyId==facultyID);
            }
           
            

            int pageSize = 5;
            int pageNumber = (page ?? 1);
           
            return View(questionsWithTags.ToPagedList(pageNumber, pageSize));
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
