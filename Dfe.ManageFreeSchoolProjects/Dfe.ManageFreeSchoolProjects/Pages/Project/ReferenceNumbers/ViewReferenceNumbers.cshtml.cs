using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ReferenceNumbers;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.ReferenceNumbers
{
    public class ViewReferenceNumbersModel : PageModel
    {

        private readonly IGetProjectReferenceNumbersService _getProjectReferenceNumbersService;
        private readonly ILogger<ViewReferenceNumbersModel> _logger;

        [BindProperty(SupportsGet = true, Name = "projectID")]
        public string ProjectId { get ; set; }

        public string Urn { get; set; }

        public string SchoolName { get; set; }

        public ViewReferenceNumbersModel(
            IGetProjectReferenceNumbersService getProjectReferenceNumbersService,
            ILogger<ViewReferenceNumbersModel> logger)
        {
            _getProjectReferenceNumbersService = getProjectReferenceNumbersService;
            _logger = logger;
        }
        public async Task<IActionResult> OnGet()
        {
            _logger.LogMethodEntered();
            try
            {

                var referenceNumbers = await _getProjectReferenceNumbersService.Execute(ProjectId);

                ProjectId = referenceNumbers.ProjectId;
                Urn = referenceNumbers.Urn;
                SchoolName = referenceNumbers.SchoolName;
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
            }

            return Page();
        }
    }
}
