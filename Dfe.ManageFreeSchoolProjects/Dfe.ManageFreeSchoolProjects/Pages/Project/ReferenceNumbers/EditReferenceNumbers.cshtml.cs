using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ReferenceNumbers;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.ReferenceNumbers
{
    public class EditReferenceNumbersModel : PageModel
    {

        private readonly IGetProjectReferenceNumbersService _getProjectReferenceNumbersService;
        private readonly IUpdateProjectReferenceNumbersService _updateProjectReferenceNumbersService;
        private readonly ILogger<EditReferenceNumbersModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectID")]
        public string ProjectIdToUpdate { get; set; }

        [BindProperty(Name = "project-id")]
        [Required(ErrorMessage = "Enter the project ID")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "urn")]
        public string Urn { get; set; }

        public string SchoolName { get; set; }

        public GetProjectReferenceNumbersResponse ReferenceNumbers { get; set; }

        public EditReferenceNumbersModel(
            IGetProjectReferenceNumbersService getProjectReferenceNumbersService,
            IUpdateProjectReferenceNumbersService updateProjectReferenceNumbersService,
            ILogger<EditReferenceNumbersModel> logger,
            ErrorService errorService)
        {
            _getProjectReferenceNumbersService = getProjectReferenceNumbersService;
            _updateProjectReferenceNumbersService = updateProjectReferenceNumbersService;
            _logger = logger;
            _errorService = errorService;
        }
        public async Task<IActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            try
            {

                var referenceNumbers = await _getProjectReferenceNumbersService.Execute(ProjectIdToUpdate);

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

        public async Task<ActionResult> OnPost()
        {
            var referenceNumbers = await _getProjectReferenceNumbersService.Execute(ProjectIdToUpdate);

            _errorService.AddErrors(ModelState.Keys, ModelState);

            SchoolName = referenceNumbers.SchoolName;

            if (!ModelState.IsValid)
            {

                return Page();
            }

            try
            {
                var request = new UpdateProjectReferenceNumbersRequest()
                {
                    ProjectId = ProjectId,
                    Urn = Urn
                };

                await _updateProjectReferenceNumbersService.Execute(ProjectIdToUpdate, request);

                return Redirect(string.Format(RouteConstants.ViewReferenceNumbers, ProjectId));
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
                throw;
            }
        }
    }
}
