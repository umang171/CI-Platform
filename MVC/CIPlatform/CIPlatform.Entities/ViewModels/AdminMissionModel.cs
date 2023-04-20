using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class AdminMissionModel
    {
        public AdminHeader adminHeader { get; set; }=new AdminHeader();
        public long MissionId { get; set; }
        [Required]
        public string MissionTitle { get; set; } =String.Empty;
        [Required]
        public string ShortDescription { get; set; }=String.Empty;
        [Required]
        public string Description { get; set; } = String.Empty;
        [Required]
        public string MissionType { get; set; } = String.Empty;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public long? GoalValue{ get; set; }
        public string? GoalObjective{ get; set; } = String.Empty;
        public long CountryId { get; set; }
        public long CityId { get; set; }
        [Required]
        public long TotalSeats{ get; set; }
        public long ThemeId { get; set; }
        public string Skills { get; set; }= String.Empty;
        public string OrganizationName { get; set; } = String.Empty;
        public string OrganizationDetail { get; set; } = String.Empty;
        public string Availability { get; set; } = String.Empty;
        public string MissionMedia { get; set; } = String.Empty;
        public string? MediaName { get; set; } = String.Empty;
        public string? MediaType { get; set; } = String.Empty;
        public string? MediaPath { get; set; } = String.Empty;
        public string? DocumentName { get; set; } = String.Empty;
        public string? DocumentType { get; set; } = String.Empty;
        public string? DocumentPath { get; set; } = String.Empty;
        public string? MissionDocuments { get; set; } = String.Empty;
        public List<CountryList> countryLists { get; set; }= new List<CountryList>();
        public List<CityList> cityLists { get; set; }= new List<CityList>();
        public List<ThemeList> themeLists { get; set; }= new List<ThemeList>();
        public List<SkillList> skillLists { get; set; }= new List<SkillList>();
    }
    public class CountryList
    {
        public long CountryId { get; set; }
        public string CountryName { get; set; } = String.Empty;
    }
    public class CityList
    {
        public long CityId { get; set; }
        public string CityName { get; set; } = String.Empty;
    }
    public class ThemeList
    {
        public long ThemeId { get; set; }
        public string ThemeName { get; set; } = String.Empty;
    }
    public class SkillList
    {
        public long SkillId { get; set; }
        public string SkillName { get; set; } = String.Empty;
    }
}

