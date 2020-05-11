namespace askLNU.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class ChangePhotoViewModel
    {
        public IFormFile Image { get; set; }
    }
}
