using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;
using ProjectStatusType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectStatus;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks
{
    public class TaskListModel(IGetProjectByTaskSummaryService getProjectTaskListSummaryService, 
        IGetProjectOverviewService getProjectOverviewService,
        ICreateTasksService createTasksService,
        ILogger<TaskListModel> logger)
        : PageModel
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }
        
        public ProjectStatusType ProjectStatus { get; set; }
        public SchoolType? SchoolType{ get; set; }

        [BindProperty(Name = "schoolName")]
        public string SchoolName { get; set; }
        public string ProjectType { get; set; }
        
        public bool? IsPresumptionRoute { get; set; }

        public ProjectByTaskSummaryResponse ProjectTaskListSummary { get; set; }


        public async Task<IActionResult> OnGet()
        {
            logger.LogMethodEntered();

            ProjectTaskListSummary = await getProjectTaskListSummaryService.Execute(ProjectId);

            var project = await getProjectOverviewService.Execute(ProjectId);
            
            SchoolType = project.SchoolDetails.SchoolType;

            ProjectStatus = project.ProjectStatus.ProjectStatus;

            ProjectType = project.ProjectType;

            IsPresumptionRoute =
                !string.IsNullOrEmpty(project.ProjectStatus.ApplicationWave) &&
                project.ProjectStatus.ApplicationWave.Contains("Presumption"); 

            if (ProjectTaskListSummary is not null)
            {
                SchoolName = ProjectTaskListSummary.SchoolName;
                return Page(); 
            }                
            
            await createTasksService.Execute(ProjectId);

            ProjectTaskListSummary = await getProjectTaskListSummaryService.Execute(ProjectId);

            SchoolName = ProjectTaskListSummary.School.Name;

            return Page();
        }
    }
}