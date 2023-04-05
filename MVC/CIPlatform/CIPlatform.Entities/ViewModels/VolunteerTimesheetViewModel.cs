using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class VolunteerTimesheetViewModel
    {
        public HeaderViewModel headerViewModel=new HeaderViewModel();
        public List<MissionApplication> missions { get; set; }=new List<MissionApplication>();

    }
}
