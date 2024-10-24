using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ProjectStatusType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectStatus;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.SiteInformation
{
    public class ViewSiteInformationModel : PageModel
    {
        private readonly IGetProjectSitesService _getProjectSitesService;
        private readonly IGetProjectOverviewService _getProjectOverviewService;
        private readonly ILogger<ViewSiteInformationModel> _logger;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        public ProjectStatusType ProjectStatus { get; set; }

        public string SchoolName { get; set; }

        public GetProjectSitesResponse SiteInformation { get; set; }

        public ViewSiteInformationModel(
            IGetProjectSitesService getProjectSitesService,
            IGetProjectOverviewService getProjectOverviewService,
            ILogger<ViewSiteInformationModel> logger)
        {
            _getProjectSitesService = getProjectSitesService;
            _getProjectOverviewService = getProjectOverviewService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            SiteInformation = await _getProjectSitesService.Execute(ProjectId);

            var project = await _getProjectOverviewService.Execute(ProjectId);

            ProjectStatus = project.ProjectStatus.ProjectStatus;

            SchoolName = project.ProjectStatus.CurrentFreeSchoolName;

            return Page();
        }
    }
}
