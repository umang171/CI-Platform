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

        public IEnumerable<Mission> getMissions();
        public IEnumerable<Mission> searchMissions(string searchText);
        public IEnumerable<MissionViewModel> getMissionsFromSP();

    }
}
