﻿namespace askLNU.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class QuestionShortViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Rating { get; set; }

        public bool IsSolved { get; set; }

        public DateTime Date { get; set; }

        public int? FacultyId { get; set; }

        public bool IsFavorite { get; set; }

        public int? NumberOfAnswers { get; set; }

        public List<string> Tags { get; set; }
    }
}
