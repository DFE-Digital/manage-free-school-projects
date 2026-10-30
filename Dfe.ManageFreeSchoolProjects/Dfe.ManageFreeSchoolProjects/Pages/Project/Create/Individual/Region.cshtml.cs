using System;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Create
{
    public class RegionModel : CreateProjectBaseModel
    {
        [BindProperty(Name = "region")]
        [Display(Name = "region")]
        [Required(ErrorMessage = "Select the region")]
        public string Region { get; set; }
        
        private readonly ErrorService _errorService;

        public RegionModel(ErrorService errorService, ICreateProjectCache createProjectCache)
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

            if (project.Region != 0)
                Region = CreateProjectCache.Get().Region.ToString();

            BackLink = GetPreviousPage(CreateProjectPageName.Region); 
            return Page();
        }

        public IActionResult OnPost()
        {
            var project = CreateProjectCache.Get();
            BackLink = GetPreviousPage(CreateProjectPageName.Region);

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            project.Region = (ProjectRegion)Enum.Parse(typeof(ProjectRegion), Region);

            CreateProjectCache.Update(project);

            return Redirect("/project/create/localauthority");
        }
    }
}