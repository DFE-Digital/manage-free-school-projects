using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.FinancePlan
{
    public class ViewFinancePlanTaskModel : ViewTaskBaseModel
    {
        private readonly IGetProjectOverviewService _getProjectOverviewService;
        private readonly ILogger<ViewFinancePlanTaskModel> _logger;

        public SchoolPhase SchoolPhase { get; set; }
        public string SchoolName { get; set; }

        public ViewFinancePlanTaskModel(
            IGetProjectByTaskService getProjectService,
            IGetProjectOverviewService getProjectOverviewService,
            ILogger<ViewFinancePlanTaskModel> logger,
            IGetTaskStatusService getTaskStatusService,
            IUpdateTaskStatusService updateTaskStatusService) : base(getProjectService, getTaskStatusService, updateTaskStatusService)
        {
            _getProjectOverviewService = getProjectOverviewService;
            _logger = logger;
        }

        public async Task<ActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            await GetTask(TaskName.FinancePlan);

            try
            {
                var projectId = RouteData.Values["projectId"] as string;
                var project = await _getProjectOverviewService.Execute(projectId);

                SchoolPhase = project.SchoolDetails.SchoolPhase;
                SchoolName = project.ProjectStatus.CurrentFreeSchoolName;
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
            }

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            _logger.LogMethodEntered();

            await PostTask(TaskName.FinancePlan);

            return Redirect(string.Format(RouteConstants.TaskList, ProjectId));
        }
    }
}
