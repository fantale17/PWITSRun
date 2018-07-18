using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ITSSelfRunning.Services
{
    public interface ICloudStorageService
    {
        Task<String> UploadPhotoFromB64(string photo);
        Task<String> UploadProfilePhoto(IFormFile photo);
    }
}
