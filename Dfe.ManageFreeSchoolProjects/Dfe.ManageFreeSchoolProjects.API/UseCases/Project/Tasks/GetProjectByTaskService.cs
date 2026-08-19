using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.AdmissionsArrangements;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.ApplicationsEvidence;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.ArticlesOfAssociation;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.CommissionedExternalExpert;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.Constituency;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.Dates;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.DueDiligenceChecks;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.EducationBrief;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.EqualitiesAssessment;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.EvidenceOfAcceptedOffers;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.FinalFinancePlan;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.FinancePlan;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.FundingAgreement;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.FundingAgreementHealthCheck;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.FundingAgreementSubmission;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.Gias;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.GovernancePlan;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.ImpactAssessment;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.KickOffMeeting;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.NewSchool;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.OfstedInspection;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PDG;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PDG.PaymentSchedule;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PDG.Refunds;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PDG.StopPayments;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PDG.TrustLetterPDGLetterSent;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PDG.WriteOff;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PreFundingAgreementCheckpointMeeting;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PrincipalDesignate;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PupilNumbersChecks;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.ReadinessToOpenMeeting;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.ReferenceNumbers;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.RegionAndLocalAuthority;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.RiskAppraisalMeeting;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.School;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.StatutoryConsultation;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.Trusts;
using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks
{
    public interface IGetProjectByTaskService
    {
        public Task<GetProjectByTaskResponse> Execute(string projectId, TaskName taskName);
    }

    public class GetProjectByTaskService(MfspContext context) : IGetProjectByTaskService
    {
        public async Task<GetProjectByTaskResponse> Execute(string projectId, TaskName taskName)
        {
            var query = context.Kpi.Where(kpi => kpi.ProjectStatusProjectId == projectId);

            var parameters = new GetTaskServiceParameters
            {
                ProjectId = projectId,
                BaseQuery = query
            };

            GetProjectByTaskResponse result;

            switch (taskName)
            {
                case TaskName.School:
                    result = await new GetSchoolTaskService().Get(parameters);
                    break;
                case TaskName.ReferenceNumbers:
                    result = await new GetReferenceNumbersTaskService().Get(parameters);
                    break;
                case TaskName.Dates:
                    result = await new GetDatesTaskService().Get(parameters);
                    break;
                case TaskName.RiskAppraisalMeeting:
                    result = await new GetRiskAppraisalMeetingTaskService(context).Get(parameters);
                    break;
                case TaskName.Trust:
                    result = await new GetTrustTaskService().Get(parameters);
                    break;
                case TaskName.RegionAndLocalAuthority:
                    result = await new GetRegionAndLocalAuthorityTaskService().Get(parameters);
                    break;
                case TaskName.Constituency:
                    result = await new GetConstituencyTaskService().Get(parameters);
                    break;
                case TaskName.KickOffMeeting:
                    result = await new GetKickOffMeetingTaskService(context).Get(parameters);
                    break;
                case TaskName.FundingAgreement:
                    result = await new GetFundingAgreementTaskService(context).Get(parameters);
                    break;
                case TaskName.StatutoryConsultation:
                    result = await new GetStatutoryConsultationTaskService(context).Get(parameters);
                    break;
                case TaskName.ArticlesOfAssociation:
                    result = await new GetArticlesOfAssociationTaskService(context).Get(parameters);
                    break;
                case TaskName.FinancePlan:
                    result = await new GetFinancePlanTaskService(context).Get(parameters);
                    break;
                case TaskName.GovernancePlan:
                    result = await new GetGovernancePlanTaskService(context).Get(parameters);
                    break;
                case TaskName.Gias:
                    result = await new GetGiasTaskService(context).Get(parameters);
                    break;
                case TaskName.EducationBrief:
                    result = await new GetEducationBriefTaskService(context).Get(parameters);
                    break;
                case TaskName.AdmissionsArrangements:
                    result = await new GetAdmissionsArrangementsTaskService(context).Get(parameters);
                    break;
                case TaskName.EqualitiesAssessment:
                    result = await new GetEqualitiesAssessmentTaskService(context).Get(parameters);
                    break;
                case TaskName.ImpactAssessment:
                    result = await new GetImpactAssessmentTaskService(context).Get(parameters);
                    break;
                case TaskName.EvidenceOfAcceptedOffers:
                    result = await new GetEvidenceOfAcceptedOffersTaskService(context).Get(parameters);
                    break;
                case TaskName.OfstedInspection:
                    result = await new GetOfstedInspectionTaskService(context).Get(parameters);
                    break;
                case TaskName.ApplicationsEvidence:
                    result = await new GetApplicationsEvidenceTaskService(context).Get(parameters);
                    break;
                case TaskName.FundingAgreementHealthCheck:
                    result = await new GetFundingAgreementHealthCheckTaskService(context).Get(parameters);
                    break;
                case TaskName.FundingAgreementSubmission:
                    result = await new GetFundingAgreementSubmissionTaskService(context).Get(parameters);
                    break;
                case TaskName.PupilNumbersChecks:
                    result = await new GetPupilNumbersChecksTaskService(context).Get(parameters);
                    break;
                case TaskName.PDG:
                    result = await new GetPDGDashboardService(context).Get(parameters);
                    break;
                case TaskName.PaymentSchedule:
                    result = await new GetPaymentScheduleService(context).Get(parameters);
                    break;
                case TaskName.TrustPDGLetterSent:
                    result = await new GetTrustPDGLetterSentService(context).Get(parameters);
                    break;
                case TaskName.StopPayment:
                    result = await new GetStopPaymentService(context).Get(parameters);
                    break;
                case TaskName.Refunds:
                    result = await new GetRefundsService(context).Get(parameters);
                    break;
                case TaskName.WriteOff:
                    result = await new GetWriteOffService(context).Get(parameters);
                    break;
                case TaskName.FinalFinancePlan:
                    result = await new GetFinalFinancePlanTaskService(context).Get(parameters);
                    break;
                case TaskName.CommissionedExternalExpert:
                    result = await new GetCommissionedExternalExpertTaskService(context).Get(parameters);
                    break;
                case TaskName.MovingToOpen:
                    result = await new GetMovingToOpenTaskService(context).Get(parameters);
                    break;
                case TaskName.PrincipalDesignate:
                    result = await new GetPrincipalDesignateTaskService(context).Get(parameters);
                    break;
                case TaskName.DueDiligenceChecks:
                    result = await new GetDueDiligenceChecksTaskService(context).Get(parameters);
                    break;
                case TaskName.PreFundingAgreementCheckpointMeeting:
                    result = await new GetPreFundingAgreementCheckpointMeetingTaskService(context).Get(parameters);
                    break;
                case TaskName.ReadinessToOpenMeeting:
                    result = await new GetROMService(context).Get(parameters);
                    break;
                case TaskName.NewSchoolSpecificationPublicationDate:
                case TaskName.NewSchoolClosingDateForProposals:
                case TaskName.NewSchoolDecisionMaker:
                case TaskName.NewSchoolDateOfDecision:
                case TaskName.NewSchoolConditions:
                case TaskName.NewSchoolDateForConditionsToBeMet:
                    result = await new GetNewSchoolTaskService().Get(parameters);
                    break;
                default:
                    throw new ArgumentException($"Unknown task name {taskName}");
            }

            if (result != null)
            {
                var schoolName =
                    await query.Select(kpi => kpi.ProjectStatusCurrentFreeSchoolName).FirstOrDefaultAsync();
                var applicationWave = await query.Select(kpi => kpi.ProjectStatusFreeSchoolApplicationWave)
                    .FirstOrDefaultAsync();
                var isPresumptionRoute = (applicationWave == "FS - Presumption");

                result.SchoolName = schoolName;
                result.IsPresumptionRoute = isPresumptionRoute;
            }

            return result;
        }
    }
}