using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Payments;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks.PDG;
using System.ComponentModel;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks
{
    public class GetProjectByTaskResponse
    {
        public string SchoolName { get; set; }
        public bool IsPresumptionRoute { get; set; }
        public DatesTask Dates { get; set; }
        public SchoolTask School { get; set; }
        public ReferenceNumbersTask ReferenceNumbers { get; set; }
        public TrustTask Trust { get; set; }
        public ConstituencyTask Constituency { get; set; }
        public RegionAndLocalAuthorityTask RegionAndLocalAuthority { get; set; }
        public RiskAppraisalMeetingTask RiskAppraisalMeeting { get; set; }
        public KickOffMeetingTask KickOffMeeting { get; set; }
        public FundingAgreementTask FundingAgreement{ get; set; }
        public StatutoryConsultationTask StatutoryConsultation { get; set; }
        public ArticlesOfAssociationTask ArticlesOfAssociation { get; set; }
        public GiasTask Gias { get; set; }
        public FinancePlanTask FinancePlan { get; set; }
        public DraftGovernancePlanTask DraftGovernancePlan { get; set; }
        public EducationBriefTask EducationBrief { get; set; }
        public AdmissionsArrangementsTask AdmissionsArrangements { get; set; }
        public EqualitiesAssessmentTask EqualitiesAssessment { get; set; }
        public ImpactAssessmentTask ImpactAssessment { get; set; }
        public EvidenceOfAcceptedOffersTask EvidenceOfAcceptedOffers { get; set; }
        public OfstedInspectionTask OfstedInspection { get; set; }
        public ApplicationsEvidenceTask ApplicationsEvidence { get; set; }
        public FundingAgreementHealthCheckTask FundingAgreementHealthCheck { get; set; }
        public FundingAgreementSubmissionTask FundingAgreementSubmission { get; set; }
        public PDGDashboard PDGDashboard { get; set; }
        public PaymentScheduleTask PaymentSchedule { get; set; }
        public TrustPDGLetterSentTask TrustPDGLetterSent { get; set; }
        public StopPaymentTask StopPayment { get; set; }
        public RefundsTask Refunds { get; set; }
        public WriteOffTask WriteOff { get; set; }
        public FinalFinancePlanTask FinalFinancePlan { get; set; }
        
        public PupilNumbersChecksTask PupilNumbersChecks { get; set; }
        
        public CommissionedExternalExpertTask CommissionedExternalExpert { get; set; }
        
        public MovingToOpenTask MovingToOpen  { get; set; }

        public PrincipalDesignateTask PrincipalDesignate { get; set; }
    }
}
