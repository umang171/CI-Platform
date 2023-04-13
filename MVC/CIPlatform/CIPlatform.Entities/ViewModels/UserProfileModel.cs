using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class UserProfileModel
    {
        public long UserId { get; set; }
        [Required]
        public string? FirstName { get; set; } = string.Empty;

        [Required]
        public string? LastName { get; set; }= string.Empty;

        public string? Email { get; set; }= string.Empty;
        [Required]
        [MinLength(8, ErrorMessage = "Please enter more than 8 characters")]
        public string? OldPassword { get; set; } = string.Empty;
        [Required]
        [MinLength(8, ErrorMessage = "Please enter more than 8 characters")]
        public string? NewPassword { get; set; }= string.Empty;
        [Required]
        [MinLength(8, ErrorMessage = "Please enter more than 8 characters")]
        [Compare("NewPassword")]
        public string? ConfirmPassword { get; set; }= string.Empty;

        public long? PhoneNumber { get; set; }

        public string? Avatar { get; set; } = string.Empty;

        public string? WhyIVolunteer { get; set; }= string.Empty;

        public string? EmployeeId { get; set; }= string.Empty;

        public string? Department { get; set; }= string.Empty;
        [Required]
        public long? CityId { get; set; }
        public string? CityName { get; set; } = string.Empty;
        [Required]
        public long? CountryId { get; set; }
        public string? CountryName { get; set; } = string.Empty;

        public List<Country> CountryNames { get; set; }= new List<Country>();
        public List<City> CityNames { get; set; }=new List<City>();

        public string? ProfileText { get; set; } = string.Empty;

        public string? LinkedInUrl { get; set; } = string.Empty;

        public string? Title { get; set; } = string.Empty;
        public List<Skill> skills { get; set; } = new List<Skill> ();
        public List<UserSkill> userSkills { get; set; }=new List<UserSkill> ();
        public string userSkillNames { get; set; } = string.Empty;
    }
}
