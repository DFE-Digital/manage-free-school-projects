﻿using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.Data
{
    public partial class MfspContext : DbContext
    {
        public virtual DbSet<Entities.Existing.Tasks> Tasks { get; set; }
    }
}

namespace Dfe.ManageFreeSchoolProjects.Data.Entities.Existing
{
    public partial class Tasks
    {
        public string Rid { get; set; }
        
        public TaskName TaskName { get; set; }

        public Status Status { get; set; }
    }

    public enum TaskName
    {
        Unknown,
        School,
        Dates,
        RiskAppraisalMeeting,
        Trust,
        RegionAndLocalAuthority,
        Constituency,
    }

    public enum Status
    {
        NotStarted,
        InProgress,
        Completed
    }
}