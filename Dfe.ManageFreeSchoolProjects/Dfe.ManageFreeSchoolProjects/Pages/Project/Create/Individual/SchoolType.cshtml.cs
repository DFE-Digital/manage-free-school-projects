using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Extensions;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SchoolTypeEnum = Dfe.ManageFreeSchoolProjects.API.Contracts.Project.SchoolType;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual
{
    public class SchoolTypeModel : CreateProjectBaseModel
    {
        private static readonly SchoolTypeEnum[] DefaultOptions = new[]
        {
            SchoolTypeEnum.AlternativeProvision,
            SchoolTypeEnum.Mainstream,
            SchoolTypeEnum.Special,
            SchoolTypeEnum.StudioSchool,
            SchoolTypeEnum.UniversityTechnicalCollege,
            SchoolTypeEnum.VoluntaryAided
        };

        private static readonly SchoolTypeEnum[] LocalAuthorityOptions = new[]
        {
            SchoolTypeEnum.AlternativeProvisionPRU,
            SchoolTypeEnum.Mainstream,
            SchoolTypeEnum.Special
        };

        private readonly ErrorService _errorService;

        [BindProperty(Name = "school-type")]
        [Display(Name = "School type")]
        [Required(ErrorMessage = "Select the school type")]
        public string SchoolType { get; set; }

        public IReadOnlyList<SchoolTypeEnum> SchoolTypeOptions { get; private set; }

        public SchoolTypeModel(ErrorService errorService, ICreateProjectCache createProjectCache)
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
            BackLink = GetPreviousPage(CreateProjectPageName.SchoolType, project.TRN);
            SchoolTypeOptions = OptionsFor(project.ProjectType);

            SchoolType = project.SchoolType.ToIntString();

            return Page();
        }

        public IActionResult OnPost()
        {
            var project = CreateProjectCache.Get();
            BackLink = GetPreviousPage(CreateProjectPageName.SchoolType, project.TRN);
            SchoolTypeOptions = OptionsFor(project.ProjectType);

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            if (!project.ReachedCheckYourAnswers)
            {
                project.SchoolType = (SchoolType)Enum.Parse(typeof(SchoolType), SchoolType);
            }
            else
            {
                project.PreviousSchoolType = (SchoolType)Enum.Parse(typeof(SchoolType), SchoolType);
            }

            CreateProjectCache.Update(project);

            return Redirect(GetNextPage(CreateProjectPageName.SchoolType));
        }

        private static IReadOnlyList<SchoolTypeEnum> OptionsFor(ProjectType projectType)
        {
            return projectType == ProjectType.LocalAuthority ? LocalAuthorityOptions : DefaultOptions;
        }
    }
}
