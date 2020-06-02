
namespace askLNU.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using askLNU.BLL.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class AnswerController : Controller
    {
        private readonly IUserService userService;
        private readonly IAnswerService answerService;
        private readonly ILogger<AnswerController> logger;

        public AnswerController(
            IUserService userService,
            IAnswerService answerService,
            ILogger<AnswerController> logger
            )
        {
            this.userService = userService;
            this.answerService = answerService;
            this.logger = logger;
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

        [Authorize(Roles = "Moderator,Admin")]
        public ActionResult Delete([FromForm] int answerId, [FromForm] int questionId)
        {
            this.logger.LogInformation($"Moderator deleted answer with id {answerId}.");
            this.answerService.Dispose(answerId);
            return this.RedirectToAction("Details", "Question", new { id = questionId });
        }
    }
}