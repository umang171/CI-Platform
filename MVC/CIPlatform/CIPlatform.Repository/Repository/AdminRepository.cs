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
    }
}
