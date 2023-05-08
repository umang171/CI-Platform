using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Helpers;
using CIPlatform.Repository.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CIPlatform.Controllers
{
    [Authorize(Roles = "user,admin")]
    public class MissionController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMissionRepository _missionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;

        public MissionController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IMissionRepository missionRepository, IConfiguration _configuration)
        {
            _userRepository = userRepository;
            _missionRepository = missionRepository;
            _httpContextAccessor = httpContextAccessor;
            configuration = _configuration;
        }
        public IActionResult Index()
        {
            string userSessionEmailId=HttpContext.Session.GetString("useremail");
            
            MissionHomeModel missionHomeModel = new MissionHomeModel();
            User userObj = _userRepository.findUser(userSessionEmailId);
            missionHomeModel.username = userObj.FirstName+" "+userObj.LastName;
            missionHomeModel.userid=userObj.UserId;
            missionHomeModel.avtar=userObj.Avatar;
            missionHomeModel.cmsPages = _missionRepository.GetCMSPages();
            return View(missionHomeModel);
        }
        public IActionResult Mission_Volunteer(int? missionId)
        {
            string userSessionEmailId = HttpContext.Session.GetString("useremail");
            
            MissionVolunteerViewModel missionVolunteerViewModelObj= new MissionVolunteerViewModel();
            missionVolunteerViewModelObj= _missionRepository.getMissionFromMissionId((int)missionId);
            User userObj = _userRepository.findUser(userSessionEmailId);
            missionVolunteerViewModelObj.username = userObj.FirstName + " " + userObj.LastName;
            missionVolunteerViewModelObj.userid= (int)userObj.UserId;
            missionVolunteerViewModelObj.avtar=userObj.Avatar;
            missionVolunteerViewModelObj.cmsPages = _missionRepository.GetCMSPages();
            missionVolunteerViewModelObj.users = _userRepository.getUsers();
            return View(missionVolunteerViewModelObj);
        }
        
        public IActionResult GetCountries() => Json(new { data = _missionRepository.getCountries() });
        
        
        public IActionResult GetCites(string country)
        {
            IEnumerable<City> cities = _missionRepository.getCities(country);
            return Json(new { data = cities });
        }
        
        public IActionResult GetThemes()
        {
            IEnumerable<MissionTheme> missionThemes = _missionRepository.getThemes();
            string[] themeArr = missionThemes.Select(u => u.Title).ToArray();
            return Json(new { data = themeArr });
        }
        public IActionResult GetSkills()
        {
            IEnumerable<Skill> missionSkills= _missionRepository.getSkills();
            return Json(new { data = missionSkills.AsEnumerable() });
        }
        
        [HttpPost]
        public IActionResult getMissionsFromSP(string countryNames, string cityNames, string themeNames, string skillNames, string searchText, string sortValue, string exploreValue, int pageNumber,int userId)
        {
            // make explicit SQL Parameter
            PaginationMission pagination =_missionRepository.getMissionsFromSP(countryNames, cityNames,themeNames,skillNames,searchText,sortValue, exploreValue, pageNumber,userId) ;
           
            return PartialView("_MissionList", pagination);
        }

        public void addFavouriteMissions(string userId,string missionId)
        {
            FavouriteMission favouriteMissionObj=new FavouriteMission();
            favouriteMissionObj.UserId =Int64.Parse(userId);
            favouriteMissionObj.MissionId=Int64.Parse(missionId);
            _missionRepository.addFavouriteMission(favouriteMissionObj);
        }
        public void removeFavouriteMissions(string userId,string missionId)
        {
            FavouriteMission favouriteMissionObj = new FavouriteMission();
            favouriteMissionObj.UserId = Int64.Parse(userId);
            favouriteMissionObj.MissionId = Int64.Parse(missionId);
            FavouriteMission favouriteMission=_missionRepository.getFavouriteMission(favouriteMissionObj);
            _missionRepository.removeFavouriteMission(favouriteMission);
        }
        public IActionResult getFavouriteMissionsOfUser(int userid)
        {
            IEnumerable<FavouriteMission> favouriteMissions=_missionRepository.getFavouriteMissionsOfUser(userid);
            string arr = "";
            foreach(FavouriteMission favouriteMission in favouriteMissions)
            {
                arr += favouriteMission.MissionId+",";
            }
            return  Json(new { data=arr });
        }
        [HttpPost]
        public void addRatingStars(int userId, int missionId, int ratingStars)
        {
            _missionRepository.addRatingStars(userId, missionId, ratingStars);
        }
        public IActionResult getRelatedMissions(string themeName,string cityName, int missionId)
        {
            IEnumerable<Mission> relatedMissions=_missionRepository.getRelatedMissions(themeName, cityName,missionId);
            return PartialView("_relatedMission",relatedMissions);
        }
        [HttpPost]
        public void addComment(int userId, int missionId, string comment)
        {
            _missionRepository.addComment(userId, missionId, comment);           
        }
        
        public IActionResult getComments(int missionId)
        {
            IEnumerable<Comment> commentsObj=_missionRepository.getComments(missionId);
            return PartialView("_Comments", commentsObj);
        }
        
        public IActionResult recommendToCoworker(int fromUserId,int missinoId,string toUserEmail)
        {
            User userObj;
            try
            {
                userObj = _userRepository.findUser(toUserEmail);
            }
            catch (Exception ex)
            {
                return Json(new { data = "Email not found",status=0 });    
            }
            User fromUser=_userRepository.findUser(fromUserId);
            _missionRepository.recommendToCoworker(fromUserId,(int)userObj.UserId, missinoId);
            Notification notification=new Notification();            
            notification.NotificationMessage = fromUser.FirstName+"-Recommended this mission-"+ _missionRepository.getMissionFromMissionId(missinoId).Title;
            notification.NotificationType = "RecommendedMission";
            notification.Status = true;
            notification.MessageId = missinoId;
            notification.UserId =(int)userObj.UserId;
            notification.FromUserId = fromUserId;
            notification.NotificationImage = fromUser.Avatar;
            notification.CreatedAt=DateTime.Now;
            _missionRepository.addNotification(notification);

            string welcomeMessage = "Welcome to CI platform, <br/> You are recommended to </br>";
            string path = "<a href=\"" + " https://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/Mission/Mission_Volunteer?missionId="+missinoId + " \"   style=\"font-weight:500;color:blue;\" > Recommended mission </a>";
        
            MailHelper mailHelper = new MailHelper(configuration);
            mailHelper.Send(toUserEmail, welcomeMessage + path,"Recommended mission");

            return Json(new { data = "Email sent successfully",status=1 });
        }
        public void addToApplication(int missionId,int userId)
        {
               _missionRepository.addToApplication(missionId, userId);
        }
        
        public IActionResult getRecentVolunteers(int missionId)
        {
            IEnumerable<MissionApplication> missionApplicationObj=_missionRepository.getRecentVolunteers(missionId);
            return PartialView("_recentVolunteers",missionApplicationObj);
        }
        public IActionResult logout()
        {
            HttpContext.Session.Remove("useremail");
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        public IActionResult getAppliedMissionOfUser(int userId, int missionId)
        {
            string status = _missionRepository.getAppliedMissionOfUser(userId, missionId);
            return Json(new {status=status});
        }
        public IActionResult getRatingOfUser(int userId,int missionId)
        {            
            return Json(new { rating = _missionRepository.getRatingOfUserForMission(userId, missionId) });
        }
        public IActionResult GetCityCountryOfUser(long userId)
        {
            return Json(new { data = _userRepository.GetCityCountryOfUser(userId) });
        }
        public IActionResult CmsPageDetails(long cmsId)
        {
            CmsPageModel cmsPageModel = new CmsPageModel();
            string userSessionEmailId = HttpContext.Session.GetString("useremail");
            User userObj = _userRepository.findUser(userSessionEmailId);
            cmsPageModel.username = userObj.FirstName + " " + userObj.LastName;
            cmsPageModel.userid = (int)userObj.UserId;
            cmsPageModel.avtar = userObj.Avatar;
            cmsPageModel.cmsPages = _missionRepository.GetCMSPages();
            cmsPageModel.cmsPage= _missionRepository.GetCmsPageDetails(cmsId);

            return View(cmsPageModel);
        }
        public IActionResult GetNotifications(long userId,string selectedNotificationSettings)
        {
            List<Notification> notifications=_missionRepository.GetNotifications(userId, selectedNotificationSettings);
            return Json(notifications);
        } 
        public IActionResult ClearNotifications(long userId)
        {
           _missionRepository.ClearNotifications(userId);
            return Ok();
        }
        public IActionResult changeStatusNotification(long notificationId)
        {
            _missionRepository.changeStatusNotification(notificationId);
            return Ok();
        }
        public IActionResult GetNotificationSettings(long userId)
        {
            NotificationSetting notificationSetting =_missionRepository.GetNotificationSettings(userId);
            return Json(notificationSetting);
        }
        public IActionResult ChangeNotificationSettings(NotificationSetting notificationSetting)
        {
            _missionRepository.ChangeNotificationSettings(notificationSetting);
            return Ok();
        }
    }
}
