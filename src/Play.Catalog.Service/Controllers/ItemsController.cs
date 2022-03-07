using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("[Items]")]
    public class ItemsController : Controller
    {
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(ILogger<ItemsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}