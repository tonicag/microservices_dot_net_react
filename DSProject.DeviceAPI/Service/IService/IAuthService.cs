using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using UsersAPI.Models.Dto;

namespace DSProject.DeviceAPI.Service.IService
{
    public interface IAuthService
    {
        Task<UserDto> GetUserById(Guid id);
    }
}
