using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class AdminMissionApplicationListModel
    {
        public long MissionApplicationId { get; set; }
        public string MissionTitle { get; set; } = String.Empty;
        public long MissionId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; } = String.Empty;
        public string AppliedDate { get; set; } = String.Empty;
    }
}
