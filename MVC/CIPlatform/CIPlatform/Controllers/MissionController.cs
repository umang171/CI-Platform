using Microsoft.AspNetCore.Mvc;

namespace CIPlatform.Controllers
{
    public class MissionController : Controller
    {
        public IActionResult Mission_Volunteer()
        {
            return View();
        }
    }
}
