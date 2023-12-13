using DSProject.AuthAPI.Models.Dto;
using DSProject.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.Models.Dto;
using UsersAPI.Service.IService;

namespace UsersAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IDeviceService _deviceService;

        public AuthService(AppDbContext appDbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IDeviceService deviceService,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            this._db = appDbContext;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._deviceService = deviceService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = _db.Users.Take(50).Select(item => new UserDto { Email = item.Email, ID = Guid.Parse(item.Id), Name = item.Name });
            return users;
        }

        async Task<bool> IAuthService.AssignRole(string email, string roleName)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == email.ToLower());

            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        async Task<bool> IAuthService.DeleteUser(Guid id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id.ToString());
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                await _deviceService.DeleteUserEntry(Guid.Parse(user.Id));
                return true;
            }
            return false;

        }

        async Task<UserDto> IAuthService.GetUserById(Guid id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id.ToString());
            if (user != null)
            {
                UserDto userDto = new()
                {
                    Email = user.Email,
                    ID = id,
                    Name = user.Name,
                };
                return userDto;
            }
            return null;

        }

        async Task<LoginResponseDto> IAuthService.Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.Username.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);
            UserDto userDto = new()
            {
                Email = user.Email,
                ID = Guid.Parse(user.Id),
                Name = user.Name
            };
            LoginResponseDto loginResponseDTO = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };
            return loginResponseDTO;
        }

        async Task<string> IAuthService.Update(UpdateRequest request)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == request.Id.ToString());
            if (user == null)
            {
                return "";
            }

            try
            {
                user.Email = request.Email;
                user.Name = request.Name;
                user.NormalizedEmail = request.Email.ToUpper();
                var result = await _userManager.UpdateAsync(user);
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, resetToken, request.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.Users.First(u => u.UserName == request.Email);
                    await _deviceService.AddUserEntry(Guid.Parse(userToReturn.Id));
                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = Guid.Parse(user.Id),
                        Name = userToReturn.Name

                    };
                    bool assignedRole = await ((IAuthService)this).AssignRole(user.Email, request.Role);
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                return "Error encountered";
            }
        }
        async Task<string> IAuthService.Register(RegistrationRequestDTO request)
        {
            ApplicationUser user = new()
            {
                UserName = request.Email,
                Email = request.Email,
                NormalizedEmail = request.Email.ToLower(),
                Name = request.Name
            };

            try
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.Users.First(u => u.UserName == request.Email);
                    await _deviceService.AddUserEntry(Guid.Parse(userToReturn.Id));
                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = Guid.Parse(user.Id),
                        Name = userToReturn.Name

                    };
                    bool assignedRole = await ((IAuthService)this).AssignRole(user.Email, request.Role);
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
