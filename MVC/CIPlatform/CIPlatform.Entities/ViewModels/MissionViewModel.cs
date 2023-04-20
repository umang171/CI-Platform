using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class MissionViewModel
    {
        [Key]
        public long MissionId { get; set; }
        public string? Title { get; set; }= string.Empty;
        public string? ShortDescription { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? OrganizationName { get; set; } = string.Empty;
        public string? MissionType { get; set; } = string.Empty;
        public string? GoalObjective { get; set; } = string.Empty;
        public int? GoalValue { get; set; }
        public string? MediaName { get; set; } = string.Empty;
        public string? MediaType { get; set; } = string.Empty;
        public string? MediaPath { get; set; } = string.Empty;
        public string? ThemeTitle { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public int? Rating { get; set; }
        public long? SeatsLeft { get; set; }
        public int? AchievedGoal { get; set; }
    }
}
