using Microsoft.AspNetCore.Mvc;

namespace Web.Chat.Api.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public Task<IActionResult> Index()
        {
            return Task.FromResult<IActionResult>(Ok());
        }
    }
}