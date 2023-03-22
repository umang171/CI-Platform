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
    public class StoryRepository : IStoryRepository
    {
        private readonly CIPlatformDbContext _ciPlatformDbContext;
        public StoryRepository(CIPlatformDbContext cIPlatformDbContext)
        {
            _ciPlatformDbContext = cIPlatformDbContext;
        }
        IEnumerable<StoryListingModel> IStoryRepository.getStories(int missionId)
        {
            return _ciPlatformDbContext.StoryListingModel.FromSqlInterpolated($"exec sp_get_story_data @missionId = {missionId}");
        }
    }
}
