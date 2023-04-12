using CIPlatform.Entities.DataModels;
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
        List<User> IAdminRepository.getUsers(string? searchText)
        {
            if (searchText == null)
                return _ciPlatformDbContext.Users.ToList();
            else
                return _ciPlatformDbContext.Users.Where(user => user.FirstName.Contains(searchText)).ToList();
        }
        public Admin findAdmin(string email)
        {
            return _ciPlatformDbContext.Admins.Where(admin => admin.Email == email).First();
        }
    }
}
