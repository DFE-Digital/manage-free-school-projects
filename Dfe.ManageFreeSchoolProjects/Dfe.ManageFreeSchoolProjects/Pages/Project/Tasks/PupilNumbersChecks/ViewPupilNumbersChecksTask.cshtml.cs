using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Logging;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.PupilNumbersChecks
{
    public class ViewPupilNumbersChecksModel : ViewTaskBaseModel
    {
        public ProjectByTaskSummaryResponse ProjectTaskListSummary { get; set; }
        
        private readonly ILogger<ViewPupilNumbersChecksModel> _logger;

        public ViewPupilNumbersChecksModel(
            IGetProjectByTaskService getProjectService,
            ILogger<ViewPupilNumbersChecksModel> logger,
            IGetTaskStatusService getTaskStatusService, IUpdateTaskStatusService updateTaskStatusService) : base(getProjectService, getTaskStatusService, updateTaskStatusService)
        {
            _logger = logger;
        }
        public ProjectOverviewResponse ProjectOverview { get; set; }
        
        public async Task<ActionResult> OnGet()
        {
            _logger.LogMethodEntered();
            
            await GetTask(TaskName.PupilNumbersChecks);

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            _logger.LogMethodEntered();
            
            await PostTask(TaskName.PupilNumbersChecks);

            return Redirect(string.Format(RouteConstants.TaskList, ProjectId));
        }
    }
}
