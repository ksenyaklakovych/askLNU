using System;
using System.Collections.Generic;
using System.Text;

namespace askLNU.BLL.DTO
{
    public class VoteDTO
    {
        public string UserId { get; set; }
        public int QuestionId { get; set; }
        public bool VotedUp { get; set; }
        public bool VotedDown { get; set; }
    }
}
