
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
                HttpContext.Session.SetString("useremail",emailId);
                return RedirectToAction("Index", "Mission");
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

                string uuid=Guid.NewGuid().ToString();
                ResetPassword resetPasswordObj=new ResetPassword();
                resetPasswordObj.Email = obj.EmailId;
                resetPasswordObj.Token = uuid;
                resetPasswordObj.CreatedAt = DateTime.Now;

                _userRepository.addResetPasswordToken(resetPasswordObj);

                var userObj = _userRepository.findUser(obj.EmailId);
                int UserID = (int)userObj.UserId;
                string welcomeMessage = "Welcome to CI platform, <br/> You can Reset your password using below link. </br>";
                string path = "<a href=\"" + " https://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/Account/NewPassword?token=" + uuid + " \"  style=\"font-weight:500;color:blue;\" > Reset Password </a>";
                MailHelper mailHelper = new MailHelper(configuration);
                ViewBag.sendMail = mailHelper.Send(obj.EmailId, welcomeMessage + path);
        

                return View();
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
                user.Avatar = "/images/user1.png";
                _userRepository.addUser(user);                
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult NewPassword(string? token)
        {
            ResetPassword resetObj;
            try
            {
                resetObj = _userRepository.findUserByToken(token);

                if (resetObj == null)
                {
                    throw new Exception("token not found");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }
            TimeSpan remainingTime = DateTime.Now-resetObj.CreatedAt;

            int hour=remainingTime.Hours;

            if (hour>=4)
            {
                _userRepository.removeResetPasswordToekn(resetObj);
                return RedirectToAction("Login");
            }
            NewPasswordModel newPasswordModel=new NewPasswordModel();
            newPasswordModel.token=token;
            return View(newPasswordModel);
        
        
        }

        [HttpPost]
        public IActionResult NewPassword(NewPasswordModel obj)
        {
            string token = obj.token;
            if (token != null)
            {
                if (obj.NewPassword.Equals(obj.ConfirmPassword))
                {

                    var resetObj = _userRepository.findUserByToken(token);
                    if (resetObj.Token == null)
                    {
                        return RedirectToAction("Login");
                    }
                    var userObj = _userRepository.findUser(resetObj.Email);
                    if (!obj.NewPassword.Equals(userObj.Password))
                    {
                        userObj.Password = obj.NewPassword;
                        _userRepository.updatePassword(userObj);
                        _userRepository.removeResetPasswordToekn(resetObj);
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
            return View(obj);
        }
        [SessionHelper]
        public IActionResult UserProfile()
        {
            string userSessionEmailId = HttpContext.Session.GetString("useremail");
            User user = _userRepository.findUser(userSessionEmailId);
            UserProfileModel userProfileModelObj=new UserProfileModel();
            userProfileModelObj.FirstName = user.FirstName;
            userProfileModelObj.LastName = user.LastName;
            userProfileModelObj.Avatar=user.Avatar;
            userProfileModelObj.EmployeeId = user.EmployeeId;
            userProfileModelObj.Title = user.Title;
            userProfileModelObj.Department = user.Department;
            userProfileModelObj.ProfileText = user.ProfileText;
            userProfileModelObj.WhyIVolunteer=user.WhyIVolunteer;
            userProfileModelObj.CityId = user.CityId == null ? -1:user.CityId ;
            userProfileModelObj.CityName = userProfileModelObj.CityId != -1?_userRepository.getCityFromCityId((long)userProfileModelObj.CityId):"";
            userProfileModelObj.CountryId = user.CountryId==null?-1:user.CountryId;
            userProfileModelObj.CountryName = userProfileModelObj.CountryId != -1 ? _userRepository.getCountryFromCountryId((long)userProfileModelObj.CountryId) : "";
            userProfileModelObj.CountryNames = _userRepository.getCountryNames();
            userProfileModelObj.LinkedInUrl = user.LinkedInUrl;
            userProfileModelObj.skills = _userRepository.getSkillNames();
            return View(userProfileModelObj);
        }
        public IActionResult logout()
        {
            HttpContext.Session.Remove("useremail");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
