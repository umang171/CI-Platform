using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repository.Repository.Interface
{
    public interface IMissionRepository
    {
        public List<Mission> getMissions();
        public List<MissionApplication> getMissionsOfUser(int userId);
        public List<Mission?> getTimeMissionsOfUser(int userId);
        public List<Mission?> getGoalMissionsOfUser(int userId);
        public IEnumerable<Country> getCountries();
        public IEnumerable<City> getCities(string country);
        public IEnumerable<MissionTheme> getThemes();
        public IEnumerable<Skill> getSkills();

        public PaginationMission getMissionsFromSP(string countryNames, string cityNames, string themeNames, string skillNames, string searchText, string sortValue, string exploreValue, int pageNumber,int userId);

        public void addFavouriteMission(FavouriteMission favouriteMissionObj);
        public void removeFavouriteMission(FavouriteMission favouriteMissionObj);
        public FavouriteMission getFavouriteMission(FavouriteMission favouriteMissionObj);
        public IEnumerable<FavouriteMission> getFavouriteMissionsOfUser(int userid);
        public MissionVolunteerViewModel getMissionFromMissionId(int missionId);
        public MissionRating getRatingOfUser(int userId,int missionId);
        public void addRatingStars(int userId,int missionId,int ratingStars);
        public IEnumerable<Mission> getRelatedMissions(string themeName, string cityName,int missionId);
        public void addComment(int userId,int missionId,string comment);
        public IEnumerable<Comment> getComments(int missionId);
        public void recommendToCoworker(int fromUserId,int toUserId,int missionId);
        public void addToApplication(int missionId,int userId);
        public IEnumerable<MissionApplication> getRecentVolunteers(int missionId);
        public string getAppliedMissionOfUser(int userId,int missionId);
        public int getRatingOfUserForMission(int userId,int missionId);
        public List<CmsPage> GetCMSPages();
        public CmsPage GetCmsPageDetails(long cmsPageId);
        public void addNotification(Notification notification);
        public List<Notification> GetNotifications(long userId);
        public Notification GetNotification(long notificationId);
        public void ClearNotifications(long userId);
        public void changeStatusNotification(long notificationId);
    }
}
