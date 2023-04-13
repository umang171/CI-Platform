using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Helpers;
using CIPlatform.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIPlatform.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMissionRepository _missionRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IAdminRepository _adminRepository;
        
        public AdminController(IUserRepository userRepository, IMissionRepository missionRepository, IStoryRepository storyRepository,IAdminRepository adminRepository)
        {
            _userRepository = userRepository;
            _missionRepository = missionRepository;
            _storyRepository = storyRepository;
            _adminRepository = adminRepository;
        }
        [SessionHelper]
        public IActionResult Index()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            Admin admin= _adminRepository.findAdmin(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username=admin.FirstName+" "+admin.LastName;
            AdminUserEditModel adminUserEditModel = new AdminUserEditModel();
            adminUserEditModel.adminHeader = adminHeader;
            return View(adminUserEditModel);
        }
        public IActionResult getUsers(string? searchText,int pageNumber,int pageSize)
        {
            AdminPageList<User> user= _adminRepository.getUsers(searchText,pageNumber,pageSize);
            return PartialView("_AdminUserList",user);
        }
        public IActionResult deleteUser(long userId)
        {
            _adminRepository.deleteUser(userId);
            return Ok();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(AdminLogin adminLoginObj)
        {
            Boolean isValidEmail = _adminRepository.validateEmail(adminLoginObj.EmailId);
            if (!isValidEmail)
            {
                ModelState.AddModelError("EmailId", "Email not found");
            }
            else
            {
                Boolean isValidUser = _adminRepository.validateUser(adminLoginObj.EmailId, adminLoginObj.Password);
                if (!isValidUser)
                {
                    ModelState.AddModelError("Password", "Password does not match");
                }
            }
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("useremail", adminLoginObj.EmailId);
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
    }
}
