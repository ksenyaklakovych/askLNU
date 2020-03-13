using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.Models
{
    public class AnswerViewModel
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public bool IsSolution { get; set; }
        public DateTime Date { get; set; }
    }
}
