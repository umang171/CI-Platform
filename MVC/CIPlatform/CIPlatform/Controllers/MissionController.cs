using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CIPlatform.Controllers
{
    public class MissionController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMissionRepository _missionRepository;
        public MissionController(IUserRepository userRepository,IMissionRepository missionRepository)
        {
            _userRepository = userRepository;
            _missionRepository = missionRepository;
        }
        public IActionResult Index()
        {
            string userSessionEmailId=HttpContext.Session.GetString("useremail");
            if ( userSessionEmailId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            MissionHomeModel missionHomeModel = new MissionHomeModel();
            User userObj = _userRepository.findUser(userSessionEmailId);
            missionHomeModel.username = userObj.FirstName+" "+userObj.LastName;
            missionHomeModel.userid=userObj.UserId;

            return View(missionHomeModel);
        }
        public IActionResult Mission_Volunteer(int? missionId)
        {
            string userSessionEmailId = HttpContext.Session.GetString("useremail");
            if (userSessionEmailId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            MissionVolunteerViewModel missionVolunteerViewModelObj= new MissionVolunteerViewModel();
            missionVolunteerViewModelObj= _missionRepository.getMissionFromMissionId((int)missionId);
            User userObj = _userRepository.findUser(userSessionEmailId);
            missionVolunteerViewModelObj.username = userObj.FirstName + " " + userObj.LastName;
            missionVolunteerViewModelObj.userid= (int)userObj.UserId;

            return View(missionVolunteerViewModelObj);
        }
        public IActionResult GetCountries()
        {
            IEnumerable<Country> countries = _missionRepository.getCountries();
            return Json(new { data = countries });
        }
        public IActionResult GetCites()
        {
            IEnumerable<City> cities = _missionRepository.getCities();
            return Json(new { data = cities });
        }
        public IActionResult GetThemes()
        {
            IEnumerable<MissionTheme> missionThemes = _missionRepository.getThemes();
            return Json(new { data = missionThemes });
        }
        public IActionResult GetSkills()
        {
            IEnumerable<Skill> missionSkills= _missionRepository.getSkills();
            return Json(new { data = missionSkills });
        }
        
        [HttpPost]
        public IActionResult getMissionsFromSP(string countryNames, string cityNames, string themeNames, string skillNames, string searchText, string sortValue,int pageNumber)
        {
            // make explicit SQL Parameter
            PaginationMission pagination =_missionRepository.getMissionsFromSP(countryNames, cityNames,themeNames,skillNames,searchText,sortValue, pageNumber) ;
           
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
            return Json(new { data=arr });
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
    }
}
