using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;
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

        void IUserRepository.editUserProfile(UserProfileModel userProfileModel)
        {
            User user= findUser((int ?)userProfileModel.UserId);
            user.FirstName=userProfileModel.FirstName;
            user.LastName=userProfileModel.LastName;
            user.Avatar=userProfileModel.Avatar!=null?userProfileModel.Avatar:user.Avatar;
            user.EmployeeId=userProfileModel.EmployeeId;
            user.Title=userProfileModel.Title;
            user.Department=userProfileModel.Department;
            user.ProfileText=userProfileModel.ProfileText;
            user.WhyIVolunteer=userProfileModel.WhyIVolunteer;
            user.CountryId= userProfileModel.CountryId;
            user.CityId = userProfileModel.CityId;
            user.LinkedInUrl = userProfileModel.LinkedInUrl;
            _ciPlatformDbContext.Users.Update(user);
            _ciPlatformDbContext.SaveChanges();

            if (userProfileModel.userSkillNames != null)
            {
                string userSkills = userProfileModel.userSkillNames.Replace("\r", "");
                string[] arrUserSkills=userSkills.Split("\n").SkipLast(1).ToArray();
                var arrUserSkillsIDs = new List<long>();
                foreach (string skill in arrUserSkills)
                {
                    long skillID = getIdOfUserSkill(skill);
                    arrUserSkillsIDs.Add(skillID);
                }
                if(_ciPlatformDbContext.UserSkills.Where(skill => skill.UserId == userProfileModel.UserId).Any())
                {
                    List<UserSkill> removeSkills=_ciPlatformDbContext.UserSkills.Where(skill => skill.UserId == userProfileModel.UserId).ToList();
                    foreach(UserSkill skill in removeSkills)
                    {
                        _ciPlatformDbContext.UserSkills.Remove(skill);
                    }
                    foreach (long skillID in arrUserSkillsIDs)
                    {
                        UserSkill userSkill = new UserSkill();
                        userSkill.SkillId = (int)skillID;
                        userSkill.UserId = userProfileModel.UserId;
                        _ciPlatformDbContext.UserSkills.Add(userSkill);
                        _ciPlatformDbContext.SaveChanges();
                    }
                }
                else
                {
                    foreach(long skillID in arrUserSkillsIDs)
                    {
                        UserSkill userSkill=new UserSkill();
                        userSkill.SkillId=(int) skillID;
                        userSkill.UserId=userProfileModel.UserId;
                        _ciPlatformDbContext.UserSkills.Add(userSkill);
                        _ciPlatformDbContext.SaveChanges();
                    }
                }
            }        
        }

        public User findUser(string email)
        {
            return _ciPlatformDbContext.Users.Include(user=>user.UserSkills).Where(u => u.Email.Equals(email)).First();
        }
        public User findUser(int? id)
        {
            return _ciPlatformDbContext.Users.Where(u=> u.UserId == id).First();
        }

        ResetPassword IUserRepository.findUserByToken(string token)
        {
            
            return _ciPlatformDbContext.ResetPasswords.Where(u => u.Token == token).First();
        }

        string IUserRepository.getCityFromCityId(long cityId)
        {
            return _ciPlatformDbContext.Cities.Where(city => city.CityId == cityId).Select(city => city.Name).First();
        }

        string IUserRepository.getCountryFromCountryId(long countryId)
        {
            return _ciPlatformDbContext.Countries.Where(country=>country.CountryId==countryId).Select(country=> country.Name).First();
        }

        List<Country> IUserRepository.getCountryNames()
        {
            return _ciPlatformDbContext.Countries.ToList();
        }

        List<Skill> IUserRepository.getSkillNames()
        {
            return _ciPlatformDbContext.Skills.ToList();
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

        List<City> IUserRepository.getCityNames(int countryId)
        {
            if (countryId == null || countryId<0)
            {
                countryId=1;
            }
            return _ciPlatformDbContext.Cities.Where(city=>city.CountryId==countryId).ToList();
        }

        public long getIdOfUserSkill(string userSkillName)
        {
            long skillId= _ciPlatformDbContext.Skills.Where(skill => skill.SkillName == userSkillName).Select(skill => skill.SkillId).First();
            return skillId;
        }

        List<UserSkill> IUserRepository.getSkillsOfUser(int userId)
        {
            return _ciPlatformDbContext.UserSkills.Where(skill => skill.UserId == userId).ToList();
        }
    }
}
