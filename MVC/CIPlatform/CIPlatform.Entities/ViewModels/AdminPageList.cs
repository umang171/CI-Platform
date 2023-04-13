using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class AdminPageList<T> where T : class
    {
        public AdminPageList(List<T> records,int totalCount)
        {
            Records= records??new List<T>();
            TotalCount= totalCount;
        }
        public List<T> Records { get; set; }
        public int TotalCount { get; set; }
    }
}
