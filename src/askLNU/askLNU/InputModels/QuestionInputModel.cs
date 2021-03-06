﻿namespace askLNU.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class QuestionInputModel
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int? FacultyId { get; set; }

        public string[] Tags { get; set; }
    }
}
