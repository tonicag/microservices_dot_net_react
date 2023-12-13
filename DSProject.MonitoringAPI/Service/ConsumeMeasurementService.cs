
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
using Microsoft.EntityFrameworkCore;
namespace DSProject.MonitoringAPI.Service
{
    public class ConsumeMeasurementService : BackgroundService
    {

        private IConnection _connection;
        private IChannel _channel;
        private readonly ILogger _logger;
        private readonly RabbitMqConfiguration _rabbitMqConfiguration;
        private readonly IServiceProvider _serviceProvider;

        public ConsumeMeasurementService(IOptions<RabbitMqConfiguration> rabbitMqConfiguration,
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
            _channel.QueueDeclare("monitoring.measurements", false, false, false, null);
            _channel.QueueBind("monitoring.measurements", "monitoring", "monitoring.measurements.*", null);
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
                var measurement = JsonConvert.DeserializeObject<MeasurementDto>(message);

                if (measurement == null) return;
                using var scope = _serviceProvider.CreateScope();
                var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _socketManger = scope.ServiceProvider.GetRequiredService<SocketsManager>();

                var device = _db.Devices.FirstOrDefault((device) => device.device_id == measurement.Device_id);
                _logger.LogInformation($"received {message}");

                if (device == null) return;

                var userId = device.userId;
                DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(measurement.Timestamp).DateTime;

                _db.Measurements.Add(new Measurement()
                {
                    Device = device,
                    Timestamp = dateTime,
                    Value = measurement.Value
                }); ;
                _db.SaveChanges();

                if (userId == null) return;

                //await _socketManger.SendAsync(userId, $"Alert! {message}");

                var lastMeasurements = _db.Measurements
                    .Where(m => m.Device.device_id == measurement.Device_id)
                    .OrderByDescending(m => m.Timestamp)
                    .Take(7);
                double firstValue = lastMeasurements.Last().Value;
                double lastValue = lastMeasurements.First().Value;

                if (lastValue - firstValue > device.MaximumHourlyConsumption)
                {
                    await _socketManger.SendAsync(userId, new AlertDto { DeviceID = measurement.Device_id, Message = "The hourly maximum consumption has been passed!" });
                }


            };
            _channel.BasicConsume(queue: "monitoring.measurements",
                                 autoAck: true,
                                 consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
