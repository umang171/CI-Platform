using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class AdminCMSModel
    {
        public AdminHeader adminHeader { get; set; }= new AdminHeader();
        [Required(ErrorMessage = "Please Enter Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Enter desciption")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please Enter slug")]
        public string Slug { get; set; } = string.Empty;
        public string Status { get; set; }
        public long CMSPageId{ get; set; }

    }
}
