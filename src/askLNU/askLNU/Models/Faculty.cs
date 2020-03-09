using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.Models
{
    public class Faculty
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }        
        public virtual ICollection<Question> Questions { get; set; }
    }
}
