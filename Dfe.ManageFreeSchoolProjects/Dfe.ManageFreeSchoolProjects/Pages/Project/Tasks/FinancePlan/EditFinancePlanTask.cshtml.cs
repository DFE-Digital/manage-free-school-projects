using Dfe.ManageFreeSchoolProjects.API.Contracts.Common;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Models;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.Validators;
using Dfe.ManageFreeSchoolProjects.Constants;


namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.FinancePlan
{
    public class EditFinancePlanTaskModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly ILogger<EditFinancePlanTaskModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }


        [BindProperty(Name = "finance-plan-agreed")]
        public bool? FinancePlanAgreed { get; set; }

        [BindProperty(Name = "date-agreed", BinderType = typeof(DateInputModelBinder))]
        [Display(Name = "Date agreed")]

        public DateTime? DateAgreed { get; set; }

        [BindProperty(Name = "plan-saved-in-workplaces-folder")]
        public bool? PlanSavedInWorkplacesFolder { get; set; }

        [BindProperty(Name = "local-authority-agreed-to-pupil-numbers")]
        public string? LocalAuthorityAgreedToPupilNumbers { get; set; }

        [BindProperty(Name = "comments")]
        [Display(Name = "Comments")]
        [ValidText(999)]
        public string Comments { get; set; }

        [BindProperty(Name = "trust-opt-into-rpa")]
        public bool? TrustOptIntoRpa { get; set; }

        [BindProperty]
        public string SchoolName { get; set; }

        public EditFinancePlanTaskModel(
            IGetProjectByTaskService getProjectService,
            IUpdateProjectByTaskService updateProjectTaskService,
            ILogger<EditFinancePlanTaskModel> logger,
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
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            var updateTaskRequest = new UpdateProjectByTaskRequest()
            {
                FinancePlan = new()
                {
                    FinancePlanAgreed = FinancePlanAgreed == true ? YesNo.Yes : YesNo.No,
                    DateAgreed = DateAgreed,
                    PlanSavedInWorkspaceFolder = PlanSavedInWorkplacesFolder == true ? YesNo.Yes : YesNo.No,
                    Comments = Comments,
                    LocalAuthorityAgreedPupilNumbers = ConvertYesNoNotApplicable(LocalAuthorityAgreedToPupilNumbers),
                    TrustWillOptIntoRpa = TrustOptIntoRpa == true ? YesNo.Yes : YesNo.No
                }
            };

            await _updateProjectTaskService.Execute(ProjectId, updateTaskRequest);

            return Redirect(string.Format(RouteConstants.ViewFinancePlanTask, ProjectId));
        }

        private async Task LoadProject()
        {
            var project = await _getProjectService.Execute(ProjectId, TaskName.FinancePlan);

            FinancePlanAgreed = project.FinancePlan.FinancePlanAgreed == YesNo.Yes;
            DateAgreed = project.FinancePlan.DateAgreed;
            PlanSavedInWorkplacesFolder = project.FinancePlan.PlanSavedInWorkspaceFolder == YesNo.Yes;
            Comments = project.FinancePlan.Comments;
            LocalAuthorityAgreedToPupilNumbers = project.FinancePlan.LocalAuthorityAgreedPupilNumbers?.ToString();
            TrustOptIntoRpa = project.FinancePlan.TrustWillOptIntoRpa == YesNo.Yes;

            SchoolName = project.SchoolName;
        }

        private static YesNoNotApplicable? ConvertYesNoNotApplicable(string value)
        {
            return Enum.TryParse<YesNoNotApplicable>(value, true, out var result) ? result : null;
        }
    }
}
