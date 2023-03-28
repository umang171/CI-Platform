using CIPlatform.Entities.DataModels;
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
        public PaginationStory getStories(int pageNumber);
        public int saveStories(StorySaveModel storySaveModelObj);
        public void submitStories(StorySaveModel storySaveModelObj);
        public Story getStoryDetail(int storyId); 
        public int getTotalStoryViews(int storyId);
        public void recommendToCoworker(int fromUserId, int toUserId, int storyId);

    }
}
