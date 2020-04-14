using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using askLNU.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace askLNU.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("Find")]
        public List<string> FindTags(string searchString)
        {
            if (searchString == null)
            {
                return new List<string>();
            }

            return _tagService.FindTags(searchString);
        }
    }
}