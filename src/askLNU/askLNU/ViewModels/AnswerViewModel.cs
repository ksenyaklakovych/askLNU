namespace askLNU.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AnswerViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int Rating { get; set; }

        public bool IsSolution { get; set; }

        public string Date { get; set; }

        public string AuthorName { get; set; }
    }
}
