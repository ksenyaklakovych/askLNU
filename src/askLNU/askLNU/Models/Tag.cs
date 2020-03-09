using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
    }
}
