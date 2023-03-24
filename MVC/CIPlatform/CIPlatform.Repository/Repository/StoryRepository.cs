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
            List<StoryListingModel> storyListingObj=_ciPlatformDbContext.StoryListingModel.FromSqlInterpolated($"exec sp_get_story_data @pageNumber = {pageNumber}, @TotalCount = {output} out").ToList();
            paginationStory.stories = storyListingObj;
            paginationStory.pageCount= long.Parse(output.Value.ToString());
            paginationStory.pageSize = 6;
            paginationStory.activePage= pageNumber;
            return paginationStory;
        }
    }
}
