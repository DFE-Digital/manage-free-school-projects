﻿namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks.PDG
{
    public class PDGDashboard
    {
        public decimal? PaymentScheduleAmount{ get; set; }
        public DateTime? PaymentScheduleDate { get; set; }
        public decimal? PaymentActualAmount { get; set; }
        public DateTime? PaymentActualDate { get; set; }
    }
}
