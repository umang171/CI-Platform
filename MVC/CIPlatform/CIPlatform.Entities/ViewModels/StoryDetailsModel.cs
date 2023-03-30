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
        public string username { get; set; }
        public string avtar { get; set; }
        public Story story { get; set; }
        public IEnumerable<User> users { get; set; }
    }
}
