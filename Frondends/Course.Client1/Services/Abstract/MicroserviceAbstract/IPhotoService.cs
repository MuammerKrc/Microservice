using Course.Client1.Models.PhotoModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Services.Abstract.MicroserviceAbstract
{
    public interface IPhotoService
    {
        Task<Boolean> DeletePhoto(string photoUrl);
        Task<PhotoViewModel> UploadPhoto(IFormFile photo);
    }
}
