using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace askLNU.DAL.Entities
{
    public class AnswerVote
    {
        [Key]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Key]
        public int AnswerId { get; set; }

        public Answer Answer { get; set; }

        public bool VotedUp { get; set; }

        public bool VotedDown { get; set; }
    }
}
