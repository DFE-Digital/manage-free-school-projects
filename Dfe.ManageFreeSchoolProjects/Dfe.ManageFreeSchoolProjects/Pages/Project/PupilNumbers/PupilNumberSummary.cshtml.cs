using System;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Contacts;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.PupilNumbers;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ProjectStatusType = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ProjectStatus;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.PupilNumbers
{
    public class PupilNumbersSummaryModel : PageModel
    {

        private readonly IGetProjectOverviewService _getProjectOverviewService;

        private readonly ILogger<PupilNumbersSummaryModel> _logger;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        public ProjectStatusType ProjectStatus { get; set; }
        public string SchoolName { get; set; }

        public PupilNumbersOverviewResponse PupilNumbers { get; set; }


        public PupilNumbersSummaryModel(IGetProjectOverviewService projectOverviewService, ILogger<PupilNumbersSummaryModel> logger)
        {
            _getProjectOverviewService = projectOverviewService;
            _logger = logger;
        }
        public async Task<IActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            try
            {
                var projectId = RouteData.Values["projectId"] as string;

                var project = await _getProjectOverviewService.Execute(projectId);
                SchoolName = project.ProjectStatus.CurrentFreeSchoolName;
                ProjectStatus = project.ProjectStatus.ProjectStatus;
                PupilNumbers = project.PupilNumbers;
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
            }

            return Page();
        }
    }
}
