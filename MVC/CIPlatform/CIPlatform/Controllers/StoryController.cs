using Microsoft.AspNetCore.Mvc;

namespace CIPlatform.Controllers
{
    public class StoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
