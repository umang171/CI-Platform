using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIPlatform.Controllers
{
    public class StoryController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly IMissionRepository _missionRepository;
        private readonly IStoryRepository _storyRepository;

        public StoryController(IUserRepository userRepository, IMissionRepository missionRepository,IStoryRepository storyRepository)
        {
            _userRepository = userRepository;
            _missionRepository = missionRepository;
            _storyRepository = storyRepository;
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
            return View(shareStoryModel);
        }
    }
}
