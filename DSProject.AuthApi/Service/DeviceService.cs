using DSProject.AuthAPI.Service.IService;
using Newtonsoft.Json;
using System.Text;
using UsersAPI.Models.Dto;

namespace DSProject.AuthAPI.Service
{
    public class DeviceService : IDeviceService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public DeviceService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> AddUserEntry(Guid userId)
        {
            var client = _httpClientFactory.CreateClient("Devices");
            var data = new StringContent("", Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/device/AddUserEntry/{userId}",data);
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp != null && resp.IsSuccess)
            {
                return true;
            }
            else return false;
        }

        public async Task<bool> DeleteUserEntry(Guid userId)
        {
            var client = _httpClientFactory.CreateClient("Devices");
            var response = await client.DeleteAsync($"/api/device/DeleteUserEntry/{userId}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp != null && resp.IsSuccess)
            {
                return true;
            }
            else return false;
        }
    }
}
