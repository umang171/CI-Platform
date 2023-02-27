
using CIPlatform.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using CIPlatform.Entities.ViewModels;

namespace CIPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel obj)
        {
            string emailId = obj.EmailId;
            string password = obj.Password;
            Boolean isValidEmail=_userRepository.validateEmail(emailId);
            if (!isValidEmail)
            {
                ModelState.AddModelError("EmailId", "Email not found");
            }
            else
            {
                Boolean isValidUser = _userRepository.validateUser(emailId, password);
                if (!isValidUser)
                {
                    ModelState.AddModelError("Password", "Password does not match");
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("Mission_Volunteer", "Mission");
            }
            return Login();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ForgotPassword(ForgotPasswordModel obj)
        {
            if(!_userRepository.validateEmail(obj.EmailId))
            {
                ModelState.AddModelError("EmailId","Email not found");
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("NewPassword");
            }
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
