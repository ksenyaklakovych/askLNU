using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace askLNU.BLL.DTO
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}
