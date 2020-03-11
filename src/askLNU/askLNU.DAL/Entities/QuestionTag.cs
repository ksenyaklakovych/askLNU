using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.DAL.Entities
{
    public class QuestionTag
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
