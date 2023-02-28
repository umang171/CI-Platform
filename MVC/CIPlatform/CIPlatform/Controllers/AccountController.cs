
using CIPlatform.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Entities.DataModels;
using MailKit.Net.Smtp;
using MimeKit;
using CIPlatform.Helpers;

namespace CIPlatform.Controllers
{
    public class AccountController : Controller
    {
        private int? tempId;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;
        public AccountController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IConfiguration _configuration)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            configuration = _configuration;
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
                int UserID = (int)userObj.UserId;
                string welcomeMessage = "Welcome to CI platform, <br/> You can Reset your password using below link. </br>";
                string path = "<a href=\"" + " https://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/Account/NewPassword/" + UserID.ToString() + " \"  style=\"font-weight:500;color:blue;\" > Reset Password </a>";
                MailHelper mailHelper = new MailHelper(configuration);
                ViewBag.sendMail = mailHelper.Send(obj.EmailId, welcomeMessage + path);
                

                //var message = new MimeMessage();
                //message.From.Add(new MailboxAddress("Manthan", "patelmanthan2000@gmail.com"));
                //message.To.Add(new MailboxAddress("Umang", "gohelumang12@gmail.com"));
                //message.Subject = "CI platform test message";
                //message.Body = new TextPart("plain")
                //{
                //    Text = "Puppy"
                //};
                //using (var client=new SmtpClient())
                //{
                //    client.Connect("smtp.gmail.com",587,false);
                //    client.Authenticate("patelmanthan2000@gmail.com", "Deeku@2631975");
                //    client.Send(message);
                //    client.Disconnect(true);
                //}

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
