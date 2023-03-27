using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
namespace CIPlatform.Controllers
{
    public class StoryController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly IMissionRepository _missionRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnvironment;

        public StoryController(IUserRepository userRepository, IMissionRepository missionRepository,IStoryRepository storyRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnvironment) 
        {
            _userRepository = userRepository;
            _missionRepository = missionRepository;
            _storyRepository = storyRepository;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(int missionId)
        {
            string userSessionEmailId = HttpContext.Session.GetString("useremail");
            if (userSessionEmailId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            StoryHomeModel storyHomeModel = new StoryHomeModel();
            User userObj = _userRepository.findUser(userSessionEmailId);
            storyHomeModel.username = userObj.FirstName + " " + userObj.LastName;
            storyHomeModel.avtar=userObj.Avatar;
            storyHomeModel.missionId= missionId;
            return View(storyHomeModel);
        }
        public IActionResult getStories(int pageNumber)
        {
            PaginationStory paginationStory=_storyRepository.getStories(pageNumber);
            return PartialView("_storyList", paginationStory);
        }
        public IActionResult ShareStory()
        {
            string userSessionEmailId = HttpContext.Session.GetString("useremail");
            if (userSessionEmailId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ShareStoryModel shareStoryModel = new ShareStoryModel();
            User userObj = _userRepository.findUser(userSessionEmailId);
            shareStoryModel.username = userObj.FirstName + " " + userObj.LastName;
            shareStoryModel.avatar=userObj.Avatar;
            shareStoryModel.userId = userObj.UserId;
            shareStoryModel.missions = _missionRepository.getMissionsOfUser((int)userObj.UserId);
            return View(shareStoryModel);
        }
        [HttpPost]
        public IActionResult Upload(List<IFormFile> postedFiles)
        {
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
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
            }

            return Content("Success");
        }
        [HttpPost]
        public void saveStory(StorySaveModel storySaveModelObj)
        {
            _storyRepository.saveStories(storySaveModelObj);
        }
        [HttpPost]
        public void submitStory(StorySaveModel storySaveModelObj)
        {
            _storyRepository.submitStories(storySaveModelObj);
        }
    }
}
