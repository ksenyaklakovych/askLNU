using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.ViewModels
{
    public class QuestionViewModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public bool IsSolved { get; set; }
        public DateTime Date { get; set; }
    }
}
