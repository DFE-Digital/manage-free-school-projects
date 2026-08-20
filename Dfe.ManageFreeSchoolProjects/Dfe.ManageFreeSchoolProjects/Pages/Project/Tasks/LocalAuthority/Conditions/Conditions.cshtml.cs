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
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.Conditions
{
    public class ConditionsModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly IUpdateTaskStatusService _updateTaskStatusService;
        private readonly ILogger<ConditionsModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }
        public string CurrentFreeSchoolName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select an option to confirm the conditions applied")]
        public YesNoOption? ConditionOption { get; set; }

        [BindProperty(Name = "condition-description")]
        public string ConditionDescription { get; set; }

        public ConditionsModel(
            IGetProjectByTaskService getProjectService,
            IUpdateProjectByTaskService updateProjectTaskService,
            IUpdateTaskStatusService updateTaskStatusService,
            ILogger<ConditionsModel> logger,
            ErrorService errorService)
        {
            _getProjectService = getProjectService;
            _updateProjectTaskService = updateProjectTaskService;
            _updateTaskStatusService = updateTaskStatusService;
            _logger = logger;
            _errorService = errorService;
        }

        private async Task ReadAsync()
        {
            var project = await _getProjectService.Execute(ProjectId, TaskName.NewSchoolConditions);
            CurrentFreeSchoolName = project.SchoolName;

            if (project.NewSchoolConditions != null)
            {
                ConditionOption = YesNoOption.No;
                if (!string.IsNullOrWhiteSpace(project.NewSchoolConditions.NewSchoolConditions) && project.NewSchoolConditions.NewSchoolConditions == "Yes")
                {
                    ConditionOption = YesNoOption.Yes;
                }

                ConditionDescription = project.NewSchoolConditions.NewSchoolConditionsDescription;
            }
        }

        public async Task<IActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            try
            {
                await ReadAsync();
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
                string hasConditions = ConditionOption == YesNoOption.Yes ? "Yes" : "No";
                string description = ConditionOption == YesNoOption.Yes ? ConditionDescription : string.Empty;

                var request = new UpdateProjectByTaskRequest()
                {
                    NewSchoolConditions = new NewSchoolConditionsTask()
                    {
                        NewSchoolConditions = hasConditions,
                        NewSchoolConditionsDescription = description
                    }
                };

                await _updateProjectTaskService.Execute(ProjectId, request);

                if (ConditionOption is not null)
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
                TaskName = TaskName.NewSchoolConditions.ToString(),
                ProjectTaskStatus = status
            });
        }
    }
}
