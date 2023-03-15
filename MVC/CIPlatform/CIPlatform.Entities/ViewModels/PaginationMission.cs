using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class PaginationMission
    {
        public long? pageCount { get; set; }
        public long? activePage { get; set; }
        public long? pageSize { get; set; }
        public long? missionCount { get; set; }
        public List<MissionViewModel> missions{ get; set; }
    }
}
