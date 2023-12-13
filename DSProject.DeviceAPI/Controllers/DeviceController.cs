using DSProject.DeviceAPI.Model;
using DSProject.DeviceAPI.Model.Dto;
using DSProject.DeviceAPI.Models.Dto;
using DSProject.DeviceAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using UsersAPI.Data;
using UsersAPI.Models.Dto;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Threading.Channels;
using Microsoft.Extensions.Options;
using DSProject.AuthApi.Service;

namespace DSProject.DeviceAPI.Controllers
{
    [Route("api/device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        protected readonly AppDbContext _db;
        private ResponseDto _response;
        private IAuthService _authService;
        private readonly RabbitMqConnection _rabbitConnection;
        public DeviceController(AppDbContext db, IAuthService authService, RabbitMqConnection rabbitConnection)
        {
            _db = db;
            _authService = authService;
            _rabbitConnection = rabbitConnection;
            _response = new ResponseDto();
        }
        [HttpPost]
        public ResponseDto createDevice([FromBody] DeviceDto deviceRequest)
        {
            try
            {
                Device device = new Device()
                {
                    Address = deviceRequest.Address,
                    Description = deviceRequest.Description,
                    MaximumHourlyEnergyConsumption = deviceRequest.MaximumHourlyEnergyConsumption,
                    UserId = deviceRequest.UserId,
                };
                var addedDevice = _db.Devices.Add(device);
                _db.SaveChanges();

                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(addedDevice.Entity));
                _rabbitConnection.channel.BasicPublish("monitoring", "monitoring.devices.add", messageBodyBytes);

                _response.IsSuccess = true;
                _response.Result = device;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("{deviceId:Guid}")]
        public ResponseDto getDeviceById(Guid deviceId)
        {
            try
            {
                var entities = _db.Devices.Where(d => d.Id == deviceId);
                _response.Result = entities;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
        [HttpGet("GetAllAvailableDevices")]
        //[Authorize(Roles ="ADMIN")]
        public async Task<ResponseDto> getAllAvailableDevices()
        {
            try
            {
                var entities = _db.Devices.Where((item) => item.UserId == null && item.User == null).ToList().Select((item) =>
                {
                    return new DeviceDto
                    {
                        Address = item.Address,
                        Description = item.Description,
                        Id = item.Id,
                        MaximumHourlyEnergyConsumption = item.MaximumHourlyEnergyConsumption,
                        UserId = item.UserId,
                    };
                });
                _response.Result = entities;
                _response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
        [HttpGet("GetAllDevices")]
        public async Task<ResponseDto> getAllDevices([FromQuery] Guid? userId)
        {
            try
            {
                if (userId != null)
                {
                    var user = await _authService.GetUserById((Guid)userId);
                    var devices = _db.Devices.Where(d => d.User.UserId == userId);
                    _response.Result = devices;
                    _response.IsSuccess = true;
                    return _response;
                }

                var entities = _db.Devices.ToList().Select((item) =>
                {
                    return new DeviceDto
                    {
                        Address = item.Address,
                        Description = item.Description,
                        Id = item.Id,
                        MaximumHourlyEnergyConsumption = item.MaximumHourlyEnergyConsumption,
                        UserId = item.UserId,
                    };
                });
                _response.Result = entities;
                _response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
        [HttpPut]
        public ResponseDto updateDevice([FromBody] DeviceDto deviceRequest)
        {
            try
            {
                var d = _db.Devices.FirstOrDefault(d => d.Id == deviceRequest.Id);
                Device device = new Device()
                {
                    Id = deviceRequest.Id,
                    Address = deviceRequest.Address,
                    Description = deviceRequest.Description,
                    MaximumHourlyEnergyConsumption = deviceRequest.MaximumHourlyEnergyConsumption,
                    UserId = d.UserId
                };
                _db.Update(device);
                _db.SaveChanges();

                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(deviceRequest));
                _rabbitConnection.channel.BasicPublish("monitoring", "monitoring.devices.add", messageBodyBytes);
                _response.Result = device;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
        [HttpDelete]
        [Route("{deviceId:Guid}")]
        public ResponseDto deleteDevice(Guid deviceId)
        {
            try
            {
                var entity = _db.Devices.First(d => d.Id == deviceId);
                _db.Devices.Remove(entity);
                _db.SaveChanges();
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {

            }

            return _response;
        }
        [HttpPost("AddUserEntry/{userId:Guid}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto AddUserEntry(Guid userId)
        {
            try
            {
                var alreadyExists = _db.UserMappings.FirstOrDefault(u => u.UserId == userId);
                if (alreadyExists != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Already exists!";
                    return _response;
                }
                var userMapping = new UserMapping() { UserId = userId };
                _db.UserMappings.Add(userMapping);
                _db.SaveChanges();
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpDelete("DeleteUserEntry/{userId:Guid}")]
        //[Authorize(Roles = "ADMIN")]
        public ResponseDto DeleteUserEntry(Guid userId)
        {
            try
            {
                var userMapping = _db.UserMappings.FirstOrDefault(u => u.UserId == userId);
                if (userMapping == null)
                {
                    _response.IsSuccess = false;
                    return _response;
                }
                foreach (var device in _db.Devices.Where(d => d.User.UserId == userId).ToList())
                {
                    _db.Devices.Remove(device);
                }
                _db.UserMappings.Remove(userMapping);
                _db.SaveChanges();
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPost("AddDeviceMapping")]
        // [Authorize(Roles = "ADMIN")]
        public ResponseDto AddDeviceMapping([FromBody] MappingRequestDto mapping)
        {
            try
            {
                var user = _db.UserMappings.FirstOrDefault((map) => map.UserId == mapping.userId);
                var device = _db.Devices.FirstOrDefault((d) => d.Id == mapping.deviceId);
                if (user == null || device == null || device.UserId != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Can't add mapping";
                    return _response;
                }
                device.User = user;
                device.UserId = mapping.userId;
                _db.Update(device);
                _db.SaveChanges();
                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new DeviceDto()
                {
                    Address = device.Address,
                    Description = device.Description,
                    MaximumHourlyEnergyConsumption = device.MaximumHourlyEnergyConsumption,
                    Id = mapping.deviceId,
                    UserId = device.User.UserId
                }));
                _rabbitConnection.channel.BasicPublish("monitoring", "monitoring.devices.add", messageBodyBytes);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Can't add mapping";

            }
            return _response;
        }
        [HttpPost("DeleteDeviceMapping")]
        // [Authorize(Roles = "ADMIN")]
        public ResponseDto DeleteDeviceMapping([FromBody] MappingRequestDto mapping)
        {
            try
            {
                var user = _db.UserMappings.FirstOrDefault((map) => map.UserId == mapping.userId);
                var device = _db.Devices.First((d) => d.Id == mapping.deviceId);
                if (user == null || device == null || device.UserId == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Can't delete mapping";
                    return _response;
                }
                device.User = null;
                device.UserId = null;
                _db.Update(device);
                _db.SaveChanges();
                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new DeviceDto()
                {
                    Address = device.Address,
                    Description = device.Description,
                    MaximumHourlyEnergyConsumption = device.MaximumHourlyEnergyConsumption,
                    Id = mapping.deviceId,
                    UserId = device.User?.UserId
                }));
                _rabbitConnection.channel.BasicPublish("monitoring", "monitoring.devices.add", messageBodyBytes);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Can't delete mapping";

            }
            return _response;
        }


    }
}
