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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(IUserRepository userRepository, IMissionRepository missionRepository, IStoryRepository storyRepository,IAdminRepository adminRepository, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _missionRepository = missionRepository;
            _storyRepository = storyRepository;
            _adminRepository = adminRepository;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult GetUsers(string? searchText,int pageNumber,int pageSize)
        {
            AdminPageList<User> user= _adminRepository.getUsers(searchText,pageNumber,pageSize);
            return PartialView("_AdminUserList",user);
        }
        [SessionHelper]
        public IActionResult AddUser()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            Admin admin = _adminRepository.findAdmin(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminUserAddModel adminUserAddModel = new AdminUserAddModel();
            adminUserAddModel.adminHeader = adminHeader;
            return View(adminUserAddModel);
        }
        [SessionHelper]
        [HttpPost]
        public IActionResult AddUser(AdminUserAddModel adminUserAddModel,IFormFile? Avatar)
        {
            if (_userRepository.validateEmail(adminUserAddModel.EmailId))
            {
                ModelState.AddModelError("EmailId", "Email already registred");
            }
            if (ModelState.IsValid)
            {
                User user = new User();
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (Avatar != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\avatars");
                    var extension = Path.GetExtension(Avatar.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        Avatar.CopyTo(fileStreams);
                    }
                    user.Avatar = @"\images\avatars\" + fileName + extension;
                }
                else
                {
                    user.Avatar = "/images/user1.png";
                }
                user.FirstName = adminUserAddModel.FirstName;
                user.LastName = adminUserAddModel.LastName;
                user.PhoneNumber = long.Parse(adminUserAddModel.PhoneNo);
                user.Email = adminUserAddModel.EmailId;
                user.Password = adminUserAddModel.Password;
                user.EmployeeId = adminUserAddModel.EmployeeId;
                user.Department = adminUserAddModel.Department;
                user.ProfileText = adminUserAddModel.MyProfile;
                user.WhyIVolunteer = adminUserAddModel.WhyIVolunteer;
                _userRepository.addUser(user);
                return RedirectToAction("Index","Admin");
            }
            return View(adminUserAddModel);
        }
        [SessionHelper]
        public IActionResult EditUser(int userId)
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            Admin admin = _adminRepository.findAdmin(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminUserAddModel adminUserAddModel = new AdminUserAddModel();
            adminUserAddModel.adminHeader = adminHeader;

            User user=_userRepository.findUser(userId);
            adminUserAddModel.FirstName= user.FirstName;
            adminUserAddModel.LastName= user.LastName;
            adminUserAddModel.PhoneNo = user.PhoneNumber.ToString();
            adminUserAddModel.EmailId = user.Email;
            adminUserAddModel.Password=user.Password;
            adminUserAddModel.Department=user.Department;
            adminUserAddModel.EmployeeId = user.EmployeeId;
            adminUserAddModel.MyProfile = user.ProfileText;
            adminUserAddModel.WhyIVolunteer = user.WhyIVolunteer;
            adminUserAddModel.UserId = user.UserId;
            adminUserAddModel.Profile = user.Avatar;
            return View(adminUserAddModel);
        }
        [HttpPost]
        public IActionResult EditUser(AdminUserAddModel adminUserAddModel, IFormFile? Avatar)
        {           
            if (ModelState.IsValid)
            {
                User user =_userRepository.findUser((int)adminUserAddModel.UserId);
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (Avatar != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\avatars");
                    var extension = Path.GetExtension(Avatar.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        Avatar.CopyTo(fileStreams);
                    }
                    user.Avatar = @"\images\avatars\" + fileName + extension;
                }
                else
                {
                    user.Avatar = adminUserAddModel.Profile;
                }
                user.FirstName = adminUserAddModel.FirstName;
                user.LastName = adminUserAddModel.LastName;
                user.PhoneNumber = long.Parse(adminUserAddModel.PhoneNo);
                user.Email = adminUserAddModel.EmailId;
                user.Password = adminUserAddModel.Password;
                user.EmployeeId = adminUserAddModel.EmployeeId;
                user.Department = adminUserAddModel.Department;
                user.ProfileText = adminUserAddModel.MyProfile;
                user.WhyIVolunteer = adminUserAddModel.WhyIVolunteer;
                _adminRepository.editUser(user);
                return RedirectToAction("Index", "Admin");
            }
            return View(adminUserAddModel);
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
        [SessionHelper]
        public IActionResult AdminCMSPage()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            Admin admin = _adminRepository.findAdmin(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminCMSModel adminCMSModel = new AdminCMSModel();
            adminCMSModel.adminHeader = adminHeader;
            return View(adminCMSModel);
        }
        [SessionHelper]
        public IActionResult GetCMSPages(string? searchText, int pageNumber, int pageSize)
        {
            AdminPageList<CmsPage> cmsPage=_adminRepository.getCmsPages(searchText, pageNumber, pageSize);
            return PartialView("_AdminCMSList",cmsPage);
        }
        public IActionResult DeleteCmsPage(long cmsPageId)
        {
            _adminRepository.deleteCmsPage(cmsPageId);
            return Ok();
        }
        [SessionHelper]
        public IActionResult ADDCmsPage()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            Admin admin = _adminRepository.findAdmin(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminCMSModel adminCMSModel = new AdminCMSModel();
            adminCMSModel.adminHeader = adminHeader;
            return View(adminCMSModel);
        }
        [SessionHelper]
        [HttpPost]
        public IActionResult ADDCmsPage(AdminCMSModel adminCMSModel)
        {
            if (ModelState.IsValid)
            {
                CmsPage cmsPage = new CmsPage
                {
                    Title = adminCMSModel.Title,
                    Slug = adminCMSModel.Slug,
                    Description = adminCMSModel.Description,
                    Status = Boolean.Parse(adminCMSModel.Status),
                };
                _adminRepository.addCMSPage(cmsPage);
                return RedirectToAction("AdminCMSPage");
            }
            return View(adminCMSModel);
        }
        [SessionHelper]
        public IActionResult EditCMSPage(long cmsPageId)
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            Admin admin = _adminRepository.findAdmin(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminCMSModel adminCMSModel = new AdminCMSModel();
            adminCMSModel.adminHeader = adminHeader;
            CmsPage cmsPage = _adminRepository.findCMSPageByID(cmsPageId);

            adminCMSModel.Title = cmsPage.Title;
            adminCMSModel.Description = cmsPage.Description;
            adminCMSModel.Slug = cmsPage.Slug;
            adminCMSModel.Status = cmsPage.Status.ToString();
            adminCMSModel.CMSPageId = cmsPageId;
            return View(adminCMSModel);
        }
        [SessionHelper]
        [HttpPost]
        public IActionResult EditCMSPage(AdminCMSModel adminCMSModel)
        {
            if (ModelState.IsValid)
            {
                CmsPage cmsPage=_adminRepository.findCMSPageByID(adminCMSModel.CMSPageId);
                cmsPage.Title = adminCMSModel.Title;
                cmsPage.Slug = adminCMSModel.Slug;
                cmsPage.Description = adminCMSModel.Description;
                cmsPage.Status = Boolean.Parse(adminCMSModel.Status);
                cmsPage.CmsPageId=adminCMSModel.CMSPageId;
                _adminRepository.editCMSPage(cmsPage);
                return RedirectToAction("AdminCMSPage");
            }
            return View(adminCMSModel);
        }
        [SessionHelper]
        public IActionResult AdminMission()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            Admin admin = _adminRepository.findAdmin(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminMissionModel adminMissionModel = new AdminMissionModel();
            adminMissionModel.adminHeader = adminHeader;
            return View(adminMissionModel);
        }
        [SessionHelper]
        public IActionResult GetMissions(string? searchText, int pageNumber, int pageSize)
        {
            AdminPageList<Mission> mission = _adminRepository.getMissions(searchText, pageNumber, pageSize);
            return PartialView("_AdminMissionList", mission);
        }
        [SessionHelper]
        public IActionResult AddMission()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            Admin admin = _adminRepository.findAdmin(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminMissionModel adminMissionModel= new AdminMissionModel();
            adminMissionModel.adminHeader = adminHeader;
            return View(adminMissionModel);
        }
        [SessionHelper]
        public IActionResult AdminBannerMgmt()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            Admin admin = _adminRepository.findAdmin(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminBannerModel adminBannerModel = new AdminBannerModel();
            adminBannerModel.adminHeader = adminHeader;
            return View(adminBannerModel);
        }
        [SessionHelper]
        public IActionResult GetBanners(string? searchText, int pageNumber, int pageSize)
        {
            AdminPageList<Banner> banners = _adminRepository.GetBanners(searchText, pageNumber, pageSize);
            return PartialView("_AdminBannerList",banners);
        }
        [SessionHelper]
        public IActionResult AddBanner()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            Admin admin = _adminRepository.findAdmin(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminBannerModel adminBannerModel = new AdminBannerModel();
            adminBannerModel.adminHeader = adminHeader;
            return View(adminBannerModel);
        }
        [SessionHelper]
        [HttpPost]
        public IActionResult AddBanner(AdminBannerModel adminBannerModel,IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                Banner banner = new Banner();

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (Image != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\banners");
                    var extension = Path.GetExtension(Image.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        Image.CopyTo(fileStreams);
                    }
                    banner.Image= @"\images\banners\" + fileName + extension;
                }
                banner.Text = adminBannerModel.Text;
                banner.SortOrder=adminBannerModel.SortOrder;
                _adminRepository.addBanner(banner);
                return RedirectToAction("AdminBannerMgmt");
            }
            return View(adminBannerModel);
        }
        [SessionHelper]
        public IActionResult DeleteBanner(long bannerId)
        {
            _adminRepository.deleteBanner(bannerId);
            return Ok();
        }
        [SessionHelper]
        public IActionResult EditBanner(long bannerId)
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            Admin admin = _adminRepository.findAdmin(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminBannerModel adminBannerModel = new AdminBannerModel();
            adminBannerModel.adminHeader = adminHeader;
            Banner banner = _adminRepository.findBannerById(bannerId);
            adminBannerModel.Text = banner.Text;
            adminBannerModel.BannerImage= banner.Image;
            adminBannerModel.SortOrder = banner.SortOrder;
            adminBannerModel.BannerID = banner.BannerId;
            return View(adminBannerModel);
        }
        [HttpPost]
        public IActionResult EditBanner(AdminBannerModel adminBannerModel, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                Banner banner = new Banner();

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (Image != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\banners");
                    var extension = Path.GetExtension(Image.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        Image.CopyTo(fileStreams);
                    }
                    banner.Image = @"\images\banners\" + fileName + extension;
                }
                else
                {
                    banner.Image= adminBannerModel.BannerImage;
                }
                banner.Text = adminBannerModel.Text;
                banner.SortOrder = adminBannerModel.SortOrder;
                banner.BannerId = adminBannerModel.BannerID;
                _adminRepository.editBanner(banner);
                return RedirectToAction("AdminBannerMgmt");
            }
            return View(adminBannerModel);
        }
    }   
}
