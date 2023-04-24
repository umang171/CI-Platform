using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class StoryDetailsModel
    {
        public string username { get; set; } = string.Empty;
        public string avtar { get; set; } = string.Empty;
        public Story story { get; set; }=new Story();
        public IEnumerable<User> users { get; set; }=new List<User>();
        public List<CmsPage> cmsPages { get; set; } = new List<CmsPage>();

    }
}
