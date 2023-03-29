using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class MissionVolunteerViewModel
    {
        public long MissionId { get; set; }
        public string username { get; set; }
        public string avtar { get; set; }
        public int userid{ get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string ? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? MissionType { get; set; }
        public string? GoalObjective { get; set; }
        public int? GoalValue { get; set; }
        public string? OrganizationName { get; set; }
        public string? OrganizationDetail { get; set; }
        public string? MediaName { get; set; }
        public string? MediaType { get; set; }
        public string? MediaPath { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentPath { get; set; }
        public string? ThemeTitle { get; set; }
        public string CityName { get; set; }
        public string? Skill { get; set; } = null;
        public string Availability { get; set; }
        public int ? Rating { get; set; }
        public int? TotalVoulunteerRating { get; set; }
    }
}
