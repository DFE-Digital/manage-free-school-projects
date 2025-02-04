using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;

using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Sites;

public interface IGetProjectSitesCentralService
{
    public Task<GetProjectSitesCentralResponse> Execute(string projectId);
    public Task<GetProjectSitesCentralResponse> Execute(Kpi project);
}

public class GetProjectSitesCentralService : IGetProjectSitesCentralService
{
    private readonly MfspContext _context;

    public GetProjectSitesCentralService(MfspContext context)
    {
        _context = context;
    }

    public async Task<GetProjectSitesCentralResponse> Execute(string projectId)
    {
        var dbProject = await _context.Kpi.FirstOrDefaultAsync(x => x.ProjectStatusProjectId == projectId);

        if (dbProject == null)
        {
            throw new NotFoundException($"Project with id {projectId} not found");
        }

        return await Execute(dbProject);
    }

    public async Task<GetProjectSitesCentralResponse> Execute(Kpi project)
    {
        var query = _context.Kpi.Where(kpi => kpi.ProjectStatusProjectId == project.ProjectStatusProjectId);
        var result = await (from kpi in query
                            join constructData in _context.ConstructData on kpi.ProjectStatusProjectId equals constructData.ProjectId into joinedConstructData
                            from constructData in joinedConstructData.DefaultIfEmpty()
                            select new GetProjectSitesCentralResponse()
                            {
                                SchoolName = kpi.ProjectStatusCurrentFreeSchoolName,
                                ProjectId = kpi.ProjectStatusProjectId,

                                HoTAgreedForSiteForMainSchoolBuildingActual = constructData.HoTAgreedForSiteForMainSchoolBuildingActual,
                                TemporaryAccommodationFirstReadyForOccupationForecast = constructData.TemporaryAccommodationFirstReadyForOccupationForecast,
                                TemporaryAccommodationFirstReadyForOccupationActual = constructData.TemporaryAccommodationFirstReadyForOccupationActual,
                                MainSchoolBuildingFirstReadyForOccupationForecast = constructData.MainSchoolBuildingFirstReadyForOccupationForecast,
                                MainSchoolBuildingFirstReadyForOccupationActual = constructData.MainSchoolBuildingFirstReadyForOccupationActual,
                                SiteIdentifiedForMainSchoolBuildingActual = constructData.SiteIdentifiedForMainSchoolBuildingActual,
                                CapitalProjectRag = constructData.CapitalProjectRag,
                                PlanningSiteId = constructData.PlanningSiteId,
                                IsThisTheMainPlanningRecord = constructData.IsThisTheMainPlanningRecord,
                                PlanningRisk = constructData.PlanningRisk,
                                PlanningDecision = constructData.PlanningDecision,
                                SiteId = constructData.SiteId,
                                TypeOfSite = constructData.TypeOfSite,
                                SiteStatus = constructData.SiteStatus,
                                PostcodeOfSite = constructData.PostcodeOfSite,
                                PracticalCompletionCertificateIssuedDateA = constructData.PracticalCompletionCertificateIssuedDateA,
                                RegionalHead = constructData.RegionalHead,
                                ProjectDirector = constructData.ProjectDirector,
                                ProjectManager = constructData.ProjectManager,
                                TemporaryRagRating = constructData.TemporaryRagRating,
                                CapitalProjectRagRatingCommentary = constructData.CapitalProjectRagRatingCommentary,
                                TemporaryRagRatingCommentary = constructData.TemporaryRagRatingCommentary,
                                DateOfHoTSecuredOnTemporaryAccommodationSiteIfRequired = constructData.DateOfHoTSecuredOnTemporaryAccommodationSiteIfRequired,
                                LastRefreshDate = constructData.LastRefreshDate,
                                WillTheProjectOpenInTemporaryAccommodation = constructData.WillTheProjectOpenInTemporaryAccommodation,
                                HoTsAgreedForTemporarySiteForecast = constructData.HoTsAgreedForTemporarySiteForecast,
                                ContractorForTemporarySiteAppointedForecast = constructData.ContractorForTemporarySiteAppointedForecast,
                                ContractorForTemporarySiteAppointedActual = constructData.ContractorForTemporarySiteAppointedActual,
                                DateOfPlanningDecisionForTemporarySiteMainPlanningRecordForecast = constructData.DateOfPlanningDecisionForTemporarySiteMainPlanningRecordForecast,
                                DateOfPlanningDecisionForTemporarySiteMainPlanningRecordActual = constructData.DateOfPlanningDecisionForTemporarySiteMainPlanningRecordActual,
                                TemporarySitePlanningDecision = constructData.TemporarySitePlanningDecision,
                                HoTsAgreedForSiteForMainSchoolBuildingForecast = constructData.HoTsAgreedForSiteForMainSchoolBuildingForecast,
                                ContractorForSiteForMainSchoolBuildingAppointedForecast = constructData.ContractorForSiteForMainSchoolBuildingAppointedForecast,
                                ContractorForSiteForMainSchoolBuildingAppointedActual = constructData.ContractorForSiteForMainSchoolBuildingAppointedActual,
                                DateOfPlanningDecisionForMainSiteMainPlanningRecordForecast = constructData.DateOfPlanningDecisionForMainSiteMainPlanningRecordForecast,
                                DateOfPlanningDecisionForMainSiteMainPlanningRecordActual = constructData.DateOfPlanningDecisionForMainSiteMainPlanningRecordActual,
                                TemporarySiteAddress = constructData.TemporarySiteAddress,
                                TemporarySitePostcode = constructData.TemporarySitePostcode,
                                TemporarySitePlanningRisk = constructData.TemporarySitePlanningRisk,
                                DateTemporarySitePlanningApprovalGranted = constructData.DateTemporarySitePlanningApprovalGranted,
                                MainSiteAddress = constructData.MainSiteAddress,
                                DateMainSitePlanningApprovalGranted = constructData.DateMainSitePlanningApprovalGranted,
                            }).FirstOrDefaultAsync();

        return result ?? new GetProjectSitesCentralResponse();
    }
}
