using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Task;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Models;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.DateOfDecision
{
    public class DateOfDecisionModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly IUpdateTaskStatusService _updateTaskStatusService;
        private readonly ILogger<DateOfDecisionModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }
        public string CurrentFreeSchoolName { get; set; }

        [BindProperty(Name = "date-of-decision", BinderType = typeof(DateInputModelBinder))]
        [Display(Name = "date-of-decision")]
        [DateValidation(DateRangeValidationService.DateRange.PastOrFuture)]
        public DateTime? DateOfDecision { get; set; }

        public DateOfDecisionModel(
            IGetProjectByTaskService getProjectService,
            IUpdateProjectByTaskService updateProjectTaskService,
            IUpdateTaskStatusService updateTaskStatusService,
            ILogger<DateOfDecisionModel> logger,
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
                var project = await _getProjectService.Execute(ProjectId, TaskName.NewSchoolDateOfDecision);
                CurrentFreeSchoolName = project.SchoolName;
                DateOfDecision = project.NewSchoolDateOfDecision?.NewSchoolDateOfDecision;
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
                var request = new UpdateProjectByTaskRequest()
                {
                    NewSchoolDateOfDecision = new NewSchoolDateOfDecisionTask()
                    {
                        NewSchoolDateOfDecision = DateOfDecision
                    }
                };

                await _updateProjectTaskService.Execute(ProjectId, request);

                if (DateOfDecision is not null)
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
                TaskName = TaskName.NewSchoolDateOfDecision.ToString(),
                ProjectTaskStatus = status
            });
        }
    }
}
