using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class AdminBannerModel
    {
        public AdminHeader adminHeader { get; set; }=new AdminHeader();
        [Required(ErrorMessage ="Please Enter text")]
        public string Text { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please Enter sort order")]
        public int SortOrder { get; set; }
        public string BannerImage { get; set; }=string.Empty;
        public long BannerID { get; set; }
    }
}
