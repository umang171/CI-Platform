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
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? OrganizationName { get; set; }
        public string? MediaName { get; set; }
        public string? MediaType { get; set; }
        public string? MediaPath { get; set; }
        public string? ThemeTitle { get; set; }
        public string CityName { get; set; }
    }
}
