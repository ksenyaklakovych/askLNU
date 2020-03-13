using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.BLL.DTO
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Course { get; set; }
        public int? FacultyId { get; set; }
        public bool IsBlocked { get; set; }
        public string ImageSrc { get; set; }
    }
}
