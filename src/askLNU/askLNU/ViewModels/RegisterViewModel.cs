﻿namespace askLNU.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 4)]
        [Display(Name = "UserName")]

        public string UserName { get; set; }

        [StringLength(100, MinimumLength = 4)]
        [Display(Name = "Name")]

        public string Name { get; set; }

        [StringLength(100, MinimumLength = 4)]
        [Display(Name = "Surname")]

        public string Surname { get; set; }

        [Display(Name = "Course")]
        public int Course { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile Image { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
