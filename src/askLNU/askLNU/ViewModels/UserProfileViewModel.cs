namespace askLNU.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class UserProfileViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Course { get; set; }

        public string ImageSrc { get; set; }

        public IFormFile Image { get; set; }
    }
}
