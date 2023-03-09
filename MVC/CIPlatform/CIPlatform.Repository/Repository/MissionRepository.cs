using CIPlatform.Entities.DataModels;
using CIPlatform.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        IEnumerable<Mission> IMissionRepository.getMissions()
        {
            return _ciPlatformDbContext.Missions.Include(mission => mission.Country).ThenInclude(mission => mission.Cities).Include(mission => mission.Theme).Include(mission => mission.MissionMedia);
        }
        IEnumerable<Mission> IMissionRepository.searchMissions(string searchText)
        {
            return _ciPlatformDbContext.Missions.Where(mission => mission.Title.Contains(searchText)).Include(mission => mission.Country).ThenInclude(mission => mission.Cities).Include(mission => mission.Theme).Include(mission => mission.MissionMedia);
        }

    }
}
