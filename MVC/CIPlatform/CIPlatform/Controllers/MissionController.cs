using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

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
                return RedirectToAction("Login","Account");
            }

            var missions = _missionRepository.GetMissions();

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
    }
}
