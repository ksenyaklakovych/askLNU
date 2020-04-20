using askLNU.BLL.Configs;
using askLNU.BLL.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace askLNU.BLL.Services
{
    public class ImageService : IImageService
    {
        private readonly Account cloudinaryAccount;
        
        public ImageService(IOptions<CloudinaryConfig> options)
        {
            cloudinaryAccount = new Account(
                options.Value.CloudName,
                options.Value.ApiKey,
                options.Value.ApiSecret
            );
        }

        public async Task<string> SaveImage(IFormFile file)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            var cloudinary = new Cloudinary(cloudinaryAccount);
            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUri.ToString();
        }
    }
}
