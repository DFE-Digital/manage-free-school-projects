using System.Diagnostics;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Summary;
using Dfe.ManageFreeSchoolProjects.API.Diagnostics;
using Dfe.ManageFreeSchoolProjects.API.Extensions;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Summary
{
    public interface IGetProjectSummaryByUserService
    {
        Task<(List<GetProjectSummaryResponse>, int)> Execute(GetProjectSummaryByUserParameters parameters);
    }

    public record GetProjectSummaryByUserParameters
    {
        public string ProjectManagedByEmail { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
    }

    public class GetProjectSummaryByUserService(
        MfspContext context,
        IProcessWarmupState processWarmupState,
        IHttpContextAccessor httpContextAccessor,
        ILogger<GetProjectSummaryByUserService> logger) : IGetProjectSummaryByUserService
    {
        private const string StageTimingsEventName = "ProjectSummaryByUser.StageTimings";

        public async Task<(List<GetProjectSummaryResponse>, int)> Execute(GetProjectSummaryByUserParameters parameters)
        {
            var isFirstBusinessRequestInProcess = ResolveIsFirstBusinessRequest();
            var totalStopwatch = Stopwatch.StartNew();

            if (string.IsNullOrWhiteSpace(parameters.ProjectManagedByEmail))
            {
                return ([], 0);
            }

            var query = ApplyFilters(context.Kpi.AsQueryable(), parameters);

            var countStopwatch = Stopwatch.StartNew();
            var count = await query.CountAsync();
            countStopwatch.Stop();

            // Wall-clock through the first EF round-trip (includes model-build/JIT when cold).
            var timeToFirstEfQueryMs = totalStopwatch.Elapsed.TotalMilliseconds;

            var toListStopwatch = Stopwatch.StartNew();
            var result = await query
                .OrderByDescending(kpi => kpi.ProjectStatusProvisionalOpeningDateAgreedWithTrust)
                .ThenBy(kpi => kpi.ProjectStatusCurrentFreeSchoolName)
                .Paginate(parameters.Page, parameters.Count)
                .Select(kpi => new GetProjectSummaryResponse
                {
                    ProjectId = kpi.ProjectStatusProjectId,
                    ProjectTitle = kpi.ProjectStatusCurrentFreeSchoolName,
                    TrustName = kpi.TrustName,
                    LocalAuthority = kpi.LocalAuthority,
                    RealisticOpeningYear = kpi.ProjectStatusRealisticYearOfOpening,
                    Region = kpi.SchoolDetailsGeographicalRegion,
                    ProjectManagedBy = kpi.KeyContactsFsgLeadContact,
                    ProjectType = kpi.ProjectStatusFreeSchoolApplicationWave == "FS - Presumption"
                        ? "Presumption"
                        : "Central Route",
                    ProjectManagedByEmail = kpi.KeyContactsFsgLeadContactEmail,
                    ProjectStatus = kpi.ProjectStatusProjectStatus,
                    SchoolType = kpi.SchoolDetailsSchoolTypeMainstreamApEtc,
                    UpdatedAt = EF.Property<DateTime>(kpi, "PeriodStart")
                })
                .ToListAsync();
            toListStopwatch.Stop();

            var mappingStopwatch = Stopwatch.StartNew();
            foreach (var summary in result)
            {
                summary.ProjectStatus = ProjectMapper.ToProjectStatusType(summary.ProjectStatus).ToDescription();
                summary.SchoolType = ProjectMapper.ToSchoolType(summary.SchoolType).ToDescription();
            }
            mappingStopwatch.Stop();

            totalStopwatch.Stop();

            EmitStageTimings(
                isFirstBusinessRequestInProcess,
                timeToFirstEfQueryMs,
                countStopwatch.Elapsed.TotalMilliseconds,
                toListStopwatch.Elapsed.TotalMilliseconds,
                mappingStopwatch.Elapsed.TotalMilliseconds,
                result.Count,
                totalStopwatch.Elapsed.TotalMilliseconds);

            return (result, count);
        }

        private bool ResolveIsFirstBusinessRequest()
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext?.Items.TryGetValue(ProcessWarmupState.HttpContextItemKey, out var value) == true
                && value is bool markedByMiddleware)
            {
                return markedByMiddleware;
            }

            return processWarmupState.MarkBusinessRequest();
        }

        private void EmitStageTimings(
            bool isFirstBusinessRequestInProcess,
            double timeToFirstEfQueryMs,
            double countAsyncDurationMs,
            double toListAsyncDurationMs,
            double mappingDurationMs,
            int rowCount,
            double totalDurationMs)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            using (logger.BeginScope(new Dictionary<string, object>
            {
                ["EventName"] = StageTimingsEventName,
                ["IsFirstBusinessRequestInProcess"] = isFirstBusinessRequestInProcess,
                ["TimeToFirstEfQueryMs"] = timeToFirstEfQueryMs,
                ["CountAsyncDurationMs"] = countAsyncDurationMs,
                ["ToListAsyncDurationMs"] = toListAsyncDurationMs,
                ["MappingDurationMs"] = mappingDurationMs,
                ["RowCount"] = rowCount,
                ["TotalDurationMs"] = totalDurationMs
            }))
            {
                logger.LogInformation(
                    "{EventName}: TimeToFirstEfQuery={TimeToFirstEfQueryMs}ms, CountAsync={CountAsyncDurationMs}ms, ToListAsync={ToListAsyncDurationMs}ms, Mapping={MappingDurationMs}ms, RowCount={RowCount}, IsFirstBusinessRequestInProcess={IsFirstBusinessRequestInProcess}, Total={TotalDurationMs}ms",
                    StageTimingsEventName,
                    timeToFirstEfQueryMs,
                    countAsyncDurationMs,
                    toListAsyncDurationMs,
                    mappingDurationMs,
                    rowCount,
                    isFirstBusinessRequestInProcess,
                    totalDurationMs);
            }
        }

        private static IQueryable<Kpi> ApplyFilters(IQueryable<Kpi> query, GetProjectSummaryByUserParameters parameters)
        {
            query = query.Where(kpi => parameters.ProjectManagedByEmail == kpi.KeyContactsFsgLeadContactEmail);

            return query;
        }
    }
}
