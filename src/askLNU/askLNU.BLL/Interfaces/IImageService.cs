using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace askLNU.BLL.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImage(IFormFile file);
    }
}
