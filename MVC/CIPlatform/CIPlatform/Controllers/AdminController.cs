using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Helpers;
using CIPlatform.Repository.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIPlatform.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMissionRepository _missionRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(IUserRepository userRepository, IMissionRepository missionRepository, IStoryRepository storyRepository, IAdminRepository adminRepository, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _missionRepository = missionRepository;
            _storyRepository = storyRepository;
            _adminRepository = adminRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public IActionResult Index()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminUserEditModel adminUserEditModel = new AdminUserEditModel();
            adminUserEditModel.adminHeader = adminHeader;
            return View(adminUserEditModel);
        }
        public IActionResult GetUsers(string? searchText, int pageNumber, int pageSize)
        {
            AdminPageList<User> user = _adminRepository.getUsers(searchText, pageNumber, pageSize);
            return PartialView("_AdminUserList", user);
        }
        
        public IActionResult AddUser()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminUserAddModel adminUserAddModel = new AdminUserAddModel();
            adminUserAddModel.adminHeader = adminHeader;
            return View(adminUserAddModel);
        }
        
        [HttpPost]
        public IActionResult AddUser(AdminUserAddModel adminUserAddModel, IFormFile? Avatar)
        {
            if (_userRepository.validateEmail(adminUserAddModel.EmailId))
            {
                ModelState.AddModelError("EmailId", "Email already registred");
            }
            if (_userRepository.HasAlreadyEmployeeId(adminUserAddModel.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", "Employee Id is already there");
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
                user.Status = Boolean.Parse(adminUserAddModel.Status);
                _userRepository.addUser(user);
                return RedirectToAction("Index", "Admin");
            }
            return View(adminUserAddModel);
        }
        
        public IActionResult EditUser(int userId)
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminUserAddModel adminUserAddModel = new AdminUserAddModel();
            adminUserAddModel.adminHeader = adminHeader;

            User user = _userRepository.findUser(userId);
            adminUserAddModel.FirstName = user.FirstName;
            adminUserAddModel.LastName = user.LastName;
            adminUserAddModel.PhoneNo = user.PhoneNumber.ToString();
            adminUserAddModel.EmailId = user.Email;
            adminUserAddModel.Password = user.Password;
            adminUserAddModel.Department = user.Department;
            adminUserAddModel.EmployeeId = user.EmployeeId;
            adminUserAddModel.MyProfile = user.ProfileText;
            adminUserAddModel.WhyIVolunteer = user.WhyIVolunteer;
            adminUserAddModel.UserId = user.UserId;
            adminUserAddModel.Profile = user.Avatar;
            adminUserAddModel.Status = user.Status.ToString();
            return View(adminUserAddModel);
        }
        [HttpPost]
        public IActionResult EditUser(AdminUserAddModel adminUserAddModel, IFormFile? Avatar)
        {
            if (ModelState.IsValid)
            {
                User user = _userRepository.findUser((int)adminUserAddModel.UserId);
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
                user.Status= Boolean.Parse(adminUserAddModel.Status);
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
        
        public IActionResult AdminCMSPage()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminCMSModel adminCMSModel = new AdminCMSModel();
            adminCMSModel.adminHeader = adminHeader;
            return View(adminCMSModel);
        }
        
        public IActionResult GetCMSPages(string? searchText, int pageNumber, int pageSize)
        {
            AdminPageList<CmsPage> cmsPage = _adminRepository.getCmsPages(searchText, pageNumber, pageSize);
            return PartialView("_AdminCMSList", cmsPage);
        }
        public IActionResult DeleteCmsPage(long cmsPageId)
        {
            _adminRepository.deleteCmsPage(cmsPageId);
            return Ok();
        }
        
        public IActionResult ADDCmsPage()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminCMSModel adminCMSModel = new AdminCMSModel();
            adminCMSModel.adminHeader = adminHeader;
            return View(adminCMSModel);
        }
        
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
        
        public IActionResult EditCMSPage(long cmsPageId)
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
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
        
        [HttpPost]
        public IActionResult EditCMSPage(AdminCMSModel adminCMSModel)
        {
            if (ModelState.IsValid)
            {
                CmsPage cmsPage = _adminRepository.findCMSPageByID(adminCMSModel.CMSPageId);
                cmsPage.Title = adminCMSModel.Title;
                cmsPage.Slug = adminCMSModel.Slug;
                cmsPage.Description = adminCMSModel.Description;
                cmsPage.Status = Boolean.Parse(adminCMSModel.Status);
                cmsPage.CmsPageId = adminCMSModel.CMSPageId;
                _adminRepository.editCMSPage(cmsPage);
                return RedirectToAction("AdminCMSPage");
            }
            return View(adminCMSModel);
        }
        
        public IActionResult AdminMission()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminMissionModel adminMissionModel = new AdminMissionModel();
            adminMissionModel.adminHeader = adminHeader;
            return View(adminMissionModel);
        }
        
        public IActionResult GetMissions(string? searchText, int pageNumber, int pageSize)
        {
            AdminPageList<Mission> mission = _adminRepository.getMissions(searchText, pageNumber, pageSize);
            return PartialView("_AdminMissionList", mission);
        }
        
        public IActionResult AddMission()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminMissionModel adminMissionModel = new AdminMissionModel();
            adminMissionModel.adminHeader = adminHeader;
            adminMissionModel.countryLists = _adminRepository.GetCountryLists();
            adminMissionModel.cityLists= _adminRepository.GetCityLists(adminMissionModel.countryLists.ElementAt(0).CountryId);
            adminMissionModel.themeLists= _adminRepository.GetThemeLists();
            adminMissionModel.skillLists= _adminRepository.GetSkillLists();
            return View(adminMissionModel);
        }
        
        [HttpPost]
        public IActionResult AddMission(AdminMissionModel adminMissionModel, List<IFormFile> missionMedias, List<IFormFile> missionDocuments)
        {
            if (missionMedias.Count() == 0)
                ModelState.AddModelError("MissionMedia", "Please select Media");
            if (adminMissionModel.StartDate == null)
                ModelState.AddModelError("StartDate", "Start Date is required");
            if (adminMissionModel.EndDate == null)
                ModelState.AddModelError("EndDate", "End Date is required");
            if (adminMissionModel.MissionType == "time")
            {                
                if (adminMissionModel.StartDate!= null && adminMissionModel.EndDate != null)
                {
                    if (adminMissionModel.StartDate > adminMissionModel.EndDate)
                    {
                        ModelState.AddModelError("StartDate", "Start Date must be less then end date");
                    }
                }
            }
            else
            {
                if (adminMissionModel.GoalValue == null || adminMissionModel.GoalValue<= 0)
                {
                    ModelState.AddModelError("GoalValue", "Goal value required for goal based mission");
                }
                if (adminMissionModel.GoalObjective == null || adminMissionModel.GoalObjective.Length == 0)
                {
                    ModelState.AddModelError("GoalObjective", "Goal Objective required for goal based mission");
                }
            }
            if (adminMissionModel.TotalSeats <= 0)
            {
                ModelState.AddModelError("TotalSeats", "Total seats must be greater than 0");
            }
            if (ModelState.IsValid)
            {
                Mission mission=new Mission();
                mission.Title = adminMissionModel.MissionTitle;
                mission.ShortDescription = adminMissionModel.ShortDescription;
                mission.Description = adminMissionModel.Description;
                mission.MissionType=adminMissionModel.MissionType;
                mission.StartDate = adminMissionModel.StartDate;
                mission.EndDate = adminMissionModel.EndDate;
                mission.CountryId=adminMissionModel.CountryId;
                mission.CityId = adminMissionModel.CityId;
                mission.TotalSeats = adminMissionModel.TotalSeats;
                mission.ThemeId=adminMissionModel.ThemeId;
                mission.OrganizationName=adminMissionModel.OrganizationName;
                mission.OrganizationDetail=adminMissionModel.OrganizationDetail;
                mission.Availability=adminMissionModel.Availability;
                mission.Status = Boolean.Parse(adminMissionModel.Status);

                mission.Status = true;
                List<MissionSkill> missionSkills=new List<MissionSkill>();
                foreach(var item in adminMissionModel.Skills.Split(",").SkipLast(1))
                {
                    MissionSkill skill=new MissionSkill();
                    skill.SkillId = Int32.Parse(item);                    
                    missionSkills.Add(skill);
                }

                GoalMission goalMission = new GoalMission();
                if (adminMissionModel.MissionType != "time")
                {
                    goalMission.GoalValue = (int)adminMissionModel.GoalValue;
                    goalMission.GoalObjectiveText = adminMissionModel.GoalObjective;
                }

                List<MissionMedium> missionMedia =new List<MissionMedium>();
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                foreach (IFormFile postedFile in missionMedias)
                {        
                    MissionMedium missionMedium = new MissionMedium();
                    string fileName = Guid.NewGuid().ToString();
                    missionMedium.MediaName = fileName;
                    var uploads = Path.Combine(wwwRootPath, @"images\missions");
                    missionMedium.MediaPath= @"images\missions";
                    var extension = Path.GetExtension(postedFile.FileName);
                    missionMedium.MediaType= extension;
                    missionMedia.Add(missionMedium);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        postedFile.CopyTo(fileStreams);
                    }
                }
                List<MissionDocument> missionDocument= new List<MissionDocument>();
                foreach (IFormFile postedFile in missionDocuments)
                {
                    MissionDocument missionDoc= new MissionDocument();
                    string fileName = Guid.NewGuid().ToString();
                    missionDoc.DocumentName= fileName;
                    var uploads = Path.Combine(wwwRootPath, @"documents");
                    missionDoc.DocumentPath = @"documents";
                    var extension = Path.GetExtension(postedFile.FileName);
                    missionDoc.DocumentType = extension;
                    missionDocument.Add(missionDoc);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        postedFile.CopyTo(fileStreams);
                    }
                }
                _adminRepository.AddMission(mission,missionSkills,goalMission,missionMedia,missionDocument);
                return RedirectToAction("AdminMission","Admin");
            }
            adminMissionModel.countryLists = _adminRepository.GetCountryLists();
            adminMissionModel.cityLists = _adminRepository.GetCityLists(adminMissionModel.CountryId);
            adminMissionModel.themeLists= _adminRepository.GetThemeLists();
            adminMissionModel.skillLists= _adminRepository.GetSkillLists();
            return View(adminMissionModel);
        }
        public IActionResult GetCities(long countryId)
        {
            return Json(_adminRepository.GetCityLists(countryId));
        }
        
        public IActionResult DeleteMission(long missionId)
        {
            _adminRepository.DeleteMission(missionId);
            return Ok();
        }
        
        public IActionResult EditMission(long missionId)
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminMissionModel adminMissionModel = _adminRepository.GetMissionDetails(missionId);
            adminMissionModel.adminHeader = adminHeader;
            adminMissionModel.countryLists = _adminRepository.GetCountryLists();
            adminMissionModel.cityLists = _adminRepository.GetCityLists(adminMissionModel.CountryId);
            adminMissionModel.themeLists = _adminRepository.GetThemeLists();
            adminMissionModel.skillLists = _adminRepository.GetSkillLists();
            return View(adminMissionModel);
        }
        
        [HttpPost]
        public IActionResult EditMission(AdminMissionModel adminMissionModel, List<IFormFile>? missionMedias, List<IFormFile>? missionDocuments)
        {
            if (missionMedias.Count() == 0 && adminMissionModel.MediaName == null)
                ModelState.AddModelError("MissionMedia", "Please select Media");
            if (missionDocuments.Count() == 0 && adminMissionModel.DocumentName== null)
                ModelState.AddModelError("MissionDocument", "Please select Document");
            if (adminMissionModel.StartDate == null)
                ModelState.AddModelError("StartDate", "Start Date is required");
            if (adminMissionModel.EndDate == null)
                ModelState.AddModelError("EndDate", "End Date is required");
            if (adminMissionModel.MissionType == "time")
            {
                if (adminMissionModel.StartDate != null && adminMissionModel.EndDate != null)
                {
                    if (adminMissionModel.StartDate > adminMissionModel.EndDate)
                    {
                        ModelState.AddModelError("StartDate", "Start Date must be less then end date");
                    }
                }
            }
            else
            {
                if (adminMissionModel.GoalValue == null || adminMissionModel.GoalValue <= 0)
                {
                    ModelState.AddModelError("GoalValue", "Goal value required for goal based mission");
                }
                if (adminMissionModel.GoalObjective == null || adminMissionModel.GoalObjective.Length == 0)
                {
                    ModelState.AddModelError("GoalObjective", "Goal Objective required for goal based mission");
                }
            }
            if (adminMissionModel.TotalSeats <= 0)
            {
                ModelState.AddModelError("TotalSeats", "Total seats must be greater than 0");
            }
            if (ModelState.IsValid)
            {
                Mission mission = _adminRepository.FindMissionById(adminMissionModel.MissionId);
                mission.Title = adminMissionModel.MissionTitle;
                mission.ShortDescription = adminMissionModel.ShortDescription;
                mission.Description = adminMissionModel.Description;
                mission.MissionType = adminMissionModel.MissionType;
                mission.StartDate = adminMissionModel.StartDate;
                mission.EndDate = adminMissionModel.EndDate;
                mission.CountryId = adminMissionModel.CountryId;
                mission.CityId = adminMissionModel.CityId;
                mission.TotalSeats = adminMissionModel.TotalSeats;
                mission.ThemeId = adminMissionModel.ThemeId;
                mission.OrganizationName = adminMissionModel.OrganizationName;
                mission.OrganizationDetail = adminMissionModel.OrganizationDetail;
                mission.Availability = adminMissionModel.Availability;
                mission.Status = Boolean.Parse(adminMissionModel.Status);

                List<MissionSkill> missionSkills = new List<MissionSkill>();
                foreach (var item in adminMissionModel.Skills.Split(",").SkipLast(1))
                {
                    MissionSkill skill = new MissionSkill();
                    skill.SkillId = Int32.Parse(item);
                    missionSkills.Add(skill);
                }

                GoalMission goalMission = new GoalMission();
                if (adminMissionModel.MissionType != "time")
                {
                    goalMission.GoalValue = (int)adminMissionModel.GoalValue;
                    goalMission.GoalObjectiveText = adminMissionModel.GoalObjective;
                }

                List<MissionMedium> missionMedia = new List<MissionMedium>();
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                string[] mediaNames = adminMissionModel.MediaName.Split(",").SkipLast(1).ToArray();
                string[] mediaPaths = adminMissionModel.MediaPath.Split(",").SkipLast(1).ToArray();
                string[] mediaTypes = adminMissionModel.MediaType.Split(",").SkipLast(1).ToArray();
                for (int i = 0; i < mediaNames.Length; i++)
                {
                    MissionMedium missionMedium = new MissionMedium();
                    missionMedium.MediaName = mediaNames[i];
                    missionMedium.MediaPath = mediaPaths[i];
                    missionMedium.MediaType = mediaTypes[i];
                    missionMedia.Add(missionMedium);
                }
                foreach (IFormFile postedFile in missionMedias)
                {
                    MissionMedium missionMedium = new MissionMedium();
                    string fileName = Guid.NewGuid().ToString();
                    missionMedium.MediaName = fileName;
                    var uploads = Path.Combine(wwwRootPath, @"images\missions");
                    missionMedium.MediaPath = @"images\missions";
                    var extension = Path.GetExtension(postedFile.FileName);
                    missionMedium.MediaType = extension;
                    missionMedia.Add(missionMedium);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        postedFile.CopyTo(fileStreams);
                    }
                }

                List<MissionDocument> missionDocument = new List<MissionDocument>();
                foreach (IFormFile postedFile in missionDocuments)
                {
                    MissionDocument missionDoc = new MissionDocument();
                    string fileName = Guid.NewGuid().ToString();
                    missionDoc.DocumentName = fileName;
                    var uploads = Path.Combine(wwwRootPath, @"documents");
                    missionDoc.DocumentPath = @"documents";
                    var extension = Path.GetExtension(postedFile.FileName);
                    missionDoc.DocumentType = extension;
                    missionDocument.Add(missionDoc);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        postedFile.CopyTo(fileStreams);
                    }
                }
                _adminRepository.EditMission(mission, missionSkills, goalMission, missionMedia, missionDocument);
                return RedirectToAction("AdminMission", "Admin");
            }
            adminMissionModel.countryLists = _adminRepository.GetCountryLists();
            adminMissionModel.cityLists = _adminRepository.GetCityLists(adminMissionModel.CountryId);
            adminMissionModel.themeLists = _adminRepository.GetThemeLists();
            adminMissionModel.skillLists = _adminRepository.GetSkillLists();
            return View(adminMissionModel);
        }
        
        public IActionResult AdminBannerMgmt()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminBannerModel adminBannerModel = new AdminBannerModel();
            adminBannerModel.adminHeader = adminHeader;
            return View(adminBannerModel);
        }
        
        public IActionResult GetBanners(string? searchText, int pageNumber, int pageSize)
        {
            AdminPageList<Banner> banners = _adminRepository.GetBanners(searchText, pageNumber, pageSize);
            return PartialView("_AdminBannerList", banners);
        }
        
        public IActionResult AddBanner()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminBannerModel adminBannerModel = new AdminBannerModel();
            adminBannerModel.adminHeader = adminHeader;
            return View(adminBannerModel);
        }
        
        [HttpPost]
        public IActionResult AddBanner(AdminBannerModel adminBannerModel, IFormFile Image)
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
                banner.Text = adminBannerModel.Text;
                banner.SortOrder = adminBannerModel.SortOrder;
                _adminRepository.addBanner(banner);
                return RedirectToAction("AdminBannerMgmt");
            }
            return View(adminBannerModel);
        }
        
        public IActionResult DeleteBanner(long bannerId)
        {
            _adminRepository.deleteBanner(bannerId);
            return Ok();
        }
        
        public IActionResult EditBanner(long bannerId)
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminBannerModel adminBannerModel = new AdminBannerModel();
            adminBannerModel.adminHeader = adminHeader;
            Banner banner = _adminRepository.findBannerById(bannerId);
            adminBannerModel.Text = banner.Text;
            adminBannerModel.BannerImage = banner.Image;
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
                    banner.Image = adminBannerModel.BannerImage;
                }
                banner.Text = adminBannerModel.Text;
                banner.SortOrder = adminBannerModel.SortOrder;
                banner.BannerId = adminBannerModel.BannerID;
                _adminRepository.editBanner(banner);
                return RedirectToAction("AdminBannerMgmt");
            }
            return View(adminBannerModel);
        }
        public IActionResult AdminSkill()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminSkillModel adminSkillModel = new AdminSkillModel();
            adminSkillModel.adminHeader = adminHeader;
            return View(adminSkillModel);
        }
        
        public IActionResult GetSkills(string? searchText, int pageNumber, int pageSize)
        {
            AdminPageList<Skill> skills = _adminRepository.GetSkills(searchText, pageNumber, pageSize);
            return PartialView("_AdminSkillList", skills);
        }
        
        public IActionResult DeleteSkill(long skillId)
        {
            _adminRepository.DeleteSkill(skillId);
            return Ok();
        }
        
        public IActionResult AddSkill()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminSkillModel adminSkillModel = new AdminSkillModel();
            adminSkillModel.adminHeader = adminHeader;
            return View(adminSkillModel);
        }
        
        [HttpPost]
        public IActionResult AddSkill(AdminSkillModel adminSkillModel)
        {
            if (ModelState.IsValid)
            {
                Skill skill = new Skill();
                skill.SkillName = adminSkillModel.SkillName;
                skill.Status = Byte.Parse(adminSkillModel.Status);
                _adminRepository.AddSkill(skill);
                return RedirectToAction("AdminSkill");
            }
            return View(adminSkillModel);
        }
        
        public IActionResult EditSkill(long skillId)
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminSkillModel adminSkillModel = new AdminSkillModel();
            adminSkillModel.adminHeader = adminHeader;
            Skill skill = _adminRepository.FindSkill(skillId);
            adminSkillModel.SkillName = skill.SkillName;
            adminSkillModel.SkillId = skill.SkillId;
            adminSkillModel.Status = skill.Status.ToString();
            return View(adminSkillModel);
        }
        
        [HttpPost]
        public IActionResult EditSkill(AdminSkillModel adminSkillModel)
        {
            if (ModelState.IsValid)
            {
                Skill skill = _adminRepository.FindSkill(adminSkillModel.SkillId);
                skill.SkillName = adminSkillModel.SkillName;
                skill.Status = Byte.Parse(adminSkillModel.Status);
                _adminRepository.EditSkill(skill);
                return RedirectToAction("AdminSkill");
            }
            return View(adminSkillModel);
        }
        
        public IActionResult AdminMissionApplication()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminMissionApplicationModel adminMissionApplicationModel = new AdminMissionApplicationModel();
            adminMissionApplicationModel.adminHeader = adminHeader;
            return View(adminMissionApplicationModel);
        }
        
        public IActionResult GetMissionApplications(string? searchText, int pageNumber, int pageSize)
        {
            AdminPageList<AdminMissionApplicationListModel> missionApplications = _adminRepository.GetMissionApplications(searchText, pageNumber, pageSize);
            return PartialView("_AdminMissionApplicationList", missionApplications);
        }
        
        public IActionResult ApproveMissionApplication(long missionApplicationId)
        {
            _adminRepository.ApproveMissionApplication(missionApplicationId);
            MissionApplication missionApplication = _missionRepository.GetMissionApplication((int)missionApplicationId);
            Mission mission = _missionRepository.GetMission((int)missionApplication.MissionId);
            Notification notification = new Notification();
            notification.NotificationMessage = "Mission Appication Approved-" +mission.Title;
            notification.NotificationType = "MissionApplicationApproved";
            notification.Status = true;
            notification.MessageId = -1;
            notification.UserId = (int)missionApplication.UserId;
            notification.FromUserId = -1;
            notification.NotificationImage = @"/images/checked.png";
            notification.CreatedAt = DateTime.Now;
            _missionRepository.addNotification(notification);
            return Ok();
        }
        
        public IActionResult RejectMissionApplication(long missionApplicationId)
        {
            MissionApplication missionApplication = _missionRepository.GetMissionApplication((int)missionApplicationId);
            Mission mission = _missionRepository.GetMission((int)missionApplication.MissionId);
            Notification notification = new Notification();
            notification.NotificationMessage = "Mission Appication Declined-" + mission.Title;
            notification.NotificationType = "MissionApplicationDeclined";
            notification.Status = true;
            notification.MessageId = -1;
            notification.UserId = (int)missionApplication.UserId;
            notification.FromUserId = -1;
            notification.NotificationImage = @"/images/new-message.png";
            notification.CreatedAt = DateTime.Now;
            _missionRepository.addNotification(notification);
            _adminRepository.RejectMissionApplication(missionApplicationId);
            return Ok();
        }
        
        public IActionResult AdminStory()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminStoryModel adminStoryModel = new AdminStoryModel();
            adminStoryModel.adminHeader = adminHeader;
            return View(adminStoryModel);
        }
        
        public IActionResult GetStories(string? searchText, int pageNumber, int pageSize)
        {
            AdminPageList<AdminStoryListModel> stories = _adminRepository.GetStories(searchText, pageNumber, pageSize);
            return PartialView("_AdminStoryList", stories);
        }
        public IActionResult ApproveStory(long storyId)
        {
            _adminRepository.ApproveStory(storyId);

            Story story=_storyRepository.getStoryDetail((int)storyId);
            Notification notification = new Notification();
            notification.NotificationMessage = "Story Approved-" + story.Title;
            notification.NotificationType = "StoryApproved";
            notification.Status = true;
            notification.MessageId = (int)storyId;
            notification.UserId = (int)story.UserId;
            notification.FromUserId = -1;
            notification.NotificationImage = @"/images/checked.png";
            notification.CreatedAt = DateTime.Now;
            _missionRepository.addNotification(notification);

            return Ok();
        }
        public IActionResult RejectStory(long storyId)
        {

            Story story = _storyRepository.getStoryDetail((int)storyId);
            Notification notification = new Notification();
            notification.NotificationMessage = "Story Declined-" + story.Title;
            notification.NotificationType = "StoryDeclined";
            notification.Status = true;
            notification.MessageId = -1;
            notification.UserId = (int)story.UserId;
            notification.FromUserId = -1;
            notification.NotificationImage = @"/images/new-message.png";
            notification.CreatedAt = DateTime.Now;
            _missionRepository.addNotification(notification);

            _adminRepository.RejectStory(storyId);
            return Ok();
        }
        public IActionResult AdminTheme()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminThemeModel adminThemeModel = new AdminThemeModel();
            adminThemeModel.adminHeader = adminHeader;
            return View(adminThemeModel);
        }
        
        public IActionResult GetThemes(string? searchText, int pageNumber, int pageSize)
        {
            AdminPageList<MissionTheme> missionTheme = _adminRepository.GetThemes(searchText, pageNumber, pageSize);
            return PartialView("_AdminThemeList", missionTheme);
        }
        
        public IActionResult AddTheme()
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminThemeModel adminThemeModel = new AdminThemeModel();
            adminThemeModel.adminHeader = adminHeader;
            return View(adminThemeModel);
        }
        
        [HttpPost]
        public IActionResult AddTheme(AdminThemeModel adminThemeModel)
        {
            if (ModelState.IsValid)
            {
                MissionTheme missionTheme = new MissionTheme();
                missionTheme.Title = adminThemeModel.ThemeName;
                missionTheme.Status = Boolean.Parse(adminThemeModel.Status);
                _adminRepository.AddTheme(missionTheme);
                return RedirectToAction("AdminTheme");
            }
            return View(adminThemeModel);
        }
        
        public IActionResult DeleteTheme(long themeId)
        {
            _adminRepository.DeleteTheme(themeId);
            return Ok();
        }
        
        public IActionResult EditTheme(long themeId)
        {
            string adminSessionEmail = HttpContext.Session.GetString("useremail");
            User admin = _userRepository.findUser(adminSessionEmail);
            AdminHeader adminHeader = new AdminHeader();
            adminHeader.username = admin.FirstName + " " + admin.LastName;
            AdminThemeModel adminThemeModel = new AdminThemeModel();
            adminThemeModel.adminHeader = adminHeader;
            MissionTheme missionTheme = _adminRepository.FindTheme(themeId);
            adminThemeModel.ThemeName = missionTheme.Title;
            adminThemeModel.ThemeId = missionTheme.MissionThemeId;
            adminThemeModel.Status = missionTheme.Status.ToString();
            return View(adminThemeModel);
        }
        
        [HttpPost]
        public IActionResult EditTheme(AdminThemeModel adminThemeModel)
        {
            if (ModelState.IsValid)
            {
                MissionTheme missionTheme = _adminRepository.FindTheme(adminThemeModel.ThemeId);
                missionTheme.Title = adminThemeModel.ThemeName;
                missionTheme.Status = Boolean.Parse(adminThemeModel.Status);
                _adminRepository.EditTheme(missionTheme);
                return RedirectToAction("AdminTheme");
            }
            return View(adminThemeModel);
        }
        public IActionResult logout()
        {
            HttpContext.Session.Remove("useremail");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}