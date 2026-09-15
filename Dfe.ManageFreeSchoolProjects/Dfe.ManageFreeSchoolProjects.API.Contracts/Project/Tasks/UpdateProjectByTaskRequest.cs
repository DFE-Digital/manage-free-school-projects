using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks.PDG;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks
{
    public class UpdateProjectByTaskRequest
    {
        public NewSchoolSpecificationPublicationDateTask NewSchoolSpecificationPublicationDate { get; set; }
        public NewSchoolClosingDateForProposalsTask NewSchoolClosingDateForProposals { get; set; }
        public NewSchoolDecisionMakerTask NewSchoolDecisionMaker { get; set; }
        public NewSchoolDateOfDecisionTask NewSchoolDateOfDecision { get; set; }
        public NewSchoolConditionsTask NewSchoolConditions { get; set; }
        public NewSchoolDateForConditionsToBeMetTask NewSchoolDateForConditionsToBeMet { get; set; }
        public NewSchoolDecisionTask NewSchoolDecision { get; set; }
        public DatesTask Dates { get; set; }
        public SchoolTask School { get; set; }
        public ReferenceNumbersTask ReferenceNumbers { get; set; }
        public TrustTask Trust { get; set; }
        public RegionAndLocalAuthorityTask RegionAndLocalAuthorityTask { get; set; }
        public RiskAppraisalMeetingTask RiskAppraisalMeeting { get; set; }
        public ConstituencyTask Constituency { get; set; }

        public KickOffMeetingTask KickOffMeeting { get; set; }

        public GiasTask Gias { get; set; }
        public FundingAgreementTask FundingAgreement { get; set; }

        public StatutoryConsultationTask StatutoryConsultation { get; set; }

        public ArticlesOfAssociationTask ArticlesOfAssociation { get; set; }

        public FinancePlanTask FinancePlan { get; set; }

        public GovernancePlanTask GovernancePlan { get;set;}
        
        public EducationBriefTask EducationBrief { get; set; }

        public AdmissionsArrangementsTask AdmissionsArrangements { get; set; }

        public ImpactAssessmentTask ImpactAssessment { get; set; }

        public EvidenceOfAcceptedOffersTask EvidenceOfAcceptedOffers { get; set; }

        public EqualitiesAssessmentTask EqualitiesAssessment { get; set; }

        public OfstedInspectionTask OfstedInspection { get; set; }

        public ApplicationsEvidenceTask ApplicationsEvidence { get; set; }

        public FundingAgreementHealthCheckTask FundingAgreementHealthCheck { get; set; }
        public FundingAgreementSubmissionTask FundingAgreementSubmission { get; set; }
        public PaymentScheduleTask PaymentSchedule { get; set; }

        public TrustPDGLetterSentTask TrustPDGLetterSent { get; set; }

        public StopPaymentTask StopPayment { get; set; }

        public RefundsTask Refunds { get; set; }

        public WriteOffTask WriteOff { get; set; }
        public FinalFinancePlanTask FinalFinancePlan { get; set; }

        public PupilNumbersChecksTask PupilNumbersChecks { get; set; }

        public CommissionedExternalExpertTask CommissionedExternalExpert { get; set; }

        public MovingToOpenTask MovingToOpen { get; set; }

        public PrincipalDesignateTask PrincipalDesignate { get; set; }

        public DueDiligenceChecks DueDiligenceChecks { get; set; }

        public PDGGrantTask PDGGrantTask { get; set; }

        public PreFundingAgreementCheckpointMeetingTask PreFundingAgreementCheckpointMeetingTask { get; set; }
        
        public ReadinessToOpenMeetingTask ReadinessToOpenMeetingTask { get; set; }
        
        public string TaskToUpdate
        {
            get
            {
                if (NewSchoolSpecificationPublicationDate != null)
                    return "NewSchoolSpecificationPublicationDate";
                if (NewSchoolClosingDateForProposals != null)
                    return "NewSchoolClosingDateForProposals";
                if (NewSchoolDecisionMaker != null)
                    return "NewSchoolDecisionMaker";
                if (NewSchoolDateOfDecision != null)
                    return "NewSchoolDateOfDecision";
                if (NewSchoolDecision != null)
                    return "NewSchoolDecision";
                if (NewSchoolConditions != null)
                    return "NewSchoolConditions";
                if (NewSchoolDateForConditionsToBeMet != null)
                    return "NewSchoolDateForConditionsToBeMet";
                if (School != null)
                    return "School";
                if (ReferenceNumbers != null)
                    return TaskName.ReferenceNumbers.ToString();
                if (Dates != null)
                    return "Dates";
                if (Trust != null)
                    return "Trust";
                if (RegionAndLocalAuthorityTask != null)
                    return "RegionAndLocalAuthority";
                if (RiskAppraisalMeeting != null)
                    return "RiskAppraisalMeeting";
                if (Constituency != null)
                    return "Constituency";
                if (ArticlesOfAssociation != null)
                    return TaskName.ArticlesOfAssociation.ToString();
                if (FinancePlan != null)
                    return TaskName.FinancePlan.ToString();
                if (KickOffMeeting != null)
                    return TaskName.KickOffMeeting.ToString();
                if (FundingAgreement != null)
                    return TaskName.FundingAgreement.ToString();
                if (GovernancePlan != null)
                    return TaskName.GovernancePlan.ToString();
                if (Gias != null)
                    return TaskName.Gias.ToString();
                if (StatutoryConsultation != null)
                    return TaskName.StatutoryConsultation.ToString();
                if (EducationBrief != null)
                    return TaskName.EducationBrief.ToString();
                if (AdmissionsArrangements != null)
                    return TaskName.AdmissionsArrangements.ToString();
                if (EqualitiesAssessment != null)
                    return TaskName.EqualitiesAssessment.ToString();
                if (ImpactAssessment != null)
                    return TaskName.ImpactAssessment.ToString();
                if (EvidenceOfAcceptedOffers != null)
                    return TaskName.EvidenceOfAcceptedOffers.ToString();
                if (OfstedInspection != null)
                    return TaskName.OfstedInspection.ToString();
                if (ApplicationsEvidence != null)
                    return TaskName.ApplicationsEvidence.ToString();
                if (FundingAgreementHealthCheck != null)
                    return TaskName.FundingAgreementHealthCheck.ToString();
                if (FundingAgreementSubmission != null)
                    return TaskName.FundingAgreementSubmission.ToString();
                if (PaymentSchedule != null)
                    return TaskName.PaymentSchedule.ToString();
                if (TrustPDGLetterSent != null)
                    return TaskName.TrustPDGLetterSent.ToString();
                if (StopPayment != null)
                    return TaskName.StopPayment.ToString();
                if (Refunds != null)
                    return TaskName.Refunds.ToString();
                if (WriteOff != null)
                    return TaskName.WriteOff.ToString();
                if (FinalFinancePlan != null)
                    return TaskName.FinalFinancePlan.ToString();
                if (PupilNumbersChecks != null)
                    return TaskName.PupilNumbersChecks.ToString();
                if (CommissionedExternalExpert != null)
                    return TaskName.CommissionedExternalExpert.ToString();
                if (MovingToOpen != null)
                    return TaskName.MovingToOpen.ToString();
                if (PrincipalDesignate != null)
                    return TaskName.PrincipalDesignate.ToString();
                if (DueDiligenceChecks != null)
                    return TaskName.DueDiligenceChecks.ToString();
                if (PDGGrantTask != null)
                    return TaskName.PDG.ToString();
                if (PreFundingAgreementCheckpointMeetingTask != null)
                    return TaskName.PreFundingAgreementCheckpointMeeting.ToString();
                if (ReadinessToOpenMeetingTask != null)
                    return TaskName.ReadinessToOpenMeeting.ToString();
                return null;
            }
        }
    }
}