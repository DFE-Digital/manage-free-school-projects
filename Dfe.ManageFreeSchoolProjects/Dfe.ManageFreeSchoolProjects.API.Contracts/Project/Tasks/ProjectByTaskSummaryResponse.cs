using System.ComponentModel;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks
{
    public class ProjectByTaskSummaryResponse
    {
        public string SchoolName { get; set; }
        public TaskSummaryResponse School { get; set; }

        public TaskSummaryResponse ReferenceNumbers { get; set; }

        public TaskSummaryResponse Dates { get; set; }

        public TaskSummaryResponse Trust { get; set; }
        
        public TaskSummaryResponse Constituency { get; set; }
        
        public TaskSummaryResponse RegionAndLocalAuthority { get; set; }

        public TaskSummaryResponse RiskAppraisalMeeting { get; set; }
        
        public TaskSummaryResponse KickOffMeeting { get; set; }
        
        public TaskSummaryResponse FundingAgreement { get; set; }

        public TaskSummaryResponse StatutoryConsultation { get; set; }

        public TaskSummaryResponse ArticlesOfAssociation { get; set; }

        public TaskSummaryResponse FinancePlan { get; set; }

        public TaskSummaryResponse GovernancePlan { get; set; }
        
        public TaskSummaryResponse Gias { get; set; }

        public TaskSummaryResponse EducationBrief { get; set; }

        public TaskSummaryResponse AdmissionsArrangements { get; set; }

        public TaskSummaryResponse EqualitiesAssessment { get; set; }

        public TaskSummaryResponse ImpactAssessment { get; set; }

        public TaskSummaryResponse EvidenceOfAcceptedOffers { get; set; }

        public TaskSummaryResponse OfstedInspection { get; set; }

        public TaskSummaryResponse ApplicationsEvidence { get; set; }
        public TaskSummaryResponse FundingAgreementHealthCheck { get; set; }
        public TaskSummaryResponse FundingAgreementSubmission{ get; set; }
        public TaskSummaryResponse PDG { get; set; }
        public TaskSummaryResponse FinalFinancePlan { get; set; }
        
        public TaskSummaryResponse PupilNumbersChecks { get; set; }
        
        public TaskSummaryResponse CommissionedExternalExpert { get; set; }
        
        public TaskSummaryResponse MovingToOpen { get; set; }
        
        public TaskSummaryResponse PrincipalDesignate { get; set; }
        
        public TaskSummaryResponse DueDiligenceChecks { get; set; }

        public TaskSummaryResponse PreFundingAgreementCheckpointMeeting { get; set; }
        
        public TaskSummaryResponse ReadinessToOpenMeeting { get; set; }

        public TaskSummaryResponse NewSchoolSpecificationPublicationDate { get; set; }
        public TaskSummaryResponse NewSchoolClosingDateForProposals { get; set; }
        public TaskSummaryResponse NewSchoolDecisionMaker { get; set; }
        public TaskSummaryResponse NewSchoolDateOfDecision { get; set; }
        public TaskSummaryResponse NewSchoolDecision { get; set; }
        public TaskSummaryResponse NewSchoolConditions { get; set; }
        public TaskSummaryResponse NewSchoolDateForConditionsToBeMet { get; set; }

        public int TaskCount { get; set; }
        
        public int CompletedTasks { get; set; }
    }

    public class TaskSummaryResponse
    {
        public string Name { get; set; }
        public ProjectTaskStatus Status { get; set; }

        public bool IsHidden { get; set; }
    }
}
