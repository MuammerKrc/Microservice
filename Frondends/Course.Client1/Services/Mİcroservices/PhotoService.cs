using Course.Client1.Models.PhotoModels;
using Course.Client1.Services.Abstract.MicroserviceAbstract;
using Course.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Client1.Services.Mİcroservices
{
    public class PhotoService : IPhotoService
    {
        private readonly HttpClient client;

        public PhotoService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<bool> DeletePhoto(string photoUrl)
        {
            var response = await client.DeleteAsync($"photos/{photoUrl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> UploadPhoto(IFormFile photo)
        {
            if(photo==null&&photo.Length<=0)
            {
                return null;
            }

            var RandomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";
            using var ms = new MemoryStream();
            await photo.CopyToAsync(ms);
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new ByteArrayContent(ms.ToArray()), "file", RandomFileName);

            var response = await client.PostAsync("photos", multipartContent);
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }
           var result = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
            return result.Data;
        }
    }
}
