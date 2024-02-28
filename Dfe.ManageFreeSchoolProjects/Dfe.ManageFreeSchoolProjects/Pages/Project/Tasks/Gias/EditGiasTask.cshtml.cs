using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;

using System.Threading.Tasks;


namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.Gias
{
    public class EditGiasTaskModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly ILogger<EditGiasTaskModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }
        
        [BindProperty(Name = "checked-trust-information")]
        public bool? CheckedTrustInformation { get; set; }
        
        [BindProperty(Name = "application-form-sent")]
        
        public bool? ApplicationFormSent { get; set; }
        
        [BindProperty(Name = "saved-to-workplaces")]
        
        public bool? SavedToWorkplaces { get; set; }
        
        [BindProperty(Name = "urn-Sent")]
        
        public bool? UrnSent { get; set; }
        public string SchoolName { get; set; }

        public EditGiasTaskModel(IGetProjectByTaskService getProjectService,
            IUpdateProjectByTaskService updateProjectTaskService,
            ILogger<EditGiasTaskModel> logger,
            ErrorService errorService)
        {
            _getProjectService = getProjectService;
            _updateProjectTaskService = updateProjectTaskService;
            _logger = logger;
            _errorService = errorService;
        }

        public async Task<ActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            await LoadProject();
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            var project = await _getProjectService.Execute(ProjectId, TaskName.Gias);
            SchoolName = project.SchoolName;

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            try
            {
                var request = new UpdateProjectByTaskRequest()
                {
                    Gias = new GiasTask()
                    {
                        CheckedTrustInformation = CheckedTrustInformation ?? false,
                        ApplicationFormSent = ApplicationFormSent ?? false,
                        SavedToWorkspaces = SavedToWorkplaces ?? false,
                        UrnSent = UrnSent ?? false
                    }
                };

                await _updateProjectTaskService.Execute(ProjectId, request);
                return Redirect(string.Format(RouteConstants.ViewGiasTask, ProjectId));
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
                throw;
            }
        }

        private async Task LoadProject()
        {
            var project = await _getProjectService.Execute(ProjectId, TaskName.Gias);

            CheckedTrustInformation = project.Gias.CheckedTrustInformation;
            ApplicationFormSent = project.Gias.CheckedTrustInformation;
            SavedToWorkplaces = project.Gias.SavedToWorkspaces;
            UrnSent = project.Gias.SavedToWorkspaces;
           
            SchoolName = project.SchoolName;
        }
    }
}
