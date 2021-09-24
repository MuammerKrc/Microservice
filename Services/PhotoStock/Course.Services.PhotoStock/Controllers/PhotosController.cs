using Course.Services.PhotoStock.Dtos;
using Course.Shared.ControllerHelper;
using Course.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhotosController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile file, CancellationToken cancellationToken)
        {
            if(file!=null&& file.Length>0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", file.FileName);

                using(var stream =new FileStream(path,FileMode.Create))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                    var returnPath = file.FileName;
                    PhotoDto photoDto = new() { Url = returnPath };
                    return QQReturnObject(Course.Shared.Dtos.Response<PhotoDto>.Success(photoDto, 200));
                }
            }
            return QQReturnObject(Course.Shared.Dtos.Response<PhotoDto>.Fail("Fotoğraf kaydedilmedi", 400));
        }
        [HttpDelete]
        public IActionResult PhotoDelete(string photoUrl)
        {
            if(photoUrl==null)
            {
                return QQReturnObject(Course.Shared.Dtos.Response<NoContent>.Fail("Fotoğraf bulunamadı", 404));
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);

            if(!System.IO.File.Exists(path))
            {
                return QQReturnObject(Course.Shared.Dtos.Response<NoContent>.Fail("Fotoğraf bulunamadı", 404));
            }
            System.IO.File.Delete(path);
            return QQReturnObject(Course.Shared.Dtos.Response<NoContent>.Success(204));
        }
    }
}
