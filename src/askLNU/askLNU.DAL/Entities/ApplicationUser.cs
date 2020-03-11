﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        public string Surname { get; set; }

        [PersonalData]
        public int Course { get; set; }

        public int? FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public bool IsBlocked { get; set; }

        public string ImageSrc { get; set; }

        public virtual ICollection<ApplicationUserLabel> ApplicationUserLabels { get; set; }
        public virtual ICollection<ApplicationUserFavoriteQuestion> ApplicationUserFavoriteQuestions { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
