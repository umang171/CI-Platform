﻿using CIPlatform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repository.Repository.Interface
{
    public interface IStoryRepository
    {
        public PaginationStory getStories(int pageNumber);
        public void saveStories(StorySaveModel storySaveModelObj);
        public void submitStories(StorySaveModel storySaveModelObj);
    }
}
