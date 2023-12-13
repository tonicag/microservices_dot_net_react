using DSProject.DeviceAPI.Model;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DSProject.AuthApi.Service
{
    public class RabbitMqConnection
    {
        public IConnection connection;
        public IChannel channel;
        private readonly RabbitMqConfiguration _rabbitMqConfiguration;
        public RabbitMqConnection(IOptions<RabbitMqConfiguration> rabbitMqConfiguration)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration.Value;
            InitRabbitMQ();
        }
        private void InitRabbitMQ()
        {
            var url = _rabbitMqConfiguration.ApplicationUrl;

            var factory = new ConnectionFactory
            {
                Uri = new Uri(url)
            };

            connection = factory.CreateConnection();

            channel = connection.CreateChannel();
            channel.ExchangeDeclare("monitoring", ExchangeType.Topic);
            channel.QueueDeclare("monitoring.devices", false, false, false, null);
            channel.QueueBind("monitoring.devices", "monitoring", "monitoring.devices.*", null);
            channel.BasicQos(0, 1, false);
        }
    }
}
