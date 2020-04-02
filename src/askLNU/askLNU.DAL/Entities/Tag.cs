using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.DAL.Entities
{
    public class Tag
    {
        public Tag()
        {

        }

        public Tag(int id, string text)
        {
            Id = id;
            Text = text;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
    }
}
