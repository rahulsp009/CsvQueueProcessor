using CsvQueueProcessor.Core.Configuration;
using CsvQueueProcessor.Core.Entities;
using CsvQueueProcessor.Core.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvQueueProcessor.Core.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;
        public RabbitMqService(RabbitMqConfiguration config)
        {
            var factory = new ConnectionFactory()
            {
                HostName = config.HostName,
                UserName = config.Username,
                Password = config.Password
            };

            _connection = factory.CreateConnection();
        }
        public IConnection GetConnection()
        {
            return _connection;
        }

        public void PublishMessage(string queueName, Product product, int fileId)
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var message = $"{fileId},{product.Name},{product.Category},{product.Price},{product.StockQuantity}";
            var body = System.Text.Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }
}
