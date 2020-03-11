using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.DAL.Entities
{
    public class ApplicationUserFavoriteQuestion
    {
        [Key]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Key]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
