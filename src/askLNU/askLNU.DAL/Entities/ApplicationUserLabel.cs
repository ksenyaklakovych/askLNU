using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.DAL.Entities
{
    public class ApplicationUserLabel
    {
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }

        public int LabelId { get; set; }
        
        public Label Label { get; set; }
    }
}
