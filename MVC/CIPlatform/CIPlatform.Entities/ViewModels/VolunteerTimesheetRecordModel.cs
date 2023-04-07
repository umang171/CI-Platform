using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class VolunteerTimesheetRecordModel
    {
        public long TimesheetId { get; set; }

        public long UserId { get; set; }

        public long MissionId { get; set; }
        public string? MissionName { get; set; }=String.Empty;

        public string? Time { get; set; }=String.Empty;

        public int? Action { get; set; }
        [Required]
        public DateTime DateVolunteered { get; set; }=DateTime.Now;
        [Required]
        public string? Notes { get; set; }=String.Empty;

    }
}
