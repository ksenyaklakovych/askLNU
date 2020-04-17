using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askLNU.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace askLNU.Controllers
{
    public class CustomErrorController : Controller
    {
        private readonly string defaultErrorMessage = "Unknown error.";

        public IActionResult ShowError(string errorMessage)
        {
            var viewModel = new CustomErrorViewModel
            {
                ErrorMessage = errorMessage ?? defaultErrorMessage
            };

            return this.View(viewModel);
        }
    }
}