using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.ApplicationsEvidence;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.FundingAgreementHealthCheck;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.FundingAgreementSubmission;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PreFundingAgreementCheckpointMeeting;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.ReadinessToOpenMeeting;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.DueDiligenceChecks;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Tasks;

public interface IGetTasksService
{
    Task<ProjectByTaskSummaryResponse> Execute(string projectId);
}

public class GetAllTasksStatusService : IGetTasksService
{
    private readonly MfspContext _context;

    private int _hiddenCompletedTasks;
    private int _tasksCount;

    public GetAllTasksStatusService(MfspContext context)
    {
        _context = context;
    }

    public async Task<ProjectByTaskSummaryResponse> Execute(string projectId)
    {
        var dbKpi = await _context.Kpi.FirstOrDefaultAsync(x => x.ProjectStatusProjectId == projectId);

        if (dbKpi == null)
        {
            throw new NotFoundException($"Project with id {projectId} not found");
        }

        var dbTasks = await _context.Tasks.Where(x => x.Rid == dbKpi.Rid).ToListAsync();

        var projectTasks = dbTasks.Select(task => new TaskSummaryResponse
        {
            Name = task.TaskName.ToString(),
            Status = task.Status.Map()
        });

        var result = BuildProjectByTaskSummaryResponse(dbKpi, projectTasks);

        return result;
    }

    private ProjectByTaskSummaryResponse BuildProjectByTaskSummaryResponse(Kpi dbKpi,
        IEnumerable<TaskSummaryResponse> taskSummaryResponses)
    {
        _tasksCount = 0;
        _hiddenCompletedTasks = 0;
        
        var projectTasks = taskSummaryResponses.ToList();
        var result = new ProjectByTaskSummaryResponse
        {
            SchoolName = dbKpi.ProjectStatusCurrentFreeSchoolName,
            School = SafeRetrieveTaskSummary(projectTasks, "School"),
            ReferenceNumbers = SafeRetrieveTaskSummary(projectTasks, "ReferenceNumbers"),
            Dates = SafeRetrieveTaskSummary(projectTasks, "Dates"),
            Trust = SafeRetrieveTaskSummary(projectTasks, "Trust"),
            RegionAndLocalAuthority = SafeRetrieveTaskSummary(projectTasks, "RegionAndLocalAuthority"),
            RiskAppraisalMeeting = SafeRetrieveTaskSummary(projectTasks, "RiskAppraisalMeeting"),
            Constituency = SafeRetrieveTaskSummary(projectTasks, "Constituency"),
            FundingAgreement = SafeRetrieveTaskSummary(projectTasks, "FundingAgreement"),
            StatutoryConsultation = SafeRetrieveTaskSummary(projectTasks, "StatutoryConsultation"),
            ArticlesOfAssociation = SafeRetrieveTaskSummary(projectTasks, "ArticlesOfAssociation"),
            FinancePlan = SafeRetrieveTaskSummary(projectTasks, "FinancePlan"),
            KickOffMeeting = SafeRetrieveTaskSummary(projectTasks, "KickOffMeeting"),
            Gias = SafeRetrieveTaskSummary(projectTasks, "Gias"),
            GovernancePlan = SafeRetrieveTaskSummary(projectTasks, TaskName.GovernancePlan.ToString()),
            EducationBrief = SafeRetrieveTaskSummary(projectTasks, "EducationBrief"),
            EqualitiesAssessment = SafeRetrieveTaskSummary(projectTasks, "EqualitiesAssessment"),
            PupilNumbersChecks = SafeRetrieveTaskSummary(projectTasks, "PupilNumbersChecks"),
            AdmissionsArrangements = SafeRetrieveTaskSummary(projectTasks, "AdmissionsArrangements"),
            ImpactAssessment = SafeRetrieveTaskSummary(projectTasks, "ImpactAssessment"),
            EvidenceOfAcceptedOffers = SafeRetrieveTaskSummary(projectTasks, "EvidenceOfAcceptedOffers"),
            OfstedInspection = SafeRetrieveTaskSummary(projectTasks, "OfstedInspection"),
            PDG = SafeRetrieveTaskSummary(projectTasks, "PDG"),
            FinalFinancePlan = SafeRetrieveTaskSummary(projectTasks, TaskName.FinalFinancePlan.ToString()),
            CommissionedExternalExpert =
                SafeRetrieveTaskSummary(projectTasks, TaskName.CommissionedExternalExpert.ToString()),
            MovingToOpen = SafeRetrieveTaskSummary(projectTasks, TaskName.MovingToOpen.ToString()),
            PrincipalDesignate = SafeRetrieveTaskSummary(projectTasks, TaskName.PrincipalDesignate.ToString()),
            NewSchoolSpecificationPublicationDate = SafeRetrieveTaskSummary(projectTasks, "NewSchoolSpecificationPublicationDate"),
            NewSchoolClosingDateForProposals = SafeRetrieveTaskSummary(projectTasks, "NewSchoolClosingDateForProposals"),
            NewSchoolDecisionMaker = SafeRetrieveTaskSummary(projectTasks, "NewSchoolDecisionMaker"),
            NewSchoolDateOfDecision = SafeRetrieveTaskSummary(projectTasks, "NewSchoolDateOfDecision"),
            //NewSchoolWasTheDecisionApprovedWithoutConditions = SafeRetrieveTaskSummary(projectTasks, "NewSchoolWasTheDecisionApprovedWithoutConditions"),
            //NewSchoolHaveAnyConditionBeenApplied = SafeRetrieveTaskSummary(projectTasks, "NewSchoolHaveAnyConditionBeenApplied"),
            NewSchoolConditions = SafeRetrieveTaskSummary(projectTasks, "NewSchoolConditions"),
            NewSchoolDateForConditionsToBeMet = SafeRetrieveTaskSummary(projectTasks, "NewSchoolDateForConditionsToBeMet")
        };

        var applicationsEvidenceTask = SafeRetrieveTaskSummary(projectTasks, TaskName.ApplicationsEvidence.ToString());
        var fundingAgreementHealthCheckTask = SafeRetrieveTaskSummary(projectTasks, "FundingAgreementHealthCheck");
        var fundingAgreementSubmissionTask = SafeRetrieveTaskSummary(projectTasks, "FundingAgreementSubmission");
        var romTask = SafeRetrieveTaskSummary(projectTasks, TaskName.ReadinessToOpenMeeting.ToString());
        var preFundingAgreementCheckpointMeeting = SafeRetrieveTaskSummary(projectTasks, TaskName.PreFundingAgreementCheckpointMeeting.ToString());
        var dueDiligenceChecks = SafeRetrieveTaskSummary(projectTasks, TaskName.DueDiligenceChecks.ToString());

        result.FundingAgreementHealthCheck =
            BuildFundingAgreementHealthCheckTask(fundingAgreementHealthCheckTask, dbKpi);
        result.FundingAgreementSubmission = BuildFundingAgreementSubmissionTask(fundingAgreementSubmissionTask, dbKpi);
        result.ReadinessToOpenMeeting = BuildROMTask(romTask, dbKpi);
        result.PreFundingAgreementCheckpointMeeting = BuildPFACMTask(preFundingAgreementCheckpointMeeting, dbKpi);
        result.DueDiligenceChecks = BuildDueDiligenceChecksTask(dueDiligenceChecks, dbKpi);
        result.ApplicationsEvidence = BuildApplicationsEvidenceTask(applicationsEvidenceTask, dbKpi);

        result.TaskCount = _tasksCount;

        RemoveHiddenCompletedTaskStatus(result.FundingAgreementHealthCheck);
        RemoveHiddenCompletedTaskStatus(result.FundingAgreementSubmission);
        RemoveHiddenCompletedTaskStatus(result.ReadinessToOpenMeeting);
        RemoveHiddenCompletedTaskStatus(result.PreFundingAgreementCheckpointMeeting);
        RemoveHiddenCompletedTaskStatus(result.DueDiligenceChecks);

        result.CompletedTasks =
            projectTasks.Count(x => x.Status == ProjectTaskStatus.Completed) - _hiddenCompletedTasks;


        return result;
    }

    private TaskSummaryResponse SafeRetrieveTaskSummary(IEnumerable<TaskSummaryResponse> projectTasks,
        string taskName)
    {
        _tasksCount++;
        return projectTasks.FirstOrDefault(x => x.Name == taskName,
            new TaskSummaryResponse { Name = taskName, Status = ProjectTaskStatus.NotStarted });
    }

    private TaskSummaryResponse BuildFundingAgreementHealthCheckTask(TaskSummaryResponse taskSummaryResponse,
        Kpi kpi)
    {
        var parameters = new FundingAgreementHealthCheckTaskSummaryBuilderParameters()
        {
            ApplicationWave = kpi.ProjectStatusFreeSchoolApplicationWave,
            TaskSummary = taskSummaryResponse
        };

        var result = new FundingAgreementHealthCheckTaskSummaryBuilder().Build(parameters);

        _tasksCount -= taskSummaryResponse.IsHidden ? 1 : 0;

        return result;
    }

    private TaskSummaryResponse BuildFundingAgreementSubmissionTask(TaskSummaryResponse taskSummaryResponse,
        Kpi kpi)
    {
        var parameters = new FundingAgreementSubmissionTaskSummaryBuilderParameters()
        {
            ApplicationWave = kpi.ProjectStatusFreeSchoolApplicationWave,
            TaskSummary = taskSummaryResponse
        };

        var result = new FundingAgreementSubmissionTaskSummaryBuilder().Build(parameters);

        _tasksCount -= taskSummaryResponse.IsHidden ? 1 : 0;

        return result;
    }

    private TaskSummaryResponse BuildROMTask(TaskSummaryResponse taskSummaryResponse,
    Kpi kpi)
    {
        var parameters = new ROMTaskSummaryBuilderParameters()
        {
            ApplicationWave = kpi.ProjectStatusFreeSchoolApplicationWave,
            TaskSummary = taskSummaryResponse
        };

        var result = new ROMTaskSummaryBuilder().Build(parameters);

        _tasksCount -= taskSummaryResponse.IsHidden ? 1 : 0;

        return result;
    }

    private TaskSummaryResponse BuildPFACMTask(TaskSummaryResponse taskSummaryResponse,
    Kpi kpi)
    {
        var parameters = new PreFundingAgreementCheckpointMeetingTaskSummaryBuilderParameters()
        {
            ApplicationWave = kpi.ProjectStatusFreeSchoolApplicationWave,
            TaskSummary = taskSummaryResponse
        };

        var result = new PreFundingAgreementCheckpointMeetingTaskSummaryBuilder().Build(parameters);

        _tasksCount -= taskSummaryResponse.IsHidden ? 1 : 0;

        return result;
    }

    private TaskSummaryResponse BuildDueDiligenceChecksTask(TaskSummaryResponse taskSummaryResponse,
    Kpi kpi)
    {
        var parameters = new DueDiligenceChecksTaskSummaryBuilderParameters()
        {
            ApplicationWave = kpi.ProjectStatusFreeSchoolApplicationWave,
            TaskSummary = taskSummaryResponse
        };

        var result = new DueDiligenceChecksTaskSummaryBuilder().Build(parameters);

        _tasksCount -= taskSummaryResponse.IsHidden ? 1 : 0;

        return result;
    }

    private TaskSummaryResponse BuildApplicationsEvidenceTask(TaskSummaryResponse taskSummaryResponse,
Kpi kpi)
    {
        var parameters = new ApplicationsEvidenceTaskSummaryBuilderParameters()
        {
            SchoolType = ProjectMapper.ToSchoolType(kpi.SchoolDetailsSchoolTypeMainstreamApEtc),
            TaskSummary = taskSummaryResponse
        };

        var result = new ApplicationsEvidenceTaskSummaryBuilder().Build(parameters);

        _tasksCount -= taskSummaryResponse.IsHidden ? 1 : 0;

        return result;
    }


    private void RemoveHiddenCompletedTaskStatus(TaskSummaryResponse taskSummaryResponse)
    {
        if (taskSummaryResponse.IsHidden && taskSummaryResponse.Status == ProjectTaskStatus.Completed)
        {
            _hiddenCompletedTasks++;
        }
    }
}