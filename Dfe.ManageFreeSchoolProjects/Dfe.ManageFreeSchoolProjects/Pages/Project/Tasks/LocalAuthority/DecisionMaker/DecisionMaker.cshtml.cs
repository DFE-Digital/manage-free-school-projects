using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Task;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.DecisionMaker
{
    public class DecisionMakerModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly IUpdateTaskStatusService _updateTaskStatusService;
        private readonly ILogger<DecisionMakerModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }
        public string CurrentFreeSchoolName { get; set; }

        [BindProperty(Name = "decision-maker")]
        public string DecisionMaker { get; set; }

        public List<string> Options { get; } =
        [
            "Local authority",
            "Regional Director on behalf of the Secretary of State"
        ];

        public DecisionMakerModel(
            IGetProjectByTaskService getProjectService,
            IUpdateProjectByTaskService updateProjectTaskService,
            IUpdateTaskStatusService updateTaskStatusService,
            ILogger<DecisionMakerModel> logger,
            ErrorService errorService)
        {
            _getProjectService = getProjectService;
            _updateProjectTaskService = updateProjectTaskService;
            _updateTaskStatusService = updateTaskStatusService;
            _logger = logger;
            _errorService = errorService;
        }

        public async Task<IActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            try
            {
                var project = await _getProjectService.Execute(ProjectId, TaskName.NewSchoolDecisionMaker);
                CurrentFreeSchoolName = project.SchoolName;
                DecisionMaker = project.NewSchoolDecisionMaker?.NewSchoolDecisionMaker;
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

            _errorService.AddErrors(ModelState.Keys, ModelState);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var request = new UpdateProjectByTaskRequest
                {
                    NewSchoolDecisionMaker = new NewSchoolDecisionMakerTask
                    {
                        NewSchoolDecisionMaker = DecisionMaker
                    }
                };

                await _updateProjectTaskService.Execute(ProjectId, request);

                if (DecisionMaker is not null)
                {
                    await UpdateStatusAsync(ProjectTaskStatus.Completed);
                }
                else
                {
                    await UpdateStatusAsync(ProjectTaskStatus.NotStarted);
                }

                return Redirect(string.Format(RouteConstants.TaskList, ProjectId));
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
                throw;
            }
        }

        private async Task UpdateStatusAsync(ProjectTaskStatus status)
        {
            await _updateTaskStatusService.Execute(ProjectId, new UpdateTaskStatusRequest
            {
                TaskName = TaskName.NewSchoolDecisionMaker.ToString(),
                ProjectTaskStatus = status
            });
        }
    }
}
