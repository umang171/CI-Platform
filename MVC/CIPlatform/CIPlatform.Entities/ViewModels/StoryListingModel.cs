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
        public string StoryTitle { get; set; } = string.Empty;
        public string StoryDescription { get; set; }= string.Empty;
        public string ThemeTitle { get; set; }= string.Empty;
        public string ImageType { get; set; }= string.Empty;
        public string Path { get; set; }= string.Empty;
        public string Username { get; set; }= string.Empty;
        public string Avtar { get; set; }= string.Empty;

    }
}
