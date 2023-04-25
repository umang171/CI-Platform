using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class AdminSkillModel
    {
        public AdminHeader adminHeader { get; set; } = new AdminHeader();
        [Required(ErrorMessage = "Please Enter Skill Name")]
        public string SkillName { get; set; } = string.Empty;
        public long SkillId{ get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
