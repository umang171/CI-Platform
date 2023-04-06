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
        public List<Mission?> timeBasedMissions { get; set; }=new List<Mission?>();
        public List<Mission?> goalBasedMissions { get; set; }=new List<Mission?>();

    }
}
