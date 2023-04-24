using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class CmsPageModel
    {
        public string username { get; set; } = String.Empty;
        public string avtar { get; set; } = String.Empty;
        public long userid { get; set; }
        public List<CmsPage> cmsPages { get; set; } =new List<CmsPage>();
        public CmsPage cmsPage { get; set; }=new CmsPage();
    }
}
