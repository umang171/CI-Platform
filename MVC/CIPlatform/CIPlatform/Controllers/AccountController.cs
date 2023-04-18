
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMissionRepository _missionRepository;
        public AccountController(IUserRepository userRepository, IMissionRepository missionRepository, IHttpContextAccessor httpContextAccessor, IConfiguration _configuration, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            configuration = _configuration;
            _webHostEnvironment = webHostEnvironment;
            _missionRepository = missionRepository;
        }
        public IActionResult Login()
        {
            List<Banner> banners=_userRepository.GetBannners();
            LoginModel loginModel = new LoginModel();
            loginModel.banners = banners;
            return View(loginModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel obj)
        {
            string emailId = obj.EmailId;
            string password = obj.Password;
            Boolean isValidEmail = _userRepository.validateEmail(emailId);
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
                HttpContext.Session.SetString("useremail", emailId);
                return RedirectToAction("Index", "Mission");
            }
            List<Banner> banners = _userRepository.GetBannners();
            LoginModel loginModel = new LoginModel();
            loginModel.banners = banners;
            return View(loginModel);
        }
        public IActionResult ForgotPassword()
        {
            List<Banner> banners = _userRepository.GetBannners();
            ForgotPasswordModel forgotPasswordModel= new ForgotPasswordModel();
            forgotPasswordModel.banners = banners;
            return View(forgotPasswordModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ForgotPassword(ForgotPasswordModel obj)
        {
            List<Banner> banners = _userRepository.GetBannners();
            ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
            forgotPasswordModel.banners = banners;
            if (!_userRepository.validateEmail(obj.EmailId))
            {
                ModelState.AddModelError("EmailId", "Email not found");
            }
            if (ModelState.IsValid)
            {

                string uuid = Guid.NewGuid().ToString();
                ResetPassword resetPasswordObj = new ResetPassword();
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
                return View(forgotPasswordModel);
            }
            return View(forgotPasswordModel);
        }
        [SessionHelper]
        public IActionResult VolunteerTimesheet()
        {
            string userSessionEmailId = HttpContext.Session.GetString("useremail");
            User user = _userRepository.findUser(userSessionEmailId);
            VolunteerTimesheetViewModel volunteerTimesheetViewModel = new VolunteerTimesheetViewModel();
            volunteerTimesheetViewModel.headerViewModel.username = user.FirstName + " " + user.LastName;
            volunteerTimesheetViewModel.headerViewModel.avtar = user.Avatar;
            volunteerTimesheetViewModel.headerViewModel.userid = user.UserId;
            volunteerTimesheetViewModel.timeBasedMissions = _missionRepository.getTimeMissionsOfUser((int)user.UserId);
            volunteerTimesheetViewModel.goalBasedMissions = _missionRepository.getGoalMissionsOfUser((int)user.UserId);
            return View(volunteerTimesheetViewModel);
        }
        public IActionResult Register()
        {
            List<Banner> banners = _userRepository.GetBannners();
            RegistrationModel register= new RegistrationModel();
            register.banners = banners;
            return View(register);
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
            List<Banner> banners = _userRepository.GetBannners();
            RegistrationModel register = new RegistrationModel();
            register.banners = banners;
            return View(register);
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
            TimeSpan remainingTime = DateTime.Now - resetObj.CreatedAt;

            int hour = remainingTime.Hours;

            if (hour >= 4)
            {
                _userRepository.removeResetPasswordToekn(resetObj);
                return RedirectToAction("Login");
            }
            List<Banner> banners = _userRepository.GetBannners();
            NewPasswordModel newPasswordModel = new NewPasswordModel();
            newPasswordModel.banners = banners;
            newPasswordModel.token = token;
            
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
            List<Banner> banners = _userRepository.GetBannners();
            obj.banners = banners;
            return View(obj);
        }
        [SessionHelper]
        public IActionResult UserProfile()
        {
            string userSessionEmailId = HttpContext.Session.GetString("useremail");
            User user = _userRepository.findUser(userSessionEmailId);
            UserProfileModel userProfileModelObj = new UserProfileModel();
            userProfileModelObj.FirstName = user.FirstName;
            userProfileModelObj.LastName = user.LastName;
            userProfileModelObj.Avatar = user.Avatar;
            userProfileModelObj.Email = user.Email;
            userProfileModelObj.EmployeeId = user.EmployeeId;
            userProfileModelObj.Title = user.Title;
            userProfileModelObj.Department = user.Department;
            userProfileModelObj.ProfileText = user.ProfileText;
            userProfileModelObj.WhyIVolunteer = user.WhyIVolunteer;
            userProfileModelObj.CityId = user.CityId == null ? -1 : user.CityId;
            userProfileModelObj.CityName = userProfileModelObj.CityId != -1 ? _userRepository.getCityFromCityId((long)userProfileModelObj.CityId) : "";
            userProfileModelObj.CountryId = user.CountryId == null ? -1 : user.CountryId;
            userProfileModelObj.CountryName = userProfileModelObj.CountryId != -1 ? _userRepository.getCountryFromCountryId((long)userProfileModelObj.CountryId) : "";
            userProfileModelObj.CountryNames = _userRepository.getCountryNames();
            userProfileModelObj.CityNames = _userRepository.getCityNames((int)userProfileModelObj.CountryId);
            userProfileModelObj.LinkedInUrl = user.LinkedInUrl;
            userProfileModelObj.skills = _userRepository.getSkillNames();
            userProfileModelObj.userSkills = user.UserSkills.ToList();
            return View(userProfileModelObj);
        }
        public IActionResult getCityNames(int countryId)
        {
            List<City> cityNames = _userRepository.getCityNames(countryId);
            return Json(new { data = cityNames });
        }
        public IActionResult getSkillsOfUser(int userId)
        {
            List<UserSkill> userSkills = _userRepository.getSkillsOfUser(userId);
            return Json(new { data = userSkills });
        }
        [SessionHelper]
        [HttpPost]
        public IActionResult EditUserProfile(UserProfileModel userProfileModel, IFormFile? file)
        {
            string userSessionEmailId = HttpContext.Session.GetString("useremail");
            User user = _userRepository.findUser(userSessionEmailId);
            userProfileModel.UserId = user.UserId;
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\avatars");
                var extension = Path.GetExtension(file.FileName);
                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                userProfileModel.Avatar = @"\images\avatars\" + fileName + extension;
            }
            _userRepository.editUserProfile(userProfileModel);
            return RedirectToAction("UserProfile", "Account");
        }
        [SessionHelper]
        [HttpPost]
        public IActionResult ChangePasswod(UserProfileModel userProfileModel)
        {
            string userSessionEmailId = HttpContext.Session.GetString("useremail");
            User user = _userRepository.findUser(userSessionEmailId);
            if (!user.Password.Equals(userProfileModel.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "Password does not match");
                TempData["Error"] = "Password does not match";
            }
            else
            {
                if (userProfileModel.NewPassword.Equals(userProfileModel.ConfirmPassword))
                {
                    user.Password = userProfileModel.NewPassword;
                    _userRepository.updatePassword(user);

                }
                else
                {
                    ModelState.AddModelError("ConfirmPassword", "Confirm password does not match to new password");
                }
            }
            return RedirectToAction("UserProfile", "Account");
        }
        [SessionHelper]
        public IActionResult Policy()
        {
            string userSessionEmailId = HttpContext.Session.GetString("useremail");
            User user = _userRepository.findUser(userSessionEmailId);
            HeaderViewModel headerViewModel = new HeaderViewModel();
            headerViewModel.username = user.FirstName + " " + user.LastName;
            headerViewModel.avtar = user.Avatar;
            headerViewModel.userid = user.UserId;
            return View(headerViewModel);
        }
        public IActionResult contactUs(string contactName, string contactEmail, string contactSubject, string contactMessage)
        {
            string welcomeMessage = "Welcome to CI platform, <br/> We have an suggerstion from " + contactName + "(" + contactEmail + ") </br>";

            MailHelper mailHelper = new MailHelper(configuration);
            ViewBag.sendMail = mailHelper.Send("gohelumang12@gmail.com", welcomeMessage + "</br>" + contactMessage, contactSubject);


            return RedirectToAction("UserProfile", "Account");
        }
        public IActionResult addVolunteerTimesheet(VolunteerTimesheetRecordModel volunteerTimesheetRecordModel)
        {
            if (ModelState.IsValid)
            {

                Timesheet timesheet = new Timesheet();
                timesheet.UserId = volunteerTimesheetRecordModel.UserId;
                timesheet.MissionId = volunteerTimesheetRecordModel.MissionId;
                timesheet.DateVolunteered = volunteerTimesheetRecordModel.DateVolunteered;
                if ((volunteerTimesheetRecordModel.Hour == null || volunteerTimesheetRecordModel.Hour == "" || volunteerTimesheetRecordModel.Hour == "0") && (volunteerTimesheetRecordModel.Minutes == null || volunteerTimesheetRecordModel.Minutes == "" || volunteerTimesheetRecordModel.Minutes == "0"))
                {
                    timesheet.Time = null;
                }
                else
                {
                    string Time=volunteerTimesheetRecordModel.Hour+":"+volunteerTimesheetRecordModel.Minutes;
                    timesheet.Time = TimeOnly.Parse(Time);
                }
                timesheet.Action = volunteerTimesheetRecordModel.Action == -1?null: volunteerTimesheetRecordModel.Action;
                timesheet.Notes = volunteerTimesheetRecordModel.Notes;
                timesheet.Status = "applied";
                _userRepository.addVolunteerTimesheet(timesheet);
                return Json(new { status = 1 });
            }
            return Json(new {status=0});
        }
        public IActionResult editVolunteerTimesheet(VolunteerTimesheetRecordModel volunteerTimesheetRecordModel)
        {
            if (ModelState.IsValid)
            {
                _userRepository.editVolunteerTimesheet(volunteerTimesheetRecordModel);
                return Json(new { status = 1 });
            }
            return Json(new { status = 0 });
        }

        public IActionResult getVolunteerTimesheetRecordHourBased(int userId)
        {
            List<VolunteerTimesheetRecordModel> volunteerTimesheetRecordModels = _userRepository.getVolunteerTimesheetRecordHourBased(userId);
            return PartialView("_VolunteerTimesheetHourBased", volunteerTimesheetRecordModels);
        }
        public IActionResult getVolunteerTimesheetRecordGoalBased(int userId)
        {
            List<VolunteerTimesheetRecordModel> volunteerTimesheetRecordModels = _userRepository.getVolunteerTimesheetRecordGoalBased(userId);
            return PartialView("_VolunteerTimesheetGoalBased", volunteerTimesheetRecordModels);
        }
        public async Task<IActionResult> deleteVolunteerTimesheet(int timesheetId)
        {
            _userRepository.deleteVolunteerTimesheet(timesheetId);
            return Ok();
        }
        public IActionResult getEditVolunteerTimesheet(int timesheetId)
        {
            VolunteerTimesheetRecordModel volunteerTimesheetRecordModel = _userRepository.getEditVolunteerTimesheet(timesheetId);
            return Json(new { data = volunteerTimesheetRecordModel });
        }
        public string GetDatesOfMission(int missionId)
        {
            string dates=_userRepository.GetDatesOfMission(missionId);
            return dates;
        }        
        public IActionResult logout()
        {
            HttpContext.Session.Remove("useremail");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
