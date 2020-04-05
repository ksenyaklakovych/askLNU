using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.ViewModels
{
    public class AnswerViewModel
    {
        public string Text { get; set; }
        public int Rating { get; set; }
        public bool IsSolution { get; set; }
        public DateTime Date { get; set; }
        public string AuthorName { get; set; }
    }
}
