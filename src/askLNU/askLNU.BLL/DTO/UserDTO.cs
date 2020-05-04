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
        public UserDTO()
        {
        }

        public UserDTO(string userName, string name, string surname, int course, bool isBlocked, string imageSrc, string email)
        {
            UserName = userName;
            Name = name;
            Surname = surname;
            Course = course;
            IsBlocked = isBlocked;
            ImageSrc = imageSrc;
            Email = email;
        }

        public string Id { get; set; }
      
        public string UserName { get; set; }
        
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public int Course { get; set; }
        
        public int? FacultyId { get; set; }
        
        public bool IsBlocked { get; set; }
        
        public string ImageSrc { get; set; }
        
        public string Email { get; set; }
    }
}
