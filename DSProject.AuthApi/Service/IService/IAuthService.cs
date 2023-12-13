using DSProject.AuthAPI.Models.Dto;
using UsersAPI.Models.Dto;

namespace UsersAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDTO request);
        Task<string> Update(UpdateRequest request);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
        Task<UserDto> GetUserById(Guid id);
        Task<bool> DeleteUser(Guid id);
        Task<IEnumerable<UserDto>> GetAllUsers();
    }
}
