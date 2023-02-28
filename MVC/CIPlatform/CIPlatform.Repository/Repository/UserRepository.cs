using CIPlatform.Entities.DataModels;
using CIPlatform.Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repository.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly CIPlatformDbContext _ciPlatformDbContext;
        public UserRepository(CIPlatformDbContext cIPlatformDbContext)
        {
            _ciPlatformDbContext = cIPlatformDbContext;
        }
        public IEnumerable<User> getUsers()
        {
            var Users= _ciPlatformDbContext.Users;
            return Users;
        }

        void IUserRepository.addUser(User user)
        {
            _ciPlatformDbContext.Users.Add(user);
            _ciPlatformDbContext.SaveChanges();
        }

        User IUserRepository.findUser(string email)
        {
            return _ciPlatformDbContext.Users.Where(u => u.Email.Equals(email)).First();
        }
        User IUserRepository.findUser(int? id)
        {
            return _ciPlatformDbContext.Users.Where(u=> u.UserId == id).First();
        }

        void IUserRepository.updatePassword(User user)
        {
            _ciPlatformDbContext.Update(user);
            _ciPlatformDbContext.SaveChanges();
        }

        bool IUserRepository.validateEmail(string email)
        {
            return _ciPlatformDbContext.Users.Any(u => u.Email == email);
        }

        bool IUserRepository.validateUser(string email, string password)
        {
            return _ciPlatformDbContext.Users.Any(u => u.Password == password && u.Email== email);
        }
    }
}
