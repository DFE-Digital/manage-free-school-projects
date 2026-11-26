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
using Dfe.ManageFreeSchoolProjects.Constants;
namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.PrincipalDesignate
{
    public class EditPrincipalDesignateTaskModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly ILogger<EditPrincipalDesignateTaskModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "commissioned-external-expert-visit")]
        public string CommissionedExternalExpertVisit { get; set; }

        [BindProperty(Name = "expected-date-that-principal-designate-will-be-appointed", BinderType = typeof(DateInputModelBinder))]
        [Display(Name = "Expected date that principal designate will be appointed")]
        public DateTime? ExpectedDatePrincipalDesignateAppointed { get; set; }

        [BindProperty(Name = "principal-designate-appointed")]
        public bool? PrincipalDesignateAppointed { get; set; }

        [BindProperty(Name = "actual-date-that-principal-designate-was-appointed", BinderType = typeof(DateInputModelBinder))]
        [Display(Name = "Actual date that principal designate was appointed")]
        public DateTime? ActualDatePrincipalDesignateAppointed { get; set; }
        
        [BindProperty]
        public string SchoolName { get; set; }

        public EditPrincipalDesignateTaskModel(
            IGetProjectByTaskService getProjectService,
            IUpdateProjectByTaskService updateProjectTaskService,
            ILogger<EditPrincipalDesignateTaskModel> logger,
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

            var principalDesignateTask = new PrincipalDesignateTask();
            
            principalDesignateTask.TrustAppointedPrincipalDesignate = PrincipalDesignateAppointed;
            principalDesignateTask.CommissionedExternalExpertVisitToSchool = ConvertYesNoNotApplicable(CommissionedExternalExpertVisit);
            principalDesignateTask.ExpectedDatePrincipalDesignateAppointed = ExpectedDatePrincipalDesignateAppointed;
            principalDesignateTask.ActualDatePrincipalDesignateAppointed = ActualDatePrincipalDesignateAppointed;

            var updateTaskRequest = new UpdateProjectByTaskRequest()
            {
                PrincipalDesignate = principalDesignateTask
            };
            
            await _updateProjectTaskService.Execute(ProjectId, updateTaskRequest);

            return Redirect(string.Format(RouteConstants.ViewPrincipalDesignateTask, ProjectId));
        }

        private async Task LoadProject()
        {
            var project = await _getProjectService.Execute(ProjectId, TaskName.PrincipalDesignate);
            
            SchoolName = project.SchoolName;

            CommissionedExternalExpertVisit = project.PrincipalDesignate.CommissionedExternalExpertVisitToSchool?.ToString();
            ExpectedDatePrincipalDesignateAppointed = project.PrincipalDesignate.ExpectedDatePrincipalDesignateAppointed;
            PrincipalDesignateAppointed = project.PrincipalDesignate.TrustAppointedPrincipalDesignate;
            ActualDatePrincipalDesignateAppointed = project.PrincipalDesignate.ActualDatePrincipalDesignateAppointed;
            
        }
        private static YesNoNotApplicable? ConvertYesNoNotApplicable(string value)
        {
            return Enum.TryParse<YesNoNotApplicable>(value, true, out var result) ? result : null;
        }

        public static YesNo? ConvertYesNo(string value)
        {
            return Enum.TryParse<YesNo>(value, true, out var result) ? result : null;
        }
    }
}
