
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels.Project;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project
{
    public class EditProjectModel : PageModel
    {
        [BindProperty]
        public ProjectResponse Project { get; set; }

        private IGetProjectByIdService _getProjectByIdService { get; set; }

        [BindProperty]
        [MaxLength(10)]
        public string ProjectID { get; set; }

        [BindProperty]
        [MaxLength(20)]
        public string SchoolName { get; set; }

        [BindProperty]
        [MaxLength(10)]
        public string ApplicationNumber { get; set; }

        [BindProperty]
        [MaxLength(10)]
        public string ApplicationWave { get; set; }
        private IEditProjectService _editProjectService { get; set; }
        //    public ILogger<EditProjectModel> _logger { get; }

        public EditProjectModel(
            IGetProjectByIdService getProjectByIdService, IEditProjectService editProjectService
            //   ILogger<EditProjectModel> logger
            )
        {
            _getProjectByIdService = getProjectByIdService;
            _editProjectService = editProjectService;
            //_logger = logger;
        }

        public async Task OnGetAsync(string projectId)
        {
            await LoadPage(projectId);
        }

        private async Task<ActionResult> LoadPage(string projectId)
        {
            try
            {
                Project = await _getProjectByIdService.GetProject(projectId);
                return Page();
            }
            catch (Exception ex)
            {
                //_logger.LogError("Case::DetailsPageModel::LoadPage::Exception - {Message}", ex.Message);

                //TempData["Error.Message"] = ErrorOnGetPage;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //_logger.LogMethodEntered();

            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var caseUrn = await _editProjectService.EditProject(ProjectID, SchoolName, ApplicationNumber, ApplicationWave, User.Identity.Name.ToString());

                return Redirect("~/");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "error");

                // TempData["Error.Message"] = ErrorOnPostPage;
            }

            return Page();
        }
    }
}
