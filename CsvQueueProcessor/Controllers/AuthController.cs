using Microsoft.AspNetCore.Mvc;

namespace CsvQueueProcessor.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
