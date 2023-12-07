using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Models;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual
{
    public class AgeRangeModel : CreateProjectBaseModel
    {
        [BindProperty(Name = "age-range", BinderType = typeof(NumberRangeModelBinder))]
        [Display(Name = "Age range")]
		[Required(ErrorMessage = "Enter a 'from' and 'to' age range")]
		public string AgeRange { get; set; }

        private readonly ErrorService _errorService;
        private readonly ICreateProjectCache _createProjectCache;

        public AgeRangeModel(ErrorService errorService, ICreateProjectCache createProjectCache)
        {
            _errorService = errorService;
            _createProjectCache = createProjectCache;
        }

        public IActionResult OnGet()
        {
            if (!User.IsInRole(RolesConstants.ProjectRecordCreator))
            {
                return new UnauthorizedResult();
            }

            var project = _createProjectCache.Get();
            AgeRange = project.AgeRange;
            BackLink = GetPreviousPage(CreateProjectPageName.AgeRange, project.Navigation);

            return Page();
        }

        public IActionResult OnPost()
        {
            var project = _createProjectCache.Get();
            BackLink = GetPreviousPage(CreateProjectPageName.AgeRange, project.Navigation);

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            project.AgeRange = AgeRange;
            _createProjectCache.Update(project);

            return Redirect(GetNextPage(CreateProjectPageName.AgeRange));
        }
    }
}
