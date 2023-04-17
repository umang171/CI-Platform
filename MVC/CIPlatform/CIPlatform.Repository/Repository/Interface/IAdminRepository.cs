using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repository.Repository.Interface
{
    public interface IAdminRepository
    {
        public Boolean validateEmail(string email);
        public Boolean validateUser(string email, string password);
        public AdminPageList<User> getUsers(string? searchText,int pageNumber,int pageSize);
        public Admin findAdmin(string email);
        public void deleteUser(long userId);
        public void addUser(User user);
        public void editUser(User user);
        public AdminPageList<CmsPage> getCmsPages(string? searchText,int pageNumber,int pageSize);
        public void deleteCmsPage(long cmsPageId);
        public void addCMSPage(CmsPage cmsPage);
        public CmsPage findCMSPageByID(long cmsPageID);
        public void editCMSPage(CmsPage cmsPage);

    }
}
