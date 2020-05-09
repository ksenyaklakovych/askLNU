
namespace askLNU.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using askLNU.BLL.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class AnswerController : Controller
    {
        private readonly IUserService userService;
        private readonly IAnswerService answerService;

        public AnswerController(
            IUserService userService,
            IAnswerService answerService
            )
        {
            this.userService = userService;
            this.answerService = answerService;
        }

        [HttpPost]
        public int VoteUp([FromForm] int answerId)
        {
            var userId = this.userService.GetUserId(this.User);
            var rating = this.answerService.VoteUp(userId, answerId);
            return rating;
        }

        [HttpPost]
        public int VoteDown([FromForm] int answerId)
        {
            var userId = this.userService.GetUserId(this.User);
            var rating = this.answerService.VoteDown(userId, answerId);
            return rating;
        }
    }
}