using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace askLNU.DAL.Entities
{
    public class QuestionVote
    {
        [Key]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Key]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public bool VotedUp { get; set; }
        public bool VotedDown { get; set; }
    }
}
