using CsvHelper;
using CsvQueueProcessor.Core.Entities;
using CsvQueueProcessor.Core.Interfaces;
using CsvQueueProcessor.Core.Services;
using RabbitMQ.Client;
using System.Globalization;
using System.Threading.Tasks;
using static CsvQueueProcessor.Helper.Enums;

namespace CsvQueueProcessor.Helper
{
    public class MessageQueuePublisher
    {
        private readonly IRabbitMqService _rabbitMqService;
        public MessageQueuePublisher(IRabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        public async Task ProcessCsvAndSendToRabbitMq(string csvFilePath, IFileProcessingService fileProcessingService)
        {
            // Insert initial status into the database
            var processingStatus = new FileProcessing
            {
                FileName = Path.GetFileName(csvFilePath),
                StatusCode = (int)StatusCode.InProgress,
                ProcessedDate = DateTime.Now
            };

            int fileId = 0;

            try
            {
                // Insert the initial processing status into the database
                fileId = await fileProcessingService.InsertFileProcessingStatus(processingStatus);

                using (var reader = new StreamReader(csvFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var products = csv.GetRecords<Product>();
                    await Parallel.ForEachAsync(products, async (product, cancellationToken) =>
                    {
                        try
                        {
                            // Send each product to RabbitMQ
                            await Task.Run(() =>
                            {
                                _rabbitMqService.PublishMessage("product_queue", product, fileId);
                            });
                        }
                        catch (Exception ex)
                        {
                            // Log individual message processing error
                            Console.WriteLine($"Error sending message for product {product.Name}: {ex.Message}");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                // Log the error and update status to errored
                Console.WriteLine($"Error processing CSV file {csvFilePath}: {ex.Message}");
                processingStatus.StatusCode = (int)StatusCode.Failed;
            }
        }
    }
}
