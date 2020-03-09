using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        [Required]
        public string Text { get; set; }

        public int Rating { get; set; }

        public bool IsSolution { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
