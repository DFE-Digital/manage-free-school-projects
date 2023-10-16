using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Create
{
    [Authorize(Roles = RolesConstants.ProjectRecordCreator)]
    public class LocalAuthorityModel : PageModel
    {
        [BindProperty(Name = "local-authority")]
        [Display(Name = "local authority")]
        [Required]
        public string LocalAuthority { get; set; }

        private readonly ErrorService _errorService;

        private readonly ICreateProjectCache _createProjectCache;

        public LocalAuthorityModel(ErrorService errorService, ICreateProjectCache createProjectCache)
        {
            _errorService = errorService;
            _createProjectCache = createProjectCache;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            var project = _createProjectCache.Get();
            project.LocalAuthority = (ProjectLocalAuthority)Enum.Parse(typeof(ProjectLocalAuthority), LocalAuthority);
            _createProjectCache.Update(project);

            return Redirect("/project/create/checkyouranswers");
        }
    }
}
