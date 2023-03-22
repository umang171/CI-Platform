using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class PaginationStory
    {
        public long? pageCount { get; set; }
        public long? activePage { get; set; }
        public long? pageSize { get; set; }
        public List<StoryListingModel> stories { get; set; }
    }
}
