using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Models;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.Dates;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.SpecificationPublicationDate;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.DateForConditionToBeMet
{
    public class DateForConditionToBeMetModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly ILogger<DateForConditionToBeMetModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }
        public string CurrentFreeSchoolName { get; set; }

        [BindProperty(Name = "date-for-conditions-to-be-met", BinderType = typeof(DateInputModelBinder))]
        [Display(Name = "Date for conditions to be met")]
        [DateValidation(DateRangeValidationService.DateRange.PastOrFuture)]
        public DateTime? DateForConditionsToBeMet { get; set; }

        public DateForConditionToBeMetModel(
            IGetProjectByTaskService getProjectService,
            IUpdateProjectByTaskService updateProjectTaskService,
            ILogger<DateForConditionToBeMetModel> logger,
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
                var project = await _getProjectService.Execute(ProjectId, TaskName.NewSchoolDateForConditionsToBeMet);
                CurrentFreeSchoolName = project.SchoolName;
                DateForConditionsToBeMet = project.NewSchool.NewSchoolDateForConditionsToBeMet;
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
            }

            return Page();
        }
    }
}
