using DSProject.AuthAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.Models.Dto;
using UsersAPI.Service.IService;

namespace UsersAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new();
        }

        [HttpPost("register")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            var errorMessage = await _authService.Register(registrationRequestDTO);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = errorMessage;
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loggedUser = await _authService.Login(loginRequestDto);

            if (loggedUser.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or password incorrect";
                return BadRequest(_responseDto);
            }

            _responseDto.Result = loggedUser;
            _responseDto.IsSuccess = true;
            return Ok(_responseDto);
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role!.ToUpper());

            if (!assignRoleSuccessful)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error encountered";
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
        [HttpGet()]
        [Route("{userId:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _authService.GetUserById(userId);
            if (user != null)
            {
                _responseDto.IsSuccess = true;
                _responseDto.Result = user;
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "This user does not exist!";
            return BadRequest(_responseDto);
        }
        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("{userId:Guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var user = await _authService.GetUserById(userId);
            if (user != null)
            {
                await _authService.DeleteUser(userId);
                _responseDto.IsSuccess = true;
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "This user does not exist!";
            return BadRequest(_responseDto);
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateUser(UpdateRequest request)
        {
            var response = await _authService.Update(request);
            if (response != "")
            {
                _responseDto.IsSuccess = true;
                return Ok(_responseDto);
            }
            _responseDto.IsSuccess = false;
            _responseDto.Message = "This user does not exist!";
            return BadRequest(_responseDto);
        }
        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> GetAllUsers()
        {
            try
            {
                var users = await _authService.GetAllUsers();
                _responseDto.Result = users;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Something went wrong!";
            }
            return _responseDto;
        }
    }
}
