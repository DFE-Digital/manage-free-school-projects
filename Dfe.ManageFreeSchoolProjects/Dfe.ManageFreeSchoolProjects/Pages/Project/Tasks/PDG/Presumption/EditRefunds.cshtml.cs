using Dfe.ManageFreeSchoolProjects.Models;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Logging;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Validators;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.PDG.Presumption
{
    public class EditRefundsModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly ILogger<EditRefundsModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        public string CurrentFreeSchoolName { get; set; }

        [BindProperty(Name = "latest-refund-date", BinderType = typeof(DateInputModelBinder))]
        [Display(Name = "Latest refund date")]
        [DateValidation(DateRangeValidationService.DateRange.PastOrFuture)]
        public DateTime? LatestRefundDate { get; set; }

        [BindProperty(Name = "total-amount", BinderType = typeof(DecimalInputModelBinder))]
        [Display(Name = "Total amount")]
        [ValidMoney(0, 25000)]
        public decimal? TotalAmount { get; set; }

        public EditRefundsModel(IGetProjectByTaskService getProjectService, IUpdateProjectByTaskService updateProjectTaskService, ILogger<EditRefundsModel> logger,
            ErrorService errorService)
        {
            _getProjectService = getProjectService;
            _logger = logger;
            _errorService = errorService;
            _updateProjectTaskService = updateProjectTaskService;
        }

        public async Task<ActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            if (!User.IsInRole(RolesConstants.GrantManagers))
            {
                return new UnauthorizedResult();
            }

            await LoadProject();
            return Page();
        }

        private async Task LoadProject()
        {
            var project = await _getProjectService.Execute(ProjectId, TaskName.Refunds);

            LatestRefundDate = project.Refunds.LatestRefundDate;
            TotalAmount = project.Refunds.TotalAmount;
            CurrentFreeSchoolName = project.SchoolName;
        }

        public async Task<ActionResult> OnPost()
        {
            _logger.LogMethodEntered();

            if (!User.IsInRole(RolesConstants.GrantManagers))
            {
                return new UnauthorizedResult();
            }

            var project = await _getProjectService.Execute(ProjectId, TaskName.Refunds);
            CurrentFreeSchoolName = project.SchoolName;

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            try
            {
                var request = new UpdateProjectByTaskRequest()
                {
                    Refunds = new()
                    {
                        LatestRefundDate = LatestRefundDate,
                        TotalAmount = TotalAmount,
                    }
                };

                await _updateProjectTaskService.Execute(ProjectId, request);

                return Redirect(string.Format(RouteConstants.ViewPDGPresumption, ProjectId));
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
                throw;
            }
        }
    }
}
