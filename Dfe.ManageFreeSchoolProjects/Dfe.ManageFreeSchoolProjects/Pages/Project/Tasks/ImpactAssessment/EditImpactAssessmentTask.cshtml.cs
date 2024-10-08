using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.Models;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.ImpactAssessment
{
    public class EditImpactAssessmentTaskModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly ILogger<EditImpactAssessmentTaskModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "impact-assessment-done")]
        public bool? ImpactAssessmentDone { get; set; }

        [BindProperty(Name = "saved-to-workplaces")]
        public bool? SavedToWorkplaces { get; set; }

        [BindProperty(Name = "sent-section9-letter-to-local-authority")]
        [DisplayName("Sent section 9 letter to local authority")]
        public bool SentSection9LetterToLocalAuthority { get; set; }

        [BindProperty(Name = "date-sent", BinderType = typeof(DateInputModelBinder))]
        [DisplayName("Date sent")]
        public DateTime? Section9LetterDateSent { get; set; }

        public bool IsPresumptionRoute { get; set; }
        
        public string SchoolName { get; set; }

        public EditImpactAssessmentTaskModel(IGetProjectByTaskService getProjectService,
            IUpdateProjectByTaskService updateProjectTaskService,
            ILogger<EditImpactAssessmentTaskModel> logger,
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
            var project = await _getProjectService.Execute(ProjectId, TaskName.ImpactAssessment);
            SchoolName = project.SchoolName;

            if (SentSection9LetterToLocalAuthority == false)
            {
                ModelState.Keys.Where(errorKey => errorKey.StartsWith("date-sent")).ToList()
                    .ForEach(errorKey => ModelState.Remove(errorKey));
                Section9LetterDateSent = null; 
            }
            
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }
            
            if (SentSection9LetterToLocalAuthority && Section9LetterDateSent.HasValue == false)
                ModelState.AddModelError("date-sent", "Enter a date sent");
            
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }
            
            try
            {
                var request = new UpdateProjectByTaskRequest
                {
                    ImpactAssessment = new ImpactAssessmentTask
                    {
                        ImpactAssessment = ImpactAssessmentDone,
                        SavedToWorkplaces = SavedToWorkplaces,
                        Section9LetterDateSent = Section9LetterDateSent,
                    }
                };

                await _updateProjectTaskService.Execute(ProjectId, request);
                return Redirect(string.Format(RouteConstants.ViewImpactAssessmentTask, ProjectId));
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
                throw;
            }
        }

        private async Task LoadProject()
        {
            var project = await _getProjectService.Execute(ProjectId, TaskName.ImpactAssessment);

            ImpactAssessmentDone = project.ImpactAssessment.ImpactAssessment;
            SavedToWorkplaces = project.ImpactAssessment.SavedToWorkplaces;
            Section9LetterDateSent = project.ImpactAssessment.Section9LetterDateSent;
            SchoolName = project.SchoolName;
            IsPresumptionRoute = project.IsPresumptionRoute;

            SentSection9LetterToLocalAuthority = Section9LetterDateSent != null; 
        }
    }
}