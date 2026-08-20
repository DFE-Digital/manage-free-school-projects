using System.ComponentModel.DataAnnotations;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual
{
    public class SchoolModel : CreateProjectBaseModel
    {
        [BindProperty(Name = "school")]
        [Display(Name = "School name")]
        [ValidText(100)]
        public string School { get; set; }

        public string SchoolNameQuestion { get; private set; }

        private string _schoolNameRequiredError;

        private readonly ErrorService _errorService;

        public SchoolModel(ErrorService errorService, ICreateProjectCache createProjectCache)
            :base(createProjectCache)
        {
            _errorService = errorService;
        }

        public IActionResult OnGet()
        {
            if (!IsUserAuthorised())
            {
                return new UnauthorizedResult();
            }

            var project = CreateProjectCache.Get();
            School = project.SchoolName;
            SetContentFor(project.ProjectType);
            BackLink = GetPreviousPage(CreateProjectPageName.SchoolName);

            return Page();
        }

        public IActionResult OnPost()
        {
            var project = CreateProjectCache.Get();
            SetContentFor(project.ProjectType);
            BackLink = GetPreviousPage(CreateProjectPageName.SchoolName);

            if (string.IsNullOrWhiteSpace(School))
            {
                ModelState.AddModelError("school", _schoolNameRequiredError);
            }

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            project.SchoolName = School;
            CreateProjectCache.Update(project);

            return Redirect(GetNextPage(CreateProjectPageName.SchoolName));
        }

        private void SetContentFor(ProjectType projectType)
        {
            var isLocalAuthority = projectType == ProjectType.LocalAuthority;

            SchoolNameQuestion = isLocalAuthority
                ? "What is the current working name of the new school?"
                : "What is the current free school name?";

            _schoolNameRequiredError = isLocalAuthority
                ? "Enter the new school name"
                : "Enter the current free school name";
        }
    }
}
