using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using CIPlatform.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CIPlatform.Controllers
{
    [Authorize(Roles = "user,admin")]
    public class StoryController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly IMissionRepository _missionRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;

        public StoryController(IUserRepository userRepository, IMissionRepository missionRepository,IStoryRepository storyRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor, IConfiguration _configuration) 
        {
            _userRepository = userRepository;
            _missionRepository = missionRepository;
            _storyRepository = storyRepository;
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            configuration = _configuration;
        }
        
        public IActionResult Index(int missionId)
        {
            string? userSessionEmailId = HttpContext.Session.GetString("useremail");
            
            StoryHomeModel storyHomeModel = new StoryHomeModel();
            User userObj = _userRepository.findUser(userSessionEmailId);
            storyHomeModel.username = userObj.FirstName + " " + userObj.LastName;
            storyHomeModel.avtar=userObj.Avatar;
            storyHomeModel.missionId= missionId;
            storyHomeModel.cmsPages = _missionRepository.GetCMSPages();
            return View(storyHomeModel);
        }
        public IActionResult getStories(int pageNumber)
        {
            PaginationStory paginationStory=_storyRepository.getStories(pageNumber);
            return PartialView("_storyList", paginationStory);
        }
        
        public IActionResult ShareStory()
        {
            string? userSessionEmailId = HttpContext.Session.GetString("useremail");
            
            ShareStoryModel shareStoryModel = new ShareStoryModel();
            User userObj = _userRepository.findUser(userSessionEmailId);
            shareStoryModel.username = userObj.FirstName + " " + userObj.LastName;
            shareStoryModel.avatar=userObj.Avatar;
            shareStoryModel.userId = userObj.UserId;
            shareStoryModel.cmsPages = _missionRepository.GetCMSPages();
            shareStoryModel.missions = _missionRepository.getMissionsOfUser((int)userObj.UserId);
            return View(shareStoryModel);
        }
        
        [HttpPost]
        public IActionResult Upload(List<IFormFile> postedFiles)
        {
            string? userSessionEmailId = HttpContext.Session.GetString("useremail");
            
            User userObj = _userRepository.findUser(userSessionEmailId);
            int userId = (int)userObj.UserId;


            string wwwPath = this._hostEnvironment.WebRootPath;
            string contentPath = this._hostEnvironment.ContentRootPath;

            string path = Path.Combine(this._hostEnvironment.WebRootPath, @"images\uploads");
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory("postedFiles");
            }

            foreach (IFormFile postedFile in postedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                string time=DateTime.Now.ToString("yyyyMMdd");

                string[] arr = fileName.Split(".");
                fileName = arr[0] + time+userId +"." + arr[1];
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
            }

            return Content("Success");
        }
        [HttpPost]
        public IActionResult saveStory(StorySaveModel storySaveModelObj)
        {
            int storyId=_storyRepository.saveStories(storySaveModelObj);
            return Json(new { storyId=storyId });
        }
        [HttpPost]
        public void submitStory(StorySaveModel storySaveModelObj)
        {
            _storyRepository.submitStories(storySaveModelObj);
        }
        
        public IActionResult StoryDetails(int? storyId)
        {
            string? userSessionEmailId = HttpContext.Session.GetString("useremail");
            
            StoryDetailsModel storyDetailsObj = new StoryDetailsModel();
            User userObj = _userRepository.findUser(userSessionEmailId);
            storyDetailsObj.username = userObj.FirstName + " " + userObj.LastName;
            storyDetailsObj.avtar = userObj.Avatar;
            storyDetailsObj.cmsPages = _missionRepository.GetCMSPages();
            storyDetailsObj.story= _storyRepository.getStoryDetail((int)storyId);
            storyDetailsObj.users=_userRepository.getUsers();
            return View(storyDetailsObj);
        }
       
        public IActionResult getTotalStoryViews(int storyId)
        {
            int total=_storyRepository.getTotalStoryViews((int)storyId);
            return Json(new {data=total});
        }
        
        public IActionResult recommendToCoworker( int storyId, string toUserEmail)
        {
            string? userSessionEmailId = HttpContext.Session.GetString("useremail");           
            User fromUserObj = _userRepository.findUser(userSessionEmailId);
            int fromUserId=(int)fromUserObj.UserId;
            User userObj;
            try
            {
                userObj = _userRepository.findUser(toUserEmail);
            }
            catch (Exception ex)
            {
                return Json(new { data = "Email not found"+ex, status = 0 });
            }
            _storyRepository.recommendToCoworker(fromUserId, (int)userObj.UserId, storyId);
            string welcomeMessage = "Welcome to CI platform, <br/> You are recommended to watch below story </br>";
            string path = "<a href=\"" + " https://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/Story/StoryDetails?storyId=" + storyId + " \"   style=\"font-weight:500;color:blue;\" > Recommended to watch story </a>";

            Notification notification = new Notification();
            notification.NotificationMessage = fromUserObj.FirstName + "-Recommended this Story-" + _storyRepository.getStoryDetail(storyId).Title;
            notification.NotificationType = "RecommendedStory";
            notification.Status = true;
            notification.MessageId = storyId;
            notification.UserId = (int)userObj.UserId;
            notification.FromUserId = fromUserId;
            notification.NotificationImage = fromUserObj.Avatar;
            notification.CreatedAt = DateTime.Now;
            _missionRepository.addNotification(notification);

            MailHelper mailHelper = new MailHelper(configuration);
            ViewBag.sendMail = mailHelper.Send(toUserEmail, welcomeMessage + path,"Recommeded to watch story");

            return Json(new { data = "Email sent successfully", status = 1 });
        }

        public IActionResult logout()
        {
            HttpContext.Session.Remove("useremail");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
