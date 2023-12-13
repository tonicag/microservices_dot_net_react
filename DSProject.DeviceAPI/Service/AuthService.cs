using DSProject.DeviceAPI.Service.IService;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using UsersAPI.Models.Dto;

namespace DSProject.DeviceAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory; 
        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<UserDto?> GetUserById(Guid id)
        {
            var client = _httpClientFactory.CreateClient("Auth");
            var response = await client.GetAsync($"/api/auth/{id}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if(resp!=null && resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<UserDto>(Convert.ToString(resp.Result));
            }
            return null;
        }
    }
}
