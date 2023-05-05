using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.ViewModels;
using CIPlatform.Repository.Repository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repository.Repository
{
    public class MissionRepository:IMissionRepository
    {
        private readonly CIPlatformDbContext _ciPlatformDbContext;
        public MissionRepository(CIPlatformDbContext cIPlatformDbContext)
        {
            _ciPlatformDbContext = cIPlatformDbContext;
        }

        List<Mission> IMissionRepository.getMissions()
        {
            return _ciPlatformDbContext.Missions.Where(mission=>mission.DeletedAt==null && mission.Status==true).ToList();
        }
        IEnumerable<City> IMissionRepository.getCities(string country)
        {            
            string[] arr;
            if (country == null)
                return _ciPlatformDbContext.Cities;
            else
            {
                string[] values = country.Split(',');
                values = values.SkipLast(1).ToArray();
                IEnumerable<City> cities = values.SelectMany(a => _ciPlatformDbContext.Cities.Where(u => u.Country.Name == a));
                return cities;
            }
        }

        IEnumerable<Country> IMissionRepository.getCountries()
        {
            return _ciPlatformDbContext.Countries;

        }
       
        IEnumerable<Skill> IMissionRepository.getSkills()
        {

            List<Skill> skills =_ciPlatformDbContext.Skills.ToList();
            List<Skill> originalskills=_ciPlatformDbContext.Skills.Where(skill => skill.DeletedAt == null && skill.Status == 1).ToList();
            foreach (Skill skill in skills)
            {

                List<Mission> mission=_ciPlatformDbContext.Missions.Where(u => u.MissionSkills.ElementAt(0).Skill.SkillName == skill.SkillName).ToList();
                if (!mission.Any())
                {
                    originalskills.Remove(_ciPlatformDbContext.Skills.Where(u=>u.SkillName == skill.SkillName).First());

                }                
            }
            return originalskills;
        }

        IEnumerable<MissionTheme> IMissionRepository.getThemes()
        {
            List<MissionTheme> themes=_ciPlatformDbContext.MissionThemes.ToList();
            List<MissionTheme> originalThemes=_ciPlatformDbContext.MissionThemes.Where(theme => theme.DeletedAt == null && theme.Status == true).ToList();
            foreach(MissionTheme theme in themes)
            {
                List<Mission> mission = _ciPlatformDbContext.Missions.Where(u => u.Theme.Title== theme.Title).ToList();
                if (!mission.Any())
                {
                    originalThemes.Remove(_ciPlatformDbContext.MissionThemes.Where(u=>u.Title==theme.Title).First());

                }
            }
            return originalThemes.AsEnumerable();
        }

        


        public PaginationMission getMissionsFromSP(string countryNames, string cityNames, string themeNames, string skillNames, string searchText, string sortValue, string exploreValue, int pageNumber, int userId)
        {
            // make explicit SQL Parameter
            var output = new SqlParameter("@TotalCount", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            var output1 = new SqlParameter("@MissionCount", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            PaginationMission pagination = new PaginationMission();
            List<MissionViewModel> test = _ciPlatformDbContext.MissionViewModel.FromSqlInterpolated($"exec sp_get_mission_data @countryNames = {countryNames}, @cityNames = {cityNames}, @themeNames = {themeNames}, @skillNames = {skillNames}, @searchText = {searchText}, @sortValue = {sortValue},@exploreValue={exploreValue}, @pageNumber = {pageNumber}, @userId = {userId}, @TotalCount = {output} out,@MissionCount={output1} out").ToList();
            pagination.missions = test;
            pagination.pageSize = 9;
            pagination.pageCount = long.Parse(output.Value.ToString());
            pagination.missionCount = long.Parse(output1.Value.ToString());
            pagination.activePage = pageNumber;
            return  pagination;
        }

        void IMissionRepository.addFavouriteMission(FavouriteMission favouriteMissionObj)
        {
            _ciPlatformDbContext.FavouriteMissions.Add(favouriteMissionObj);
            _ciPlatformDbContext.SaveChanges();
        }

        void IMissionRepository.removeFavouriteMission(FavouriteMission favouriteMissionObj)
        {
            _ciPlatformDbContext.FavouriteMissions.Remove(favouriteMissionObj);
            _ciPlatformDbContext.SaveChanges();
        }
        FavouriteMission IMissionRepository.getFavouriteMission(FavouriteMission favouriteMissionObj)
        {
            FavouriteMission favouriteMission= _ciPlatformDbContext.FavouriteMissions.Where(u=>u.UserId==favouriteMissionObj.UserId && u.MissionId==favouriteMissionObj.MissionId).First();
            
            return favouriteMission;
        }

        MissionVolunteerViewModel IMissionRepository.getMissionFromMissionId(int missionId)
        {
            MissionVolunteerViewModel missionVolunteerViewModelObj=new MissionVolunteerViewModel();
            SqlConnection conn = new SqlConnection();

            SqlDataReader rdr = null;
            conn = new SqlConnection("Server=PCTR29\\SQL2017;User Id=sa;Password=tatva123;Database=CI;Trusted_Connection=true;TrustServerCertificate=true;");
            //conn = new SqlConnection("SERVER=localhost\\SQLEXPRESS;Database=CI;Trusted_Connection=true;TrustServerCertificate=true;");
            conn.Open();
            SqlCommand cmd = new SqlCommand("sp_get_mission_data_from_id", conn);
            // 2. set the command object so it knows to execute a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;
            // 3. add parameter to command, which will be passed to the stored procedure
            cmd.Parameters.Add(new SqlParameter("@missionId", missionId));
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                missionVolunteerViewModelObj.MissionId= rdr.GetInt64("MissionId");
                missionVolunteerViewModelObj.Title= rdr.GetString("Title");
                missionVolunteerViewModelObj.ShortDescription= rdr.GetString("ShortDescription");
                missionVolunteerViewModelObj.StartDate = !rdr.IsDBNull("StartDate") ? rdr.GetDateTime("StartDate") : null;
                missionVolunteerViewModelObj.EndDate= !rdr.IsDBNull("EndDate") ? rdr.GetDateTime("EndDate") : null;
                missionVolunteerViewModelObj.MissionType= rdr.GetString("MissionType");
                missionVolunteerViewModelObj.GoalObjective = !rdr.IsDBNull("GoalObjective") ? rdr.GetString("GoalObjective") : null;
                missionVolunteerViewModelObj.GoalValue = !rdr.IsDBNull("GoalValue") ? rdr.GetInt32("GoalValue") : null;
                missionVolunteerViewModelObj.OrganizationName= rdr.GetString("OrganizationName");
                missionVolunteerViewModelObj.OrganizationDetail= rdr.GetString("OrganizationDetail");
                missionVolunteerViewModelObj.MediaName = rdr.GetString("MediaName");
                missionVolunteerViewModelObj.MediaType = rdr.GetString("MediaType");
                missionVolunteerViewModelObj.MediaPath = rdr.GetString("MediaPath"); 
                missionVolunteerViewModelObj.DocumentName =!rdr.IsDBNull("DocumentName")? rdr.GetString("DocumentName"):null;
                missionVolunteerViewModelObj.DocumentType = !rdr.IsDBNull("DocumentPath")? rdr.GetString("DocumentType"):null;
                missionVolunteerViewModelObj.DocumentPath = !rdr.IsDBNull("DocumentPath") ? rdr.GetString("DocumentPath") : null;
                missionVolunteerViewModelObj.ThemeTitle= rdr.GetString("ThemeTitle");
                missionVolunteerViewModelObj.CityName= rdr.GetString("cityName");
                missionVolunteerViewModelObj.Skill = !rdr.IsDBNull("Skill") ? rdr.GetString("Skill") : null;
                missionVolunteerViewModelObj.Availability = rdr.GetString("Availability");
                missionVolunteerViewModelObj.Rating = ! rdr.IsDBNull("Rating")?rdr.GetInt32("Rating"):0;
                missionVolunteerViewModelObj.TotalVoulunteerRating = rdr.GetInt32("TotalVoulunteerRating");
                missionVolunteerViewModelObj.Description = rdr.GetString("Description");
                missionVolunteerViewModelObj.SeatsLeft = rdr.GetInt64("SeatsLeft");
                missionVolunteerViewModelObj.AchievedGoal = rdr.IsDBNull("AchievedGoal")? 0 : rdr.GetInt32("AchievedGoal");
            }
            conn.Close();
            return missionVolunteerViewModelObj;
        }

        IEnumerable<FavouriteMission> IMissionRepository.getFavouriteMissionsOfUser(int userid)
        {
            return _ciPlatformDbContext.FavouriteMissions.Where(u=>u.UserId==userid);
        }

        public MissionRating getRatingOfUser(int userId, int missionId)
        {
            return _ciPlatformDbContext.MissionRatings.Where(r => r.UserId == userId && r.MissionId == missionId).First();
        }
        void IMissionRepository.addRatingStars(int userId, int missionId, int ratingStars)
        {
            MissionRating missionRating = new MissionRating();
            missionRating.UserId = userId;
            missionRating.MissionId = missionId;
            missionRating.Rating = (byte)ratingStars;
            bool hasAlreadyRating=_ciPlatformDbContext.MissionRatings.Any(u => u.UserId == userId && u.MissionId == missionId);
            if (hasAlreadyRating)
            {
                    MissionRating missionRatingObj = _ciPlatformDbContext.MissionRatings.Where(r => r.UserId == userId && r.MissionId == missionId).First();
                    missionRatingObj.Rating = (byte)ratingStars;
                    _ciPlatformDbContext.MissionRatings.Update(missionRatingObj);
                    _ciPlatformDbContext.SaveChanges();
                
                
            }
            else
            {
                _ciPlatformDbContext.MissionRatings.Add(missionRating);
                _ciPlatformDbContext.SaveChanges();
            }
        }

        IEnumerable<Mission> IMissionRepository.getRelatedMissions(string themeName,string cityName, int missionId)
        {
            IEnumerable <Mission> cityRelatedMissions= _ciPlatformDbContext.Missions.Include(mission => mission.Country).Include(mission => mission.City).Include(mission => mission.Theme).Include(mission => mission.MissionSkills).Include(mission=>mission.Timesheets).Include(mission=>mission.MissionApplications).Include(mission => mission.MissionMedia).Include(mission=>mission.GoalMissions).Include(mission=>mission.MissionRatings).Where(u=>u.MissionId!=missionId).Where(u => u.City.Name == cityName).Where(mission => mission.DeletedAt == null && mission.Status == true).Take(3);

            if(cityRelatedMissions.Count()==3)
            {
                return cityRelatedMissions;
            }
            string countryName = _ciPlatformDbContext.Cities.Include(city => city.Country).Where(city => city.Name == cityName).First().Country.Name;
            int remainingRelatedMissions = 3-cityRelatedMissions.Count();
            IEnumerable<Mission> countryRelatedMissions=_ciPlatformDbContext.Missions.Include(mission => mission.Country).Include(mission => mission.City).Include(mission => mission.Theme).Include(mission=>mission.MissionSkills).Include(mission => mission.Timesheets).Include(mission => mission.MissionApplications).Include(mission => mission.MissionMedia).Include(mission => mission.GoalMissions).Include(mission => mission.MissionRatings).Where(u => u.City.Name != cityName).Where(u => u.MissionId != missionId).Where(u =>u.Country.Name== countryName).Where(mission => mission.DeletedAt == null && mission.Status == true).Take(remainingRelatedMissions);
            IEnumerable<Mission> relatedMissions=cityRelatedMissions.Concat(countryRelatedMissions);
            if(relatedMissions.Count()==3)
                return relatedMissions;

            remainingRelatedMissions = 3 - relatedMissions.Count();
            IEnumerable<Mission> themeRelatedMissions = _ciPlatformDbContext.Missions.Include(mission => mission.Country).Include(mission => mission.City).Include(mission => mission.Theme).Include(mission => mission.MissionSkills).Include(mission => mission.Timesheets).Include(mission => mission.MissionApplications).Include(mission => mission.MissionMedia).Include(mission => mission.GoalMissions).Include(mission => mission.MissionRatings).Where(u => u.Country.Name != countryName).Where(u => u.MissionId != missionId).Where(u => u.Theme.Title == themeName).Where(mission => mission.DeletedAt == null && mission.Status == true).Take(remainingRelatedMissions);
            IEnumerable<Mission> finalRelatedMissions = relatedMissions.Concat(themeRelatedMissions);
            return finalRelatedMissions;
        }

        void IMissionRepository.addComment(int userId, int missionId, string comment)
        {
            Comment commentObj=new Comment();
            commentObj.UserId=userId;
            commentObj.MissionId=missionId;
            commentObj.Comment1 = comment;
            _ciPlatformDbContext.Comments.Add(commentObj);
            _ciPlatformDbContext.SaveChanges();
        }

        IEnumerable<Comment> IMissionRepository.getComments(int missionId)
        {
            IEnumerable<Comment>  comments=_ciPlatformDbContext.Comments.Include(u => u.User).Where(u=>u.MissionId==missionId);
            return comments;
        }

        void IMissionRepository.recommendToCoworker(int fromUserId, int toUserId, int missionId)
        {
            MissionInvite missionInviteObj=new MissionInvite();
            missionInviteObj.FromUserId=fromUserId;
            missionInviteObj.ToUserId=toUserId;
            missionInviteObj.MissionId = missionId;
            _ciPlatformDbContext.MissionInvites.Add(missionInviteObj);
            _ciPlatformDbContext.SaveChanges();
        }

        void IMissionRepository.addToApplication(int missionId, int userId)
        {
            MissionApplication missionApplication=new MissionApplication();
            missionApplication.UserId=userId;
            missionApplication.MissionId=missionId;
            missionApplication.CreatedAt=DateTime.Now;
            missionApplication.AppliedAt=DateTime.Now;
            missionApplication.ApprovalStatus = "PENDING";
            bool hasAlreadyApplied=_ciPlatformDbContext.MissionApplications.Any(u=>u.MissionId==missionId && u.UserId==userId);
            if(!hasAlreadyApplied)                
                _ciPlatformDbContext.MissionApplications.Add(missionApplication);
            else
            {
                MissionApplication missionApp=_ciPlatformDbContext.MissionApplications.Where(u => u.MissionId == missionId && u.UserId == userId).First();
                missionApp.UserId=userId;
                missionApp.MissionId=missionId;
                missionApp.CreatedAt=DateTime.Now;
                missionApp.AppliedAt=DateTime.Now;
                missionApp.ApprovalStatus = "PENDING";
                _ciPlatformDbContext.MissionApplications.Update(missionApp);
            }

            _ciPlatformDbContext.SaveChanges();
        }

        IEnumerable<MissionApplication> IMissionRepository.getRecentVolunteers(int missionId)
        {
            return _ciPlatformDbContext.MissionApplications.Include(u => u.User).Where(u => u.MissionId == missionId && u.ApprovalStatus == "applied" && u.User.DeletedAt==null && u.User.Status==true).Take(9);
        }

        List<MissionApplication> IMissionRepository.getMissionsOfUser(int userId)
        {
            return _ciPlatformDbContext.MissionApplications.Include(u => u.Mission).Where(u => u.UserId == userId && u.ApprovalStatus == "applied").ToList();
        }
        List<Mission?> IMissionRepository.getTimeMissionsOfUser(int userId)
        {
            return _ciPlatformDbContext.MissionApplications.Where(u => u.UserId == userId && u.ApprovalStatus == "applied").Select(u => u.Mission).Where(mission=>mission.MissionType=="time").ToList();
        }
        List<Mission?> IMissionRepository.getGoalMissionsOfUser(int userId)
        {
            return _ciPlatformDbContext.MissionApplications.Where(u => u.UserId == userId && u.ApprovalStatus == "applied").Select(u => u.Mission).Where(mission => mission.MissionType == "goal").ToList();
        }

        string IMissionRepository.getAppliedMissionOfUser(int userId, int missionId)
        {
            if(_ciPlatformDbContext.MissionApplications.Any(mission => mission.UserId == userId && mission.MissionId == missionId)){
                return _ciPlatformDbContext.MissionApplications.Where(mission=>mission.UserId==userId && mission.MissionId==missionId).Select(missionApp=> missionApp.ApprovalStatus).First();
            }
            return "not applied";
        }

        int IMissionRepository.getRatingOfUserForMission(int userId, int missionId)
        {
            if(_ciPlatformDbContext.MissionRatings.Any(rating => rating.UserId == userId && rating.MissionId == missionId))
            {
                return _ciPlatformDbContext.MissionRatings.Where(rating => rating.UserId == userId && rating.MissionId == missionId).Select(rating => rating.Rating).First();
            }
            return 0;
        }

        List<CmsPage> IMissionRepository.GetCMSPages()
        {
            return _ciPlatformDbContext.CmsPages.Where(page => page.DeletedAt == null && page.Status==true).ToList();
        }

        CmsPage IMissionRepository.GetCmsPageDetails(long cmsPageId)
        {
            return _ciPlatformDbContext.CmsPages.Where(page => page.CmsPageId== cmsPageId).First();
        }

        MissionApplication IMissionRepository.GetMissionApplication(int missionApplicationId)
        {
            return _ciPlatformDbContext.MissionApplications.Where(mission => mission.MissionApplicationId == missionApplicationId).First();
        }
        Mission IMissionRepository.GetMission(int missionId)
        {
            return _ciPlatformDbContext.Missions.Where(mission => mission.MissionId == missionId).First();
        }
        void IMissionRepository.addNotification(Notification notification)
        {
            _ciPlatformDbContext.Add(notification);
            _ciPlatformDbContext.SaveChanges();
        }

        public List<Notification> GetNotifications(long userId,string selectedNotificatinSettings)
        {
            if (selectedNotificatinSettings is null || selectedNotificatinSettings == "")
            {
                return _ciPlatformDbContext.Notifications.Where(notification => notification.UserId == userId).OrderByDescending(notification => notification.CreatedAt).ToList();
            }
            string[] notificationTypes = selectedNotificatinSettings.Split(",").SkipLast(1).ToArray();
            return _ciPlatformDbContext.Notifications.Where(notification => notification.UserId == userId && notificationTypes.Contains(notification.NotificationType)).OrderByDescending(notification => notification.CreatedAt).ToList();

        }

        void IMissionRepository.ClearNotifications(long userId)
        {
            List<Notification> notifications = GetNotifications(userId,"");
            foreach (Notification notification in notifications)
            {
                _ciPlatformDbContext.Remove(notification);
                _ciPlatformDbContext.SaveChanges();
            }
        }
        public Notification GetNotification(long notificationId)
        {
            return _ciPlatformDbContext.Notifications.Where(notification => notification.NotificationId == notificationId).First();
        }

        void IMissionRepository.changeStatusNotification(long notificationId)
        {
            Notification notification = GetNotification(notificationId);
            if ((bool)notification.Status)
            {
                notification.Status = false;
            }
            else
            {
                notification.Status = true;
            }
            _ciPlatformDbContext.Update(notification);
            _ciPlatformDbContext.SaveChanges();
        }

    }
}
