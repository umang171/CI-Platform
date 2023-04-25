using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class AdminThemeModel
    {
        public AdminHeader adminHeader { get; set; } = new AdminHeader();
        [Required]
        public string ThemeName { get; set; }=String.Empty;
        public long ThemeId{ get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
