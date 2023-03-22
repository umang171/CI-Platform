using CIPlatform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repository.Repository.Interface
{
    public interface IStoryRepository
    {
        public IEnumerable<StoryListingModel> getStories(int missionId);
    }
}
