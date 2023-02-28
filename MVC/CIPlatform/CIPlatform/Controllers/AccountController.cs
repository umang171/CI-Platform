
using CIPlatform.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Entities.DataModels;

namespace CIPlatform.Controllers
{
    public class AccountController : Controller
    {
        private int? tempId;
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
                var userObj=_userRepository.findUser(obj.EmailId);
                return RedirectToAction("NewPassword",new {id=userObj.UserId});
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegistrationModel obj)
        {
            if (_userRepository.validateEmail(obj.EmailId))
            {
                ModelState.AddModelError("EmailId", "Email already registred");
            }
            else
            {
                if (!obj.Password.Equals(obj.ConfirmPassword))
                { 
                    ModelState.AddModelError("ConfirmPassword", "Confirm password does not match to new password");
                }
            }
            if (ModelState.IsValid)
            {
                User user = new User();
                user.FirstName = obj.FirstName;
                user.LastName = obj.LastName;
                user.PhoneNumber = long.Parse(obj.PhoneNo);
                user.Email = obj.EmailId;
                user.Password = obj.Password;
                _userRepository.addUser(user);                
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult NewPassword(int? id)
        {
            tempId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewPassword(NewPasswordModel obj,int? id)
        {
            if (id != null)
            {
                if (obj.NewPassword.Equals(obj.ConfirmPassword))
                {
                    var userObj = _userRepository.findUser(id);
                    if (!obj.NewPassword.Equals(userObj.Password))
                    {
                        userObj.Password = obj.NewPassword;
                        _userRepository.updatePassword(userObj);
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("NewPassword", "You can not set Old password as New Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("ConfirmPassword", "Confirm password does not match to new password");
                }
            }
            return View();
        }



    }
}
