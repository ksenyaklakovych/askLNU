namespace askLNU.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserForAdminViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        [Display(Name = "Has moderator rights")]
        public bool IsModerator { get; set; }
        public bool IsBlocked { get; set; }
    }
}
