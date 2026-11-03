using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Sites.NewSchool
{
    public interface IGetProjectSitesNewSchoolService
    {
        Task<GetProjectSitesResponse> Execute(string projectId);

        Task<GetProjectSitesResponse> Execute(Kpi project);
    }

    public class GetProjectSitesNewSchoolService : IGetProjectSitesNewSchoolService
    {
        private readonly MfspContext _context;

        public GetProjectSitesNewSchoolService(MfspContext context)
        {
            _context = context;
        }

        public async Task<GetProjectSitesResponse> Execute(string projectId)
        {
            var dbProject = await _context.Kpi.FirstOrDefaultAsync(x => x.ProjectStatusProjectId == projectId);

            if (dbProject == null)
            {
                throw new NotFoundException($"Project with id {projectId} not found");
            }

            if (dbProject.ProjectStatusFreeSchoolApplicationWave != "New School")
            {
                throw new NotFoundException($"Project with id {projectId} is not of application wave New School");
            }

            return await Execute(dbProject);
        }

        public async Task<GetProjectSitesResponse> Execute(Kpi project)
        {
            var sites = await _context.Property
                .Where(p => p.PRid == project.Rid).ToListAsync();

            var permanentSite = sites.FirstOrDefault(p => p.IsPermanentSite());
            var temporarySite = sites.FirstOrDefault(p => p.IsTemporarySite());

            var result = new GetProjectSitesResponse()
            {
                PermanentSite = MapToSite(permanentSite),
                TemporarySite = MapToSite(temporarySite),
                SchoolName = project.ProjectStatusCurrentFreeSchoolName,
                ProjectType = "New School"
            };

            return result;
        }

        private static ProjectSite MapToSite(Property property)
        {
            if (property == null)
            {
                return new ProjectSite();
            }

            return new ProjectSite()
            {
                Address = new()
                {
                    AddressLine1 = property.SiteNameOfSite,
                    AddressLine2 = property.SiteAddressOfSite,
                    TownOrCity = property.TownOrCity,
                    Postcode = property.SitePostcodeOfSite,
                },
                StartDateOfSiteOccupation = property.SiteStartDateOfSchoolOccupationActual,
                DatePlanningPermissionObtained = property.DatePlanningPermissionObtained,
            };
        }
    }
}
