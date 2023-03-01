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

        void IUserRepository.addResetPasswordToken(ResetPassword passwordResetObj)
        {
            bool isAlreadyGenerated=_ciPlatformDbContext.ResetPasswords.Any(u => u.Email.Equals(passwordResetObj.Email));
            if (isAlreadyGenerated)
            {
                _ciPlatformDbContext.Update(passwordResetObj);

            }
            else
            {
                _ciPlatformDbContext.Add(passwordResetObj);
            }
            _ciPlatformDbContext.SaveChanges();
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

        ResetPassword IUserRepository.findUserByToken(string token)
        {
            return _ciPlatformDbContext.ResetPasswords.Where(u => u.Token == token).First();
        }

        void IUserRepository.removeResetPasswordToekn(ResetPassword obj)
        {
            _ciPlatformDbContext.Remove(obj);
            _ciPlatformDbContext.SaveChanges();
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
