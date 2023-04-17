using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repository.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly CIPlatformDbContext _ciPlatformDbContext;
        public AdminRepository(CIPlatformDbContext  cIPlatformDbContext)
        {
            _ciPlatformDbContext = cIPlatformDbContext;
        }      
        bool IAdminRepository.validateEmail(string email)
        {
            return _ciPlatformDbContext.Admins.Any(admin=> admin.Email == email);
        }

        bool IAdminRepository.validateUser(string email, string password)
        {
            return _ciPlatformDbContext.Admins.Any(admin => admin.Password == password && admin.Email == email);
        }
        AdminPageList<User> IAdminRepository.getUsers(string? searchText, int pageNumber, int pageSize)
        {
            IEnumerable<User> users;
            if (searchText == null)
                users=_ciPlatformDbContext.Users.Where(user=>user.DeletedAt==null);
            else
                users=_ciPlatformDbContext.Users.Where(user => user.DeletedAt == null).Where(user => user.FirstName.Contains(searchText) || user.Email.Contains(searchText) || user.LastName.Contains(searchText));
            var totalCounts=users.Count();
            var records = users
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .ToList();

            return new AdminPageList<User>(records,totalCounts){};
        }       
        public Admin findAdmin(string email)
        {
            return _ciPlatformDbContext.Admins.Where(admin => admin.Email == email).First();
        }

        void IAdminRepository.deleteUser(long userId)
        {
            User user=_ciPlatformDbContext.Users.Where(user => user.UserId == userId).First();
            user.DeletedAt = DateTime.Now;
            _ciPlatformDbContext.Users.Update(user);
            _ciPlatformDbContext.SaveChanges();
        }

        public void addUser(User user)
        {
            _ciPlatformDbContext.Users.Add(user);
            _ciPlatformDbContext.SaveChanges();
        }

        void IAdminRepository.editUser(User user)
        {
            _ciPlatformDbContext.Update(user);
            _ciPlatformDbContext.SaveChanges();
        }
        AdminPageList<CmsPage> IAdminRepository.getCmsPages(string? searchText, int pageNumber, int pageSize)
        {
            IEnumerable<CmsPage> cmsPages;
            if (searchText == null)
                cmsPages = _ciPlatformDbContext.CmsPages.Where(page => page.DeletedAt == null && page.Status == true);
            else
                cmsPages = _ciPlatformDbContext.CmsPages.Where(page => page.DeletedAt == null && page.Status == true).Where(page => page.Title.Contains(searchText));
            var totalCounts = cmsPages.Count();
            var records = cmsPages.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new AdminPageList<CmsPage>(records, totalCounts);
        }

        void IAdminRepository.deleteCmsPage(long cmsPageId)
        {
            CmsPage cmsPage=_ciPlatformDbContext.CmsPages.Where(cmsPage=>cmsPage.CmsPageId == cmsPageId).First();
            cmsPage.DeletedAt = DateTime.Now;
            _ciPlatformDbContext.CmsPages.Update(cmsPage);
            _ciPlatformDbContext.SaveChanges();
        }

        void IAdminRepository.addCMSPage(CmsPage cmsPage)
        {
            _ciPlatformDbContext.Add(cmsPage);
            _ciPlatformDbContext.SaveChanges();
        }

        CmsPage IAdminRepository.findCMSPageByID(long cmsPageID)
        {
            return _ciPlatformDbContext.CmsPages.Where(cmsPage=> cmsPage.DeletedAt == null && cmsPage.CmsPageId==cmsPageID).First();
        }

        void IAdminRepository.editCMSPage(CmsPage cmsPage)
        {
            _ciPlatformDbContext.Update(cmsPage);
            _ciPlatformDbContext.SaveChanges();
        }

        AdminPageList<Mission> IAdminRepository.getMissions(string? searchText, int pageNumber, int pageSize)
        {
            IEnumerable<Mission> missions;
            if (searchText == null)
                missions = _ciPlatformDbContext.Missions.Where(mission=> mission.DeletedAt == null && mission.Status == true);
            else
                missions = _ciPlatformDbContext.Missions.Where(mission => mission.DeletedAt == null && mission.Status == true).Where(mission => mission.Title.Contains(searchText));
            var totalCounts = missions.Count();
            var records = missions.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new AdminPageList<Mission>(records, totalCounts);
        }
    }
}