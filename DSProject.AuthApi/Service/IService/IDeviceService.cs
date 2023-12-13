using UsersAPI.Models.Dto;

namespace DSProject.AuthAPI.Service.IService
{
    public interface IDeviceService
    {
        public Task<bool> AddUserEntry(Guid userId);
        public Task<bool> DeleteUserEntry(Guid userId);
    }
}
