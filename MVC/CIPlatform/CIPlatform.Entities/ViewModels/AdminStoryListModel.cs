using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class AdminStoryListModel
    {
        public long StoryId { get; set; }
        public string StoryTitle { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;
        public string MissionTitle { get; set; } = String.Empty;
    }
}
