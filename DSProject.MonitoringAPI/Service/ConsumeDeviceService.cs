
using Microsoft.AspNetCore.Connections;
using System.Threading.Channels;
using RabbitMQ.Client;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Configuration;
using DSProject.MonitoringAPI.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using DSProject.MonitoringAPI.Model.Dto;
using DSProject.MonitoringAPI.Models.Dto;
namespace DSProject.MonitoringAPI.Service
{
    public class ConsumeDeviceService : BackgroundService
    {

        private IConnection _connection;
        private IChannel _channel;
        private readonly ILogger _logger;
        private readonly RabbitMqConfiguration _rabbitMqConfiguration;
        private readonly IServiceProvider _serviceProvider;

        public ConsumeDeviceService(IOptions<RabbitMqConfiguration> rabbitMqConfiguration,
            ILogger<ConsumeMeasurementService> logger,
            IServiceProvider serviceProvider)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration.Value;
            _logger = logger;
            _serviceProvider = serviceProvider;
            InitRabbitMQ();
        }
        private void InitRabbitMQ()
        {
            var url = _rabbitMqConfiguration.ApplicationUrl;

            var factory = new ConnectionFactory
            {
                Uri = new Uri(url)
            };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateChannel();

            _channel.ExchangeDeclare("monitoring", ExchangeType.Topic);
            _channel.QueueDeclare("monitoring.devices", false, false, false, null);
            _channel.QueueBind("monitoring.devices", "monitoring", "monitoring.devices.*", null);
            _channel.BasicQos(0, 1, false);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var receivedDevice = JsonConvert.DeserializeObject<DeviceDto>(message);

                if (receivedDevice == null) return;
                using var scope = _serviceProvider.CreateScope();
                var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var device = _db.Devices.FirstOrDefault((device) => device.device_id == receivedDevice.Id);

                _logger.LogInformation($"received {message}");
                if (device == null)
                {

                    _db.Devices.Add(new Device
                    {
                        device_id = receivedDevice.Id,
                        LastMeasurement = 0,
                        MaximumHourlyConsumption = receivedDevice.MaximumHourlyEnergyConsumption,
                        userId = receivedDevice.UserId
                    });
                    _db.SaveChanges();
                }
                else
                {
                    device.userId = receivedDevice.UserId;
                    device.MaximumHourlyConsumption = receivedDevice.MaximumHourlyEnergyConsumption;
                    _db.SaveChanges();
                }

            };
            _channel.BasicConsume(queue: "monitoring.devices",
                                 autoAck: true,
                                 consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
