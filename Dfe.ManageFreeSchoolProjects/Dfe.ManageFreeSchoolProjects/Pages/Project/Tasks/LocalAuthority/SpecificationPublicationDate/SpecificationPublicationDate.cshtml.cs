using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Models;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.Dates;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.SpecificationPublicationDate
{
    public class SpecificationPublicationDateModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly ILogger<SpecificationPublicationDateModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }
        public string CurrentFreeSchoolName { get; set; }

        [BindProperty(Name = "specification-publication-date", BinderType = typeof(DateInputModelBinder))]
        [Display(Name = "Specification publication date")]
        [DateValidation(DateRangeValidationService.DateRange.PastOrFuture)]
        public DateTime? SpecificationPublicationDate { get; set; }

        public SpecificationPublicationDateModel(
            IGetProjectByTaskService getProjectService,
            IUpdateProjectByTaskService updateProjectTaskService,
            ILogger<SpecificationPublicationDateModel> logger,
            ErrorService errorService)
        {
            _getProjectService = getProjectService;
            _updateProjectTaskService = updateProjectTaskService;
            _logger = logger;
            _errorService = errorService;
        }

        public async Task<IActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            try
            {
                var project = await _getProjectService.Execute(ProjectId, TaskName.NewSchoolSpecificationPublicationDate);
                CurrentFreeSchoolName = project.SchoolName;
                SpecificationPublicationDate = project.NewSchool.NewSchoolSpecificationPublicationDate;
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
            }

            return Page();
        }

        //public async Task<ActionResult> OnPost()
        //{
        //    var project = await _getProjectService.Execute(ProjectId, TaskName.Dates);

        //    ProjectClosedDateHasValue = project.Dates.ProjectClosedDate.HasValue;

        //    ProjectCancelledDateHasValue = project.Dates.ProjectCancelledDate.HasValue;

        //    ProjectWithdrawnDateHasValue = project.Dates.ProjectWithdrawnDate.HasValue;



        //    if (project.Dates.ProjectClosedDate.HasValue
        //        && !project.Dates.ProjectCancelledDate.HasValue
        //        && !project.Dates.ProjectWithdrawnDate.HasValue
        //        )
        //    {
        //        ModelState.Remove("project-cancelled-date");
        //        ModelState.Remove("project-withdrawn-date");
        //    }

        //    if (project.Dates.ProjectCancelledDate.HasValue
        //        && !project.Dates.ProjectClosedDate.HasValue
        //        && !project.Dates.ProjectWithdrawnDate.HasValue
        //        )
        //    {
        //        ModelState.Remove("project-closed-date");
        //        ModelState.Remove("project-withdrawn-date");
        //    }

        //    if (project.Dates.ProjectWithdrawnDate.HasValue
        //        && !project.Dates.ProjectClosedDate.HasValue
        //        && !project.Dates.ProjectCancelledDate.HasValue
        //        )
        //    {
        //        ModelState.Remove("project-closed-date");
        //        ModelState.Remove("project-cancelled-date");
        //    }

        //    if (!project.Dates.ProjectWithdrawnDate.HasValue
        //        && !project.Dates.ProjectCancelledDate.HasValue
        //        && !project.Dates.ProjectClosedDate.HasValue)
        //    {
        //        ModelState.Remove("project-closed-date");
        //        ModelState.Remove("project-cancelled-date");
        //        ModelState.Remove("project-withdrawn-date");
        //    }

        //    _errorService.AddErrors(ModelState.Keys, ModelState);

        //    CurrentFreeSchoolName = project.SchoolName;

        //    if (!ModelState.IsValid)
        //    {

        //        return Page();
        //    }

        //    try
        //    {
        //        var request = new UpdateProjectByTaskRequest()
        //        {
        //            Dates = new DatesTask()
        //            {
        //                DateOfEntryIntoPreopening = EntryIntoPreOpening,
        //                ProvisionalOpeningDateAgreedWithTrust = ProvisionalOpeningDateAgreedWithTrust,
        //                ProjectClosedDate = ProjectClosedDate,
        //                ProjectCancelledDate = ProjectCancelledDate,
        //                ProjectWithdrawnDate = ProjectWithdrawnDate,
        //                RealisticYearOfOpening = RealisticYearOfOpening
        //            }
        //        };

        //        await _updateProjectTaskService.Execute(ProjectId, request);

        //        return Redirect(string.Format(RouteConstants.ViewDatesTask, ProjectId));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogErrorMsg(ex);
        //        throw;
        //    }
        //}
    }
}
