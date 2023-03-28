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
    public class StoryRepository : IStoryRepository
    {
        private readonly CIPlatformDbContext _ciPlatformDbContext;
        public StoryRepository(CIPlatformDbContext cIPlatformDbContext)
        {
            _ciPlatformDbContext = cIPlatformDbContext;
        }
        PaginationStory IStoryRepository.getStories(int pageNumber)
        {
            var output = new SqlParameter("@TotalCount", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            PaginationStory paginationStory = new PaginationStory();
            List<StoryListingModel> storyListingObj = _ciPlatformDbContext.StoryListingModel.FromSqlInterpolated($"exec sp_get_story_data @pageNumber = {pageNumber}, @TotalCount = {output} out").ToList();
            paginationStory.stories = storyListingObj;
            paginationStory.pageCount = long.Parse(output.Value.ToString());
            paginationStory.pageSize = 6;
            paginationStory.activePage = pageNumber;
            return paginationStory;
        }

        Story IStoryRepository.getStoryDetail(int storyId)
        {
            Story story=_ciPlatformDbContext.Stories.Where(u=>u.StoryId == storyId).Include(u=>u.User).Include(u=>u.StoryMedia).First();
            return story;
        }

        int IStoryRepository.getTotalStoryViews(int storyId)
        {
            Story story=_ciPlatformDbContext.Stories.Where(u => u.StoryId == storyId).First();
            if (story.TotalViews == null)
                story.TotalViews = 0;
            story.TotalViews++;
            _ciPlatformDbContext.Update(story);
            _ciPlatformDbContext.SaveChanges();
            return (int)story.TotalViews;

        }     

        void IStoryRepository.recommendToCoworker(int fromUserId, int toUserId, int storyId)
        {
            StoryInvite storyInvite = new StoryInvite();
            storyInvite.FromUserId= fromUserId;
            storyInvite.ToUserId = toUserId;
            storyInvite.StoryId=storyId;
            _ciPlatformDbContext.StoryInvites.Add(storyInvite);
            _ciPlatformDbContext.SaveChanges();
        }

        int IStoryRepository.saveStories(StorySaveModel storySaveModelObj)
        {
            Story story = new Story();
            story.UserId = (long)storySaveModelObj.userId;
            story.MissionId = (long)storySaveModelObj.missionId;
            story.Title = storySaveModelObj.storyTitle;
            story.Description = storySaveModelObj.storyDescription;
            story.PublishedAt = DateTime.Parse(storySaveModelObj.storyPublishedDate);
            _ciPlatformDbContext.Stories.Add(story);
            _ciPlatformDbContext.SaveChanges();

            long storyId = story.StoryId;
            StoryMedium storyMediumObj = new StoryMedium();
            storyMediumObj.StoryId = storyId;
            string type = storySaveModelObj.storyFileNames.Substring(storySaveModelObj.storyFileNames.Length - 5, 4);
            string mediaPath = storySaveModelObj.storyFileNames.Replace(type, "");
            storyMediumObj.Type = type;
            storyMediumObj.Path = mediaPath;
            _ciPlatformDbContext.StoryMedia.Add(storyMediumObj);
            _ciPlatformDbContext.SaveChanges();
            return (int)storyId;
        }
        void IStoryRepository.submitStories(StorySaveModel storySaveModelObj)
        {
            if (!_ciPlatformDbContext.Stories.Any(u => u.UserId == storySaveModelObj.userId && u.MissionId == storySaveModelObj.missionId && u.Title == storySaveModelObj.storyTitle))
            {
                Story story = new Story();
                story.UserId = (long)storySaveModelObj.userId;
                story.MissionId = (long)storySaveModelObj.missionId;
                story.Title = storySaveModelObj.storyTitle;
                story.Description = storySaveModelObj.storyDescription;
                story.PublishedAt = DateTime.Parse(storySaveModelObj.storyPublishedDate);
                story.Status = "1";
                _ciPlatformDbContext.Stories.Add(story);
                _ciPlatformDbContext.SaveChanges();

                long storyId = story.StoryId;
                StoryMedium storyMediumObj = new StoryMedium();
                storyMediumObj.StoryId = storyId;
                string type = storySaveModelObj.storyFileNames.Substring(storySaveModelObj.storyFileNames.Length - 5, 4);
                string mediaPath = storySaveModelObj.storyFileNames.Replace(type, "");
                storyMediumObj.Type = type;
                storyMediumObj.Path = mediaPath;
                _ciPlatformDbContext.StoryMedia.Add(storyMediumObj);
                _ciPlatformDbContext.SaveChanges();
            }
            else
            {
                Story story = _ciPlatformDbContext.Stories.Where(u => u.UserId == storySaveModelObj.userId && u.MissionId == storySaveModelObj.missionId && u.Title == storySaveModelObj.storyTitle).First();
                story.UserId = (long)storySaveModelObj.userId;
                story.MissionId = (long)storySaveModelObj.missionId;
                story.Title = storySaveModelObj.storyTitle;
                story.Description = storySaveModelObj.storyDescription;
                story.PublishedAt = DateTime.Parse(storySaveModelObj.storyPublishedDate);
                story.Status = "1";
                _ciPlatformDbContext.Stories.Update(story);
                _ciPlatformDbContext.SaveChanges();
                long storyId = story.StoryId;
                if (!_ciPlatformDbContext.StoryMedia.Any(u => u.StoryId == storyId))
                {
                    StoryMedium storyMediumObj = new StoryMedium();
                    storyMediumObj.StoryId = storyId;
                    string type = storySaveModelObj.storyFileNames.Substring(storySaveModelObj.storyFileNames.Length - 5, 4);
                    string mediaPath = storySaveModelObj.storyFileNames.Replace(type, "");
                    storyMediumObj.Type = type;
                    storyMediumObj.Path = mediaPath;
                    _ciPlatformDbContext.StoryMedia.Add(storyMediumObj);
                    _ciPlatformDbContext.SaveChanges();
                }
                else
                {
                    StoryMedium storyMediumObj =_ciPlatformDbContext.StoryMedia.Where(u=>u.StoryId==storyId).First();
                    storyMediumObj.StoryId = storyId;
                    string type = storySaveModelObj.storyFileNames.Substring(storySaveModelObj.storyFileNames.Length - 5, 4);
                    string mediaPath = storySaveModelObj.storyFileNames.Replace(type, "");
                    storyMediumObj.Type = type;
                    storyMediumObj.Path = mediaPath;
                    _ciPlatformDbContext.StoryMedia.Update(storyMediumObj);
                    _ciPlatformDbContext.SaveChanges();
                }
            }
        }
    }
}
