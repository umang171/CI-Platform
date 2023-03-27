using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class StorySaveModel
    {
        public int? userId { get; set; }
        public int? missionId{ get; set; }
        public string? storyTitle { get; set; }
        public string? storyPublishedDate { get; set; }
        public string? storyDescription { get; set; }
        public string? storyVideoUrl { get; set; }
        public string? storyFileNames { get; set; }
    }
}
