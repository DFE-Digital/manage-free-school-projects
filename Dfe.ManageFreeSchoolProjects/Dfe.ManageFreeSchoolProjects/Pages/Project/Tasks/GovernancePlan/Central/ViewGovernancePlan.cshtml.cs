using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Risk;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.GovernancePlan.Central
{
    public class ViewGovernancePlanModel : ViewTaskBaseModel
    {
        private readonly ILogger<ViewGovernancePlanModel> _logger;
        private readonly IGetProjectRiskService _getProjectRiskService;

        public GetProjectRiskResponse ProjectRisk { get; set; }

        public ViewGovernancePlanModel(
            IGetProjectByTaskService getProjectService,
            ILogger<ViewGovernancePlanModel> logger,
            IGetTaskStatusService getTaskStatusService, IUpdateTaskStatusService updateTaskStatusService,
            IGetProjectRiskService getProjectRiskService) : base(getProjectService, getTaskStatusService, updateTaskStatusService)
        {
            _logger = logger;
            _getProjectRiskService = getProjectRiskService;
        }

        public async Task<ActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            await GetTask(TaskName.GovernancePlan);

            ProjectRisk = await _getProjectRiskService.Execute(ProjectId, 1);

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            _logger.LogMethodEntered();

            await PostTask(TaskName.GovernancePlan);

            return Redirect(string.Format(RouteConstants.TaskList, ProjectId));
        }
    }
}
