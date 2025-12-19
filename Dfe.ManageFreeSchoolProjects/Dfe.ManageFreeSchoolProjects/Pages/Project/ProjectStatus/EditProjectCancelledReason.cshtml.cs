using System;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProjectStatusType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectStatus;
using ProjectCancelledReasonType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectCancelledReason;
using ProjectWithdrawnReasonType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectWithdrawnReason;
using Dfe.ManageFreeSchoolProjects.Models;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Common;
using Dfe.ManageFreeSchoolProjects.Extensions;
using Dfe.ManageFreeSchoolProjects.Validators;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.ProjectCancelledReason
{
    public class EditProjectCancelledModel(
        IGetProjectOverviewService getProjectOverviewService,
        IUpdateProjectStatusService updateProjectStatusService,
        ILogger<EditProjectCancelledModel> logger,
        IHttpContextAccessor httpContextAccessor,
        ErrorService errorService)
        : PageModel
    {
        public const string CancelledYearId = "year-cancelled";
        public const string SelectedCancelledStatus = "SelectedCancelledStatus";

        public ProjectOverviewResponse Project { get; set; }
        
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [Required]
        [BindProperty(Name = CancelledYearId, BinderType = typeof(DateInputModelBinder))]
        [Display(Name = "Date the project was cancelled")]
        [DateValidation(DateRangeValidationService.DateRange.PastOrFuture)]
        public DateTime? CancelledYear { get; set; }

        [Required (ErrorMessage = "Enter the main reason for cancellation")]
        [BindProperty(Name = "project-cancelled-reason-type")]
        [Display(Name = "Main reason for cancellation")]
        public ProjectCancelledReasonType ProjectCancelledReason { get; set; }

        [Required(ErrorMessage = "Enter whether the project was cancelled as a result of the 2024/25 national review of pipeline projects")]
        [BindProperty(Name = "project-cancelled-as-a-result-of-national-review-of-pipeline")]
        [Display(Name = "Project cancelled as a result of the 2024/25 national review of pipeline projects")]
        public YesNo? ProjectCancelledAsAResultOfNationalPipelineReview { get; set; }

       
        [Required(ErrorMessage = "Enter the notes about the cancellation")]
        [BindProperty(Name = "add-notes-about-the-cancellation")]
        [Display(Name = "Notes about the cancellation")]
        [ValidText(500)]
        public string Notes { get; set; }

        public ProjectStatusType SelectedProjectStatus { get; set; } = ProjectStatusType.Cancelled;

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var projectId = RouteData.Values["projectId"] as string;

                if (TempData.TryGetValue(SelectedCancelledStatus, out object statusValue))
                {
                    SelectedProjectStatus = (ProjectStatusType)statusValue;
                    TempData.Keep(SelectedCancelledStatus);
                }

                Project = await getProjectOverviewService.Execute(projectId);
                CancelledYear = Project.ProjectStatus.ProjectCancelledDate;
                ProjectCancelledReason = Project.ProjectStatus.ProjectCancelledReason;
                ProjectCancelledAsAResultOfNationalPipelineReview = Project.ProjectStatus.ProjectCancelledDueToNationalReviewOfPipelineProjects;
                Notes = Project.ProjectStatus.CommentaryForCancellation;
            }
            catch (Exception ex)
            {
                logger.LogErrorMsg(ex);
            }

            ProjectId = Project.ProjectStatus.ProjectId;
            TempData["projectStatusUpdated"] = false;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var yearFormatErrorMessage = "Enter a date in the correct format";

            if (ModelState.IsValid && CancelledYear == null)
            {
                ModelState.AddModelError(CancelledYearId, yearFormatErrorMessage);
            }

            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);
                Project = await getProjectOverviewService.Execute(ProjectId);

                return Page();
            }

            var projectStatus = ProjectStatusType.Cancelled;
            if (TempData.TryGetValue(SelectedCancelledStatus, out object statusValue))
            {
                projectStatus = (ProjectStatusType)statusValue;
            }

            UpdateProjectStatusRequest request = new UpdateProjectStatusRequest()
            {
                ProjectStatus = projectStatus,

                CancelledDate = CancelledYear,
                ProjectCancelledReason = ProjectCancelledReason,
                ProjectCancelledDueToNationalReviewOfPipelineProjects = ProjectCancelledAsAResultOfNationalPipelineReview,
                CommentaryForCancellation = Notes,

                WithdrawnDate = null,
                ProjectWithdrawnReason = ProjectWithdrawnReasonType.NotSet,
                ProjectWithdrawnDueToNationalReviewOfPipelineProjects = null,
                CommentaryForWithdrawal = null,

                ClosedDate = null
            };

            var projectId = RouteData.Values["projectId"] as string;

            await updateProjectStatusService.Execute(projectId, request);
            TempData["projectStatusUpdated"] = true;
            return Redirect(GetNextPage());
        }

       
        
        public string GetNextPage()
        {
            var referrerQuery = httpContextAccessor.HttpContext.Request.Query["referrer"];

            var isReferred = Enum.TryParse(referrerQuery, out Referrer referrer);

            if (referrer == Referrer.ProjectOverview)
            {
                return string.Format(RouteConstants.ProjectOverview, ProjectId);
            }
            
            else if (referrer == Referrer.TaskList)
            {
                return string.Format(RouteConstants.TaskList, ProjectId);
            }
            
            else if (referrer == Referrer.ContactsOverview)
            {
                return string.Format(RouteConstants.Contacts, ProjectId);
            }

            if (!isReferred)
            {
                return string.Format(RouteConstants.ProjectOverview, ProjectId);
            }

            return string.Format(RouteConstants.ProjectOverview, ProjectId);
        }
    }
}
