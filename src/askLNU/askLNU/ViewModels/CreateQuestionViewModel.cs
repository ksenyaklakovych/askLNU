using askLNU.BLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.ViewModels
{
    public class CreateQuestionViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public List<FacultyDTO> Faculties { get; set; }

        public List<TagDTO> Tags { get; set; }
    }
}
