using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class ShareStoryModel
    {
        public string username { get; set; } = string.Empty;
        public string avatar { get; set; } = string.Empty;
        public long userId { get; set; }
        public List<MissionApplication> missions { get; set; }=new List<MissionApplication>();
    }
}
