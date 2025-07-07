using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Summary;
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
    }

    public class GetProjectSummaryByUserService(MfspContext context) : IGetProjectSummaryByUserService
    {
        public async Task<(List<GetProjectSummaryResponse>, int)> Execute(GetProjectSummaryByUserParameters parameters)
        {
            var query = context.Kpi.AsQueryable();

            query = ApplyFilters(query, parameters);

            var count = query.Count();

            var projectRecords = await
                query
                    .Select(
                        e => new
                        {
                            Kpi = e,
                            PeriodStart = EF.Property<DateTime>(e, "PeriodStart"),
                            PeriodEnd = EF.Property<DateTime>(e, "PeriodEnd")
                        })
                    .OrderByDescending(record => record.Kpi.ProjectStatusProvisionalOpeningDateAgreedWithTrust)
                    .ThenBy(record => record.Kpi.ProjectStatusCurrentFreeSchoolName)
                    .ToListAsync();

            var result = projectRecords.Select(record => new GetProjectSummaryResponse()
            {
                ProjectId = record.Kpi.ProjectStatusProjectId,
                ProjectTitle = record.Kpi.ProjectStatusCurrentFreeSchoolName,
                TrustName = record.Kpi.TrustName,
                LocalAuthority = record.Kpi.LocalAuthority,
                RealisticOpeningYear = record.Kpi.ProjectStatusRealisticYearOfOpening,
                Region = record.Kpi.SchoolDetailsGeographicalRegion,
                ProjectManagedBy = record.Kpi.KeyContactsFsgLeadContact,
                ProjectType = record.Kpi.ProjectStatusFreeSchoolApplicationWave == "FS - Presumption"
                    ? "Presumption"
                    : "Central Route",
                ProjectManagedByEmail = record.Kpi.KeyContactsFsgLeadContactEmail,
                ProjectStatus = ProjectMapper.ToProjectStatusType(record.Kpi.ProjectStatusProjectStatus).ToDescription(),
                SchoolType = ProjectMapper.ToSchoolType(record.Kpi.SchoolDetailsSchoolTypeMainstreamApEtc).ToDescription(),
                UpdatedAt = record.PeriodStart
            }).ToList();

            return (result, count);
        }

        private static IQueryable<Kpi> ApplyFilters(IQueryable<Kpi> query, GetProjectSummaryByUserParameters parameters)
        {
            query = query.Where(kpi => parameters.ProjectManagedByEmail == kpi.KeyContactsFsgLeadContactEmail);

            return query;
        }
    }
}
