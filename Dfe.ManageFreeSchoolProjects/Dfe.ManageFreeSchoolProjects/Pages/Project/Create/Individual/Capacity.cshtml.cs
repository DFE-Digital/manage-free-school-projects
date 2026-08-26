using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Models;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual;
using Dfe.ManageFreeSchoolProjects.Utils;
using static Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ClassType;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Create
{
    public class CapacityModel : CreateProjectBaseModel
    {

        [BindProperty(Name = "nursery-capacity")]
        [Display(Name = "Nursery capacity")]
        [ValidNumber(0, 9999)]
        public string NurseryCapacity { get; set; }

        [BindProperty(Name = "yr-y6-capacity")]
        [Display(Name = "Reception to year 6 capacity")]
        [Required(ErrorMessage = "Enter the reception to year 6 capacity")]
        [ValidNumber(0,9999)]
        public string YRY6Capacity { get; set; }

        [BindProperty(Name = "y7-y11-capacity")]
        [Display(Name = "Year 7 to year 11 capacity")]
        [Required(ErrorMessage = "Enter the year 7 to year 11 capacity")]
        [ValidNumber(0,9999)]
        public string Y7Y11Capacity { get; set; }

        [BindProperty(Name = "y12-y14-capacity")]
        [Display(Name = "Year 12 to year 14 capacity")]
        [Required(ErrorMessage = "Enter the year 12 to year 14 capacity")]
        [ValidNumber(0,9999)]
        public string Y12Y14Capacity { get; set; }

        [BindProperty(Name = "ap-resources-provision")]
        [Display(Name = "AP resources provision")]
        [ValidNumber(0, 9999)]
        public string APResourcesProvision { get; set; }

        [BindProperty(Name = "sen-resourced-provision-sen-unit")]
        [Display(Name = "SEN resourced provision / SEN unit")]
        [ValidNumber(0, 9999)]
        public string SENResourcedProvisionSENUnit { get; set; }

        public Nursery HasNursery { get; set; }

        public bool IsLocalAuthority { get; private set; }

        public bool IsMainStream { get; private set; }

        // The AP / SEN provision inputs are only rendered for a new school mainstream project,
        // so they can only be required when they are actually on the page.
        private bool ShowProvisionCapacities => IsLocalAuthority && IsMainStream;

        private readonly ErrorService _errorService;

        public CapacityModel(ErrorService errorService, ICreateProjectCache createProjectCache)
            : base(createProjectCache)
        {
            _errorService = errorService;
        }

        public IActionResult OnGet()
        {
            if (!User.IsInRole(RolesConstants.ProjectRecordCreator))
            {
                return new UnauthorizedResult();
            }

            var project = CreateProjectCache.Get();
            NurseryCapacity = project.NurseryCapacity.ToString();
            YRY6Capacity = project.YRY6Capacity.ToString();
            Y7Y11Capacity = project.Y7Y11Capacity.ToString();
            Y12Y14Capacity = project.Y12Y14Capacity.ToString();
            APResourcesProvision = project.APResourcesProvision.ToString();
            SENResourcedProvisionSENUnit = project.SENResourcedProvisionSENUnit.ToString();

            HasNursery = project.Nursery;
            IsLocalAuthority = project.ProjectType == ProjectType.NewSchool;
            IsMainStream = project.SchoolType == SchoolType.Mainstream;

            BackLink = GetPreviousPage(CreateProjectPageName.Capacity);

            return Page();
        }

        public IActionResult OnPost()
        {
            var project = CreateProjectCache.Get();

            BackLink = GetPreviousPage(CreateProjectPageName.Capacity);

            HasNursery = project.Nursery;
            IsLocalAuthority = project.ProjectType == ProjectType.NewSchool;
            IsMainStream = project.SchoolType == SchoolType.Mainstream;

            if (HasNursery == Nursery.Yes && string.IsNullOrEmpty(NurseryCapacity))
            {
                ModelState.AddModelError("nursery-capacity", "Enter the nursery capacity");
            }

            if (ShowProvisionCapacities)
            {
                if (string.IsNullOrEmpty(APResourcesProvision))
                {
                    ModelState.AddModelError("ap-resources-provision", "Enter the AP resources provision");
                }

                if (string.IsNullOrEmpty(SENResourcedProvisionSENUnit))
                {
                    ModelState.AddModelError("sen-resourced-provision-sen-unit", "Enter the SEN resourced provision / SEN unit");
                }
            }

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            project.NurseryCapacity = !string.IsNullOrEmpty(NurseryCapacity) ? int.Parse(NurseryCapacity) : 0;
            project.YRY6Capacity = int.Parse(YRY6Capacity);
            project.Y7Y11Capacity = int.Parse(Y7Y11Capacity);
            project.Y12Y14Capacity = int.Parse(Y12Y14Capacity);
            project.APResourcesProvision = ShowProvisionCapacities ? int.Parse(APResourcesProvision) : 0;
            project.SENResourcedProvisionSENUnit = ShowProvisionCapacities ? int.Parse(SENResourcedProvisionSENUnit) : 0;


            CreateProjectCache.Update(project);

            return Redirect(GetNextPage(CreateProjectPageName.Capacity));
        }
    }
}
