using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Risk;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using ProjectStatusType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectStatus;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Risk
{
    public class RiskAndRatingSummaryModel : PageModel
    {
        private readonly IGetProjectRiskService _getProjectRiskRatingService;

        private readonly IGetProjectOverviewService _getProjectOverviewService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        public GetProjectRiskResponse ProjectRisk { get; set; }

        public ProjectStatusType ProjectStatus { get; set; }

        public string SchoolName { get; set; }

        public string ProjectType { get; set; }

        public RiskAndRatingSummaryModel(
            IGetProjectRiskService getProjectRiskRatingService,
            IGetProjectOverviewService getProjectOverviewService)
        {
            _getProjectRiskRatingService = getProjectRiskRatingService;
            _getProjectOverviewService = getProjectOverviewService;
        }

        public async Task<IActionResult> OnGet(int entry = 1)
        {

            var projectId = RouteData.Values["projectId"] as string;

            var project = await _getProjectOverviewService.Execute(projectId);
            SchoolName = project.ProjectStatus.CurrentFreeSchoolName;
            ProjectStatus = project.ProjectStatus.ProjectStatus;
            ProjectType = project.ProjectType;

            ProjectRisk = new GetProjectRiskResponse();
            var projectRiskResponse = await _getProjectRiskRatingService.Execute(ProjectId, entry);
            ProjectRisk = projectRiskResponse;

            return Page();
        }
    }
}
