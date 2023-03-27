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
        public string username { get; set; }
        public string avatar { get; set; }
        public long userId { get; set; }
        public List<MissionApplication> missions { get; set; }
    }
}
