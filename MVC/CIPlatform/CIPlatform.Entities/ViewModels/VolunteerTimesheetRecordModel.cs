﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Entities.ViewModels
{
    public class VolunteerTimesheetRecordModel
    {
        public long TimesheetId { get; set; }

        public long UserId { get; set; }

        public long MissionId { get; set; }

        public string? Time { get; set; }=String.Empty;

        public int? Action { get; set; }

        public DateTime DateVolunteered { get; set; }=new DateTime();

        public string? Notes { get; set; }=String.Empty;

    }
}
