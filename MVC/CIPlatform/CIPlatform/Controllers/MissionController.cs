using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

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

            IEnumerable<Country> countries = _missionRepository.getCountries();
            missionHomeModel.countryList = countries;

            IEnumerable<City> cities=_missionRepository.getCities();
            missionHomeModel.cityList = cities;

            IEnumerable<MissionTheme>   themes=_missionRepository.getThemes();
            missionHomeModel.themeList= themes;

            IEnumerable<Skill> skills= _missionRepository.getSkills();
            missionHomeModel.skillList = skills; 

            return View(missionHomeModel);
        }
        public IActionResult Mission_Volunteer()
        {
            return View();
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
        public IActionResult getMissions()
        {
            IEnumerable<Mission> missions = _missionRepository.getMissions();

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            string mission = System.Text.Json.JsonSerializer.Serialize(missions, options);
            return Json(new { data = mission });
        }
        public IActionResult searchMissions(string searchText)
        {
            IEnumerable<Mission> missions = _missionRepository.searchMissions(searchText);

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            string mission = System.Text.Json.JsonSerializer.Serialize(missions, options);
            return Json(new { data = mission });
        }
        //public IActionResult gridSP()
        //{
        //    return _ciPlatformDbContext.missionViewModel.FromSqlInterpolated($"exec GetMissionData"));

        //    return Json(_ciPlatformContext.MissionList.FromSqlInterpolated($"exec GetMissionData"));
        //}
    }
}
