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
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public long? PhoneNumber { get; set; }

        public string? Avatar { get; set; }

        public string? WhyIVolunteer { get; set; }

        public string? EmployeeId { get; set; }

        public string? Department { get; set; }
        [Required]
        public long? CityId { get; set; }
        public string? CityName { get; set; }
        [Required]
        public long? CountryId { get; set; }
        public string? CountryName { get; set; }
        
        public List<Country> CountryNames { get; set; }
        public List<City> CityNames { get; set; }

        public string? ProfileText { get; set; }

        public string? LinkedInUrl { get; set; }

        public string? Title { get; set; }
        public List<Skill> skills { get; set; }
        public List<UserSkill> userSkills { get; set; }
        public string userSkillNames { get; set; }
    }
}
