using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Task;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks
{
    public class ViewTaskBaseModel(
        IGetProjectByTaskService getProjectService,
        IGetTaskStatusService getTaskStatusService,
        IUpdateTaskStatusService updateTaskStatusService)
        : PageModel
    {
        protected readonly IGetProjectByTaskService GetProjectService = getProjectService;
        protected readonly IGetTaskStatusService GetTaskStatusService = getTaskStatusService;
        protected readonly IUpdateTaskStatusService UpdateTaskStatusService = updateTaskStatusService;

        public ProjectTaskStatus ProjectTaskStatus { get; set; }

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "mark-as-completed")]
        public bool MarkAsCompleted { get; set; }

        public bool IsPresumptionRoute { get; set; }

        public GetProjectByTaskResponse Project { get; set; }

        protected async Task GetTask(TaskName taskName)
        {
            Project = await GetProjectService.Execute(ProjectId, taskName);

            var taskStatusResponse = await GetTaskStatusService.Execute(ProjectId, taskName.ToString());

            ProjectTaskStatus = taskStatusResponse.ProjectTaskStatus;
            MarkAsCompleted = ProjectTaskStatus == ProjectTaskStatus.Completed;

            IsPresumptionRoute = Project.IsPresumptionRoute;

        }

        protected async Task PostTask(TaskName taskName)
        {
            ProjectTaskStatus = MarkAsCompleted ? ProjectTaskStatus.Completed : ProjectTaskStatus.InProgress;

            await UpdateTaskStatusService.Execute(ProjectId, new UpdateTaskStatusRequest
            {
                TaskName = taskName.ToString(),
                ProjectTaskStatus = ProjectTaskStatus
            });
        }
    }
}
