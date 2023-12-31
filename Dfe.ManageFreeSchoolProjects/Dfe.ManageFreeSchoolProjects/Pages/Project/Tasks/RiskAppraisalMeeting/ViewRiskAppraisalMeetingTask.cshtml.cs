﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Dfe.ManageFreeSchoolProjects.Logging;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Task;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.RiskAppraisalMeeting
{
    public class ViewRiskAppraisalMeetingTaskModel : PageModel
    {
        private readonly ILogger<ViewRiskAppraisalMeetingTaskModel> _logger;
        private readonly IGetTaskStatusService _getTaskStatusService;
        private readonly IUpdateTaskStatusService _updateTaskStatusService;
        private readonly ErrorService _errorService;
        private readonly IGetProjectByTaskService _getProjectService;
        private const string TaskName = "RiskAppraisalMeeting";

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        public string CurrentFreeSchoolName { get; set; }

        [BindProperty]
        public bool MarkAsCompleted { get; set; }

        public ProjectTaskStatus ProjectTaskStatus { get; set; }

        public GetProjectByTaskResponse Project { get; set; }


        public ViewRiskAppraisalMeetingTaskModel(
            IGetProjectByTaskService getProjectService,
            ILogger<ViewRiskAppraisalMeetingTaskModel> logger, IGetTaskStatusService getTaskStatusService,
            IUpdateTaskStatusService updateTaskStatusService,
            ErrorService errorService)
        {
            _logger = logger;
            _errorService = errorService;
            _getTaskStatusService = getTaskStatusService;
            _updateTaskStatusService = updateTaskStatusService;
            _getProjectService = getProjectService;
        }

        public async Task<IActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            Project = await _getProjectService.Execute(ProjectId);

            var taskStatusResponse = await _getTaskStatusService.Execute(ProjectId, TaskName);
            CurrentFreeSchoolName = Project.School.CurrentFreeSchoolName;
            ProjectTaskStatus = taskStatusResponse.ProjectTaskStatus;
            MarkAsCompleted = ProjectTaskStatus == ProjectTaskStatus.Completed;

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            ProjectTaskStatus = MarkAsCompleted ? ProjectTaskStatus.Completed : ProjectTaskStatus.InProgress;

            await _updateTaskStatusService.Execute(ProjectId, new UpdateTaskStatusRequest
            {
                TaskName = TaskName,
                ProjectTaskStatus = ProjectTaskStatus
            });

            return Redirect(string.Format(RouteConstants.TaskList, ProjectId));
        }
    }
}