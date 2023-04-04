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
        public string? storyTitle { get; set; } = string.Empty;
        public string? storyPublishedDate { get; set; }= string.Empty;
        public string? storyDescription { get; set; }= string.Empty;
        public string? storyVideoUrl { get; set; }= string.Empty;
        public string? storyFileNames { get; set; }= string.Empty;
    }
}
