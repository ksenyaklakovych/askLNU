using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.DAL.Entities
{
    public class Label
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string Color { get; set; }

        public virtual ICollection<ApplicationUserLabel> ApplicationUserLabels { get; set; }
    }
}
