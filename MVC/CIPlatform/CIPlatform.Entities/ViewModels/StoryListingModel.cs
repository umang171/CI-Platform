using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class StoryListingModel
    {
        [Key]
        public long StoryId { get; set; }
        public string StoryTitle { get; set; }
        public string StoryDescription { get; set; }
        public string ThemeTitle { get; set; }
        public string ImageType { get; set; }
        public string Path { get; set; }
        public string Username { get; set; }
        public string Avtar { get; set; }

    }
}
