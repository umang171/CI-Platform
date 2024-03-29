﻿using CIPlatform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class HeaderViewModel
    {
        public long userid { get; set; }

        public string username { get; set; } = String.Empty;
        public string avtar { get; set; } = String.Empty;
        public List<CmsPage> cmsPages { get; set; }=new List<CmsPage>();
    }
}
