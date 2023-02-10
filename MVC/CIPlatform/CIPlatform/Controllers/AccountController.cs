using Microsoft.AspNetCore.Mvc;

namespace CIPlatform.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult NewPassword()
        {
            return View();
        }

    }
}
