using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.BLL.DTO
{
    public class LabelDTO
    {
        public int Id { get; set; }
       
        public string Text { get; set; }
        
        public string Color { get; set; }
    }
}
