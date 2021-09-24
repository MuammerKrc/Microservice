using Course.Client1.ConfigurationOptions.Abstract;
using Course.Client1.Helpers;
using Course.Client1.Models.CatalogModels;
using Course.Client1.Services.Abstract.MicroserviceAbstract;
using Course.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Client1.Services.Mİcroservices
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient client;
        private readonly IPhotoService photoService;
        private readonly PhotoHelper photoHelper;

        public CatalogService(HttpClient httpClient,IPhotoService photoService, PhotoHelper photoHelper)
        {
            this.client = httpClient;
            this.photoService = photoService;
            this.photoHelper = photoHelper;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var resultPhotoService = await photoService.UploadPhoto(courseCreateInput.PhotoFile);
            if(resultPhotoService!=null)
            {
                courseCreateInput.PictureUrl = resultPhotoService.Url;
            }
            var response = await client.PostAsJsonAsync<CourseCreateInput>("courses", courseCreateInput);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourse(string courseId)
        {
            var response = await client.DeleteAsync($"courses/{courseId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await client.GetAsync("categories");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

            return content.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {

            var response = await client.GetAsync("courses");
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content=await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            content.Data.ForEach(i => i.PictureUrl = photoHelper.GetPhotoStockUri(i.PictureUrl));
            return content.Data;

        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {

            var response = await client.GetAsync($"courses/ByUserId/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            content.Data.ForEach(i => i.PictureUrl = photoHelper.GetPhotoStockUri(i.PictureUrl));
            return content.Data;
        }

        public async Task<CourseViewModel> GetByCourseId(string courseId)
        {
            var response = await client.GetAsync($"courses/{courseId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();

            return content.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var response = await client.PutAsJsonAsync<CourseUpdateInput>("courses", courseUpdateInput);
            return response.IsSuccessStatusCode;
        }
    }
}
