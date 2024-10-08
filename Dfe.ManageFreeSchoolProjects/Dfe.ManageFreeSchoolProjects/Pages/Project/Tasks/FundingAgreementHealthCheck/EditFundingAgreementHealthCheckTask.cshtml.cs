using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;



namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.FundingAgreementHealthCheck
{
    public class EditFundingAgreementHealthCheckTaskModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly ILogger<EditFundingAgreementHealthCheckTaskModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }


        [BindProperty(Name = "drafted-fa-health-check")]
        public bool? DraftedFAHealthCheck { get; set; }

        [BindProperty(Name = "regional-director-signed-off-fa-health-check")]
        public bool? RegionalDirectorSignedOffFaHealthCheck { get; set; }

        [BindProperty(Name = "minister-signed-off-fa-health-check")]
        public bool? MinisterSignedOffFaHealthCheck { get; set; }

        [BindProperty(Name = "included-signed-off-impact-assessment")]
        public bool? IncludedSignedOffImpactAssessment { get; set; }

        [BindProperty(Name = "saved-fa-health-check-in-workplaces-folder")]
        public bool? SavedFaHealthCheckInWorkplacesFolder { get; set; }

        [BindProperty]
        public string SchoolName { get; set; }
        public bool IsPresumptionRoute { get; set; }

        public EditFundingAgreementHealthCheckTaskModel(
            IGetProjectByTaskService getProjectService,
            IUpdateProjectByTaskService updateProjectTaskService,
            ILogger<EditFundingAgreementHealthCheckTaskModel> logger,
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

            if (!IsPresumptionRoute)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            var updateTaskRequest = new UpdateProjectByTaskRequest()
            {
                FundingAgreementHealthCheck = new()
                {
                    DraftedFundingAgreementHealthCheck = DraftedFAHealthCheck,
                    RegionalDirectorSignedOffFundingAgreementHealthCheck = RegionalDirectorSignedOffFaHealthCheck,
                    MinisterSignedOffFundingAgreementHealthCheck = MinisterSignedOffFaHealthCheck,
                    IncludedSignedOffImpactAssessment = IncludedSignedOffImpactAssessment,
                    SavedFundingAgreementHealthCheckInWorkplacesFolder = SavedFaHealthCheckInWorkplacesFolder
                }
            };

            await _updateProjectTaskService.Execute(ProjectId, updateTaskRequest);

            return Redirect(string.Format(RouteConstants.ViewFundingAgreementHealthCheckTask, ProjectId));
        }

        private async Task LoadProject()
        {
            var project = await _getProjectService.Execute(ProjectId, TaskName.FundingAgreementHealthCheck);

            DraftedFAHealthCheck = project.FundingAgreementHealthCheck.DraftedFundingAgreementHealthCheck;
            RegionalDirectorSignedOffFaHealthCheck = project.FundingAgreementHealthCheck.RegionalDirectorSignedOffFundingAgreementHealthCheck;
            MinisterSignedOffFaHealthCheck = project.FundingAgreementHealthCheck.MinisterSignedOffFundingAgreementHealthCheck;
            IncludedSignedOffImpactAssessment = project.FundingAgreementHealthCheck.IncludedSignedOffImpactAssessment;
            SavedFaHealthCheckInWorkplacesFolder = project.FundingAgreementHealthCheck.SavedFundingAgreementHealthCheckInWorkplacesFolder;

            SchoolName = project.SchoolName;
            IsPresumptionRoute = project.IsPresumptionRoute;
        }

    }
}
