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
    }
}
