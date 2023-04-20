using CIPlatform.Entities.DataModels;
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
        public string username { get; set; } = string.Empty;
        public string avtar { get; set; } = string.Empty;
        public IEnumerable<User> users { get; set; }=new List<User>();
        public int userid{ get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? ShortDescription { get; set; } = string.Empty;
        public string ? Description { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? MissionType { get; set; } = string.Empty;
        public string? GoalObjective { get; set; } = string.Empty;
        public int? GoalValue { get; set; }
        public string? OrganizationName { get; set; } = string.Empty;
        public string? OrganizationDetail { get; set; } = string.Empty;
        public string? MediaName { get; set; } = string.Empty;
        public string? MediaType { get; set; } = string.Empty;
        public string? MediaPath { get; set; } = string.Empty;
        public string? DocumentName { get; set; } = string.Empty;
        public string? DocumentType { get; set; } = string.Empty;
        public string? DocumentPath { get; set; } = string.Empty;
        public string? ThemeTitle { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string? Skill { get; set; } = null;
        public string Availability { get; set; } = string.Empty;
        public int ? Rating { get; set; }
        public int? TotalVoulunteerRating { get; set; }
        public long? SeatsLeft { get; set; }
        public int? AchievedGoal { get; set; }

    }
}
