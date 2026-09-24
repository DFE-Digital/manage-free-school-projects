using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace Dfe.ManageFreeSchoolProjects.Pages.Project
{
    public class ProjectOverviewModel(
        IGetProjectOverviewService getProjectOverviewService,
        IGetProjectByTaskService getProjectService,
        ILogger<ProjectOverviewModel> logger)
        : PageModel
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        public ProjectOverviewResponse ProjectOverview { get; set; }

        public GetProjectByTaskResponse DecisionTask { get; set; }

        public async Task<IActionResult> OnGet()
        {
            logger.LogMethodEntered();
            
            try
            {
                var projectId = RouteData.Values["projectId"] as string;
                ProjectOverview = await getProjectOverviewService.Execute(projectId);

                DecisionTask = await getProjectService.Execute(ProjectId, TaskName.NewSchoolDecision);
            }
            catch (Exception ex)
            {
                logger.LogErrorMsg(ex);
            }

            return Page();
        }
    }
}
