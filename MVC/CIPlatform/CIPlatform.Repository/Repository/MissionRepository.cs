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

        IEnumerable<City> IMissionRepository.getCities()
        {
            return _ciPlatformDbContext.Cities;
        }

        IEnumerable<Country> IMissionRepository.getCountries()
        {
            return _ciPlatformDbContext.Countries;

        }

        IEnumerable<Skill> IMissionRepository.getSkills()
        {
            return _ciPlatformDbContext.Skills;
        }

        IEnumerable<MissionTheme> IMissionRepository.getThemes()
        {
            return _ciPlatformDbContext.MissionThemes;
        }

        public IEnumerable<MissionViewModel> getMissionsFromSP(string countryNames,string cityNames,string themeNames,string skillNames,string searchText, string sortValue)
        {
            IEnumerable<MissionViewModel> missionViewModelObj = _ciPlatformDbContext.MissionViewModel.FromSqlInterpolated($"exec sp_get_mission_data @countryNames={countryNames},@cityNames={cityNames},@themeNames={themeNames},@skillNames={skillNames},@searchText={searchText},@sortValue={sortValue}");
            return missionViewModelObj;
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
            SqlConnection conn = null;

            SqlDataReader rdr = null;
            conn = new SqlConnection("Server=PCTR29\\SQL2017;User Id=sa;Password=tatva123;Database=CI;Trusted_Connection=true;TrustServerCertificate=true;");
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
                missionVolunteerViewModelObj.StartDate= rdr.GetDateTime("StartDate");
                missionVolunteerViewModelObj.EndDate= rdr.GetDateTime("EndDate");
                missionVolunteerViewModelObj.OrganizationName= rdr.GetString("OrganizationName");
                missionVolunteerViewModelObj.MediaName= rdr.GetString("MediaName");
                missionVolunteerViewModelObj.MediaType= rdr.GetString("MediaType");
                missionVolunteerViewModelObj.MediaPath= rdr.GetString("MediaPath");
                missionVolunteerViewModelObj.ThemeTitle= rdr.GetString("ThemeTitle");
                missionVolunteerViewModelObj.CityName= rdr.GetString("cityName");
            }
            


            return missionVolunteerViewModelObj;
        }

        IEnumerable<FavouriteMission> IMissionRepository.getFavouriteMissionsOfUser(int userid)
        {
            return _ciPlatformDbContext.FavouriteMissions.Where(u=>u.UserId==userid);
        }
    }
}
