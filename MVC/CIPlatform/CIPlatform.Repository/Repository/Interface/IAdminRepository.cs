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
        public AdminPageList<Mission> getMissions(string? searchText, int pageNumber, int pageSize);
        public void DeleteMission(long missionId);
        public void AddMission(Mission mission, List<MissionSkill> missionSkills, GoalMission goalMission, List<MissionMedium> missionMedia, List<MissionDocument> missionDocument);
        public void EditMission(Mission mission, List<MissionSkill> missionSkills, GoalMission goalMission, List<MissionMedium> missionMedia, List<MissionDocument> missionDocument);
        public Mission FindMissionById(long missionId);
        public AdminMissionModel GetMissionDetails(long missionId);
        public AdminPageList<Banner> GetBanners(string? searchText, int pageNumber, int pageSize);
        public void addBanner(Banner banner);
        public void deleteBanner(long bannerId);
        public Banner findBannerById(long bannerId);
        public void editBanner(Banner banner);
        public AdminPageList<Skill> GetSkills(string? searchText, int pageNumber, int pageSize);
        public void DeleteSkill(long skillId);
        public void AddSkill(Skill skill);
        public Skill FindSkill(long skillId);
        public void EditSkill(Skill skill);
        public AdminPageList<AdminMissionApplicationListModel> GetMissionApplications(string? searchText, int pageNumber, int pageSize);
        public void ApproveMissionApplication(long missionApplicationId);
        public void RejectMissionApplication(long missionApplicationId);
        public AdminPageList<AdminStoryListModel> GetStories(string? searchText, int pageNumber, int pageSize);
        public void ApproveStory(long storyId);
        public void RejectStory(long storyId);
        public AdminPageList<MissionTheme> GetThemes(string? searchText, int pageNumber, int pageSize);
        public void AddTheme(MissionTheme theme);
        public void DeleteTheme(long themeId);
        public MissionTheme FindTheme(long themeId);
        public void EditTheme(MissionTheme theme);
        public List<CountryList> GetCountryLists();
        public List<CityList> GetCityLists(long countryId);
        public List<ThemeList> GetThemeLists();
        public List<SkillList> GetSkillLists();
    }
}
