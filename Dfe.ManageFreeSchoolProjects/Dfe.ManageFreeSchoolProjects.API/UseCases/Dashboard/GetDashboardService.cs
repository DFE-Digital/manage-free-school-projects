using Dfe.ManageFreeSchoolProjects.API.Contracts.Dashboard;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Extensions;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Dashboard
{
    public interface IGetDashboardService
    {
        Task<(List<GetDashboardResponse>, int)> Execute(GetDashboardParameters parameters);
        Task<IEnumerable<string>> ExecuteProjectIds(GetDashboardParameters parameters);
    }

    public record GetDashboardParameters
    {
        public string Project { get; set; }
        public string UserId { get; set; }
        public List<string> Regions { get; set; }
        public List<string> LocalAuthority { get; set; }
        public List<string> ProjectManagedBy { get; set; }
        public List<string> ProjectStatus { get; set; }
        public string Wave { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
    }

    public class GetDashboardService(MfspContext context) : IGetDashboardService
    {
        public async Task<(List<GetDashboardResponse>, int)> Execute(GetDashboardParameters parameters)
        {
            var query = context.Kpi.AsQueryable();

            query = ApplyFilters(query, parameters);

            var countTask = query.CountAsync();
            var projectRecordsTask = query
                    .OrderByDescending(kpi => kpi.ProjectStatusProvisionalOpeningDateAgreedWithTrust)
                    .ThenBy(kpi => kpi.ProjectStatusCurrentFreeSchoolName)
                    .Paginate(parameters.Page, parameters.Count)
                    .ToListAsync();

            await Task.WhenAll(countTask, projectRecordsTask);
            
            var count = countTask.Result;
            var projectRecords = projectRecordsTask.Result;

            var result = projectRecords.Select(record => new GetDashboardResponse
            {
                ProjectId = record.ProjectStatusProjectId,
                ProjectTitle = record.ProjectStatusCurrentFreeSchoolName,
                TrustName = record.TrustName,
                LocalAuthority = record.LocalAuthority,
                RealisticOpeningDate = record.ProjectStatusProvisionalOpeningDateAgreedWithTrust,
                Region = record.SchoolDetailsGeographicalRegion,
                ProjectManagedBy = record.KeyContactsFsgLeadContact, 
                ProjectType = record.ProjectStatusFreeSchoolApplicationWave == "FS - Presumption" ? "Presumption" : "Central Route",
                ProjectStatus = ProjectMapper.ToProjectStatusType(record.ProjectStatusProjectStatus)
            }).ToList();
            
            return (result, count);
        }

        private static IQueryable<Kpi> ApplyFilters(IQueryable<Kpi> query, GetDashboardParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.UserId))
                query = query.Where(kpi => kpi.User.Email == parameters.UserId);

            if (parameters.Regions.Count != 0)
                query = query.Where(kpi => parameters.Regions.Any(region => kpi.SchoolDetailsGeographicalRegion == region));

            if (!string.IsNullOrEmpty(parameters.Project))
            {
                query = query.Where(kpi => 
                kpi.ProjectStatusCurrentFreeSchoolName.Contains(parameters.Project)
                || kpi.ProjectStatusProjectId == parameters.Project);
            }

            if (parameters.LocalAuthority.Count != 0)
                query = query.Where(kpi => parameters.LocalAuthority.Any(localAuthority => kpi.LocalAuthority == localAuthority));

            if (parameters.ProjectManagedBy.Count != 0)
                query = query.Where(kpi => parameters.ProjectManagedBy.Any(projectManagedBy => kpi.KeyContactsFsgLeadContact == projectManagedBy));

            var projectStatuses = parameters.ProjectStatus;
            if (projectStatuses.Count != 0)
            {
                if (projectStatuses.Exists(x => x == ProjectStatus.WithdrawnInPreOpening.ToDescription()))
                    projectStatuses.Add(ProjectStatus.WithdrawnDuringPreOpening.ToDescription());

                if (projectStatuses.Exists(x => x == ProjectStatus.Preopening.ToDescription()))
                    projectStatuses.Add(null);

                if (projectStatuses.Exists(x => x == ProjectStatus.Cancelled.ToDescription()))
                    projectStatuses.Add(ProjectStatus.CancelledDuringPreOpening.ToDescription());
                
                query = query.Where(kpi => projectStatuses.Any(projectStatus => kpi.ProjectStatusProjectStatus == projectStatus));
            }

            if (!string.IsNullOrEmpty(parameters.Wave))
                query = query.Where(kpi => kpi.ProjectStatusFreeSchoolApplicationWave == parameters.Wave);

            return query;
        }
        
        public async Task<IEnumerable<string>> ExecuteProjectIds(GetDashboardParameters parameters)
        {
            var query = context.Kpi.AsQueryable();

            query = ApplyFilters(query, parameters);

            var totalListOfIds = await query
                .Select(x => x.ProjectStatusProjectId)
                .Distinct()
                .ToListAsync();

            return totalListOfIds;
        }
    }
}
