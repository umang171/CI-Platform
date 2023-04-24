using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class MissionHomeModel
    {
        public string username { get; set; }=String.Empty;
        public string avtar { get; set; }=String.Empty;
        public long userid { get; set; }
        public IEnumerable<Country> countryList { get; set; }=new List<Country>();
        public IEnumerable<City> cityList { get; set; }=new List<City>();
        public IEnumerable<MissionTheme> themeList { get; set; }= new List<MissionTheme>();
        public IEnumerable<Skill> skillList{ get; set; }=new List<Skill>();
        public List<CmsPage> cmsPages=new List<CmsPage>();
    }
}
