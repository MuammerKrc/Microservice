using Course.Client1.Models;
using Course.Client1.Services.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Client1.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserViewModel> GetUser()
        {
            //kısaltılmış hali
            //await _httpClient.GetFromJsonAsync<UserViewModel>("api/users");
            var response = await _httpClient.GetAsync("/api/users");
            var content=await response.Content.ReadAsStringAsync();
            UserViewModel userViewModel=JsonConvert.DeserializeObject<UserViewModel>(content);
            return userViewModel;
        }
    }
}
