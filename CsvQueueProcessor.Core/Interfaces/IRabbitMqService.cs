using CsvQueueProcessor.Core.Entities;
using RabbitMQ.Client;

namespace CsvQueueProcessor.Core.Interfaces
{
    public interface IRabbitMqService
    {
        IConnection GetConnection();
        void PublishMessage(string queueName, Product product, int fileId);
    }
}
