using CsvQueueProcessor.Core.Interfaces;
using CsvQueueProcessor.Helper;
using Microsoft.AspNetCore.Mvc;

namespace CsvQueueProcessor.Controllers
{
    public class FileManagementController : Controller
    {
        private readonly IFileProcessingService _fileProcessingService;
        private readonly IRabbitMqService _rabbitMqService;

        public FileManagementController(IFileProcessingService fileProcessingService, IRabbitMqService rabbitMqService)
        {
            _fileProcessingService = fileProcessingService;
            _rabbitMqService = rabbitMqService;
        }
        public async Task<IActionResult> Index()
        {
            var fileProcessingStatuses = await _fileProcessingService.GetAllFileProcessingStatuses();
            return View(fileProcessingStatuses);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProducts(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var filePath = Path.Combine(Path.GetTempPath(), file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            MessageQueuePublisher messageQueuePublisher = new MessageQueuePublisher(_rabbitMqService);

            await messageQueuePublisher.ProcessCsvAndSendToRabbitMq(filePath, _fileProcessingService);

            return Ok("Products uploaded and sent to RabbitMQ.");
        }
    }
}
