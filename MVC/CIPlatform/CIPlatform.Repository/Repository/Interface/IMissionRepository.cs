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
        public IEnumerable<Country> getCountries();
        public IEnumerable<City> getCities();
        public IEnumerable<MissionTheme> getThemes();
        public IEnumerable<Skill> getSkills();

        public IEnumerable<MissionViewModel> getMissionsFromSP(string countryNames,string cityNames,string themeNames,string skillNames,string searchText,string sortValue);
        public PaginationMission gridSP(string countryNames, string cityNames, string themeNames, string skillNames, string searchText, string sortValue,int pageNumber);

        public void addFavouriteMission(FavouriteMission favouriteMissionObj);
        public void removeFavouriteMission(FavouriteMission favouriteMissionObj);
        public FavouriteMission getFavouriteMission(FavouriteMission favouriteMissionObj);
        public IEnumerable<FavouriteMission> getFavouriteMissionsOfUser(int userid);
        public MissionVolunteerViewModel getMissionFromMissionId(int missionId);
    }
}
