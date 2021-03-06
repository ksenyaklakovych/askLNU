﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.DAL.Entities
{
    public class Question
    {
        public Question()
        {

        }

        public Question(string applicationUserId, string title, string text, DateTime date)
        {
            ApplicationUserId = applicationUserId;
            Title = title;
            Text = text;
            Date = date;
        }

        [Key]
        public int Id { get; set; }
        
        [Required]
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public int Rating { get; set; }

        public bool IsSolved { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int? FacultyId { get; set; }
       
        public Faculty Faculty { get; set; }

        public virtual ICollection<ApplicationUserFavoriteQuestion> ApplicationUserFavoriteQuestions { get; set; }
        
        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
        
        public virtual ICollection<Answer> Answers { get; set; }
        
        public virtual ICollection<QuestionVote> QuestionVotes { get; set; }
    }
}
