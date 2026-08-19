namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks
{
    public record NewSchoolTask
    {
        public DateTime? NewSchoolSpecificationPublicationDate { get; set; }

        public DateTime? NewSchoolClosingDateForProposals { get; set; }

        public string NewSchoolDecisionMaker { get; set; }

        public DateTime? NewSchoolDateOfDecision { get; set; }

        //public string NewSchoolWasTheDecisionApprovedWithoutConditions { get; set; }

        //public string NewSchoolHaveAnyConditionBeenApplied { get; set; }

        public string NewSchoolConditions { get; set; }
        public string NewSchoolConditionsDescription { get; set; }

        public DateTime? NewSchoolDateForConditionsToBeMet { get; set; }
    }
}
