

using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;

namespace CIPlatform.Repository.Repository.Interface
{
    public interface IUserRepository
    {
        public IEnumerable<User> getUsers();
        public Boolean validateEmail(string email);
        public Boolean validateUser(string email,string password);
        public User findUser(string email);
        public User findUser(int? id);
        
        public ResetPassword findUserByToken(string token);
        public void updatePassword(User user);

        public void addUser(User user);
        public void addResetPasswordToken(ResetPassword obj);
        public void removeResetPasswordToekn(ResetPassword obj);
        public string getCityFromCityId(long cityId);
        public string getCountryFromCountryId(long countryId);
        public List<Country> getCountryNames();
        public List<City> getCityNames(int countryId);
        public List<Skill> getSkillNames();
        public long getIdOfUserSkill(string userSkillName);
        public List<UserSkill> getSkillsOfUser(int userId);
        public void editUserProfile(UserProfileModel userProfileModel);
        public void addTimeBasedVolunteerTimesheet(Timesheet timesheet);
    }
}
