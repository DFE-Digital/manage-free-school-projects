using Dfe.ManageFreeSchoolProjects.API.Contracts.Common;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Models;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.Validators;
using Dfe.ManageFreeSchoolProjects.Constants;
using System.ComponentModel;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;
using Dfe.ManageFreeSchoolProjects.Extensions;


namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.FinancePlan
{
    public class EditUnderwrittenPlacesModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IGetProjectOverviewService _getProjectOverviewService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly IUpdateFinancePlanCache _updateFinancePlanCache;
        private readonly ILogger<EditUnderwrittenPlacesModel> _logger;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        public FinancePlanTask FinancePlan { get; set; }

        [BindProperty(Name = "primary-year-1-places")]
        [DisplayName("Primary places in first year of opening")]
        [ValidNumber(0,9999)]
        public string PrimaryYear1Places { get; set; }

        [BindProperty(Name = "primary-year-2-places")]
        [DisplayName("Primary places in second year of opening")]
        [ValidNumber(0, 9999)]
        public string PrimaryYear2Places { get; set; }

        [BindProperty(Name = "primary-year-3-places")]
        [DisplayName("Primary places in third year of opening")]
        [ValidNumber(0, 9999)]
        public string PrimaryYear3Places { get; set; }

        [BindProperty(Name = "primary-year-4-places")]
        [DisplayName("Primary places in fourth year of opening")]
        [ValidNumber(0, 9999)]
        public string PrimaryYear4Places { get; set; }

        [BindProperty(Name = "primary-year-5-places")]
        [DisplayName("Primary places in fifth year of opening")]
        [ValidNumber(0, 9999)]
        public string PrimaryYear5Places { get; set; }

        [BindProperty(Name = "primary-year-6-places")]
        [DisplayName("Primary places in sixth year of opening")]
        [ValidNumber(0, 9999)]
        public string PrimaryYear6Places { get; set; }

        [BindProperty(Name = "primary-year-7-places")]
        [DisplayName("Primary places in seventh year of opening")]
        [ValidNumber(0, 9999)]
        public string PrimaryYear7Places { get; set; }

        [BindProperty(Name = "secondary-year-1-places")]
        [DisplayName("Secondary places in first year of opening")]
        [ValidNumber(0, 9999)]
        public string SecondaryYear1Places { get; set; }

        [BindProperty(Name = "secondary-year-2-places")]
        [DisplayName("Secondary places in second year of opening")]
        [ValidNumber(0, 9999)]
        public string SecondaryYear2Places { get; set; }

        [BindProperty(Name = "secondary-year-3-places")]
        [DisplayName("Secondary places in third year of opening")]
        [ValidNumber(0, 9999)]
        public string SecondaryYear3Places { get; set; }

        [BindProperty(Name = "secondary-year-4-places")]
        [DisplayName("Secondary places in fourth year of opening")]
        [ValidNumber(0, 9999)]
        public string SecondaryYear4Places { get; set; }

        [BindProperty(Name = "secondary-year-5-places")]
        [DisplayName("Secondary places in fifth year of opening")]
        [ValidNumber(0, 9999)]
        public string SecondaryYear5Places { get; set; }

        [BindProperty(Name = "sixteen-to-nineteen-year-1-places")]
        [DisplayName("16 to 19 places in first year of opening")]
        [ValidNumber(0, 9999)]
        public string SixteenToNineteenYear1Places { get; set; }

        [BindProperty(Name = "sixteen-to-nineteen-year-2-places")]
        [DisplayName("16 to 19 places in second year of opening")]
        [ValidNumber(0, 9999)]
        public string SixteenToNineteenYear2Places { get; set; }

        [BindProperty(Name = "sixteen-to-nineteen-year-3-places")]
        [DisplayName("16 to 19 places in third year of opening")]
        [ValidNumber(0, 9999)]
        public string SixteenToNineteenYear3Places { get; set; }


        [BindProperty(Name = "confirmation-from-local-authority-saved-in-workplaces-folder")]
        [DisplayName("Confirmation from local authority saved in workplaces folder")]
        public bool? ConfirmationFromLocalAuthoritySavedInWorkplacesFolder { get; set; }

        [BindProperty(Name = "comments-about-underwritten-places")]
        [Display(Name = "Add comments about underwritten places")]
        [ValidText(500)]
        public string CommentsAboutUnderwrittenPlaces { get; set; }

        public SchoolPhase SchoolPhase { get; set; }

        [BindProperty]
        public string SchoolName { get; set; }

        public EditUnderwrittenPlacesModel(
            IGetProjectByTaskService getProjectService,
            IGetProjectOverviewService getProjectOverviewService,
            IUpdateProjectByTaskService updateProjectTaskService,
            IUpdateFinancePlanCache updateFinancePlanCache,
            ILogger<EditUnderwrittenPlacesModel> logger,
            ErrorService errorService)
        {
            _getProjectService = getProjectService;
            _getProjectOverviewService = getProjectOverviewService;
            _updateProjectTaskService = updateProjectTaskService;
            _updateFinancePlanCache = updateFinancePlanCache;
            _logger = logger;
            _errorService = errorService;
        }

        public async Task<ActionResult> OnGet()
        {
            _logger.LogMethodEntered();

            await LoadProject();
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            var existingCacheItem = _updateFinancePlanCache.Get();

            var updateTaskRequest = new UpdateProjectByTaskRequest()
            {
                FinancePlan = new()
                {
                    FinancePlanAgreed = existingCacheItem.FinancePlan.FinancePlanAgreed,
                    DateAgreed = existingCacheItem.FinancePlan.DateAgreed,
                    PlanSavedInWorkplacesFolder = existingCacheItem.FinancePlan.PlanSavedInWorkplacesFolder,
                    LocalAuthorityAgreedPupilNumbers = existingCacheItem.FinancePlan.LocalAuthorityAgreedPupilNumbers,
                    TrustWillOptIntoRpa = existingCacheItem.FinancePlan.TrustWillOptIntoRpa,
                    RpaStartDate = existingCacheItem.FinancePlan.RpaStartDate,
                    RpaCoverType = existingCacheItem.FinancePlan.RpaCoverType,

                    UnderwrittenPlacesPrimaryYear1 = PrimaryYear1Places.ToInt(),
                    UnderwrittenPlacesPrimaryYear2 = PrimaryYear2Places.ToInt(),
                    UnderwrittenPlacesPrimaryYear3 = PrimaryYear3Places.ToInt(),
                    UnderwrittenPlacesPrimaryYear4 = PrimaryYear4Places.ToInt(),
                    UnderwrittenPlacesPrimaryYear5 = PrimaryYear5Places.ToInt(),
                    UnderwrittenPlacesPrimaryYear6 = PrimaryYear6Places.ToInt(),
                    UnderwrittenPlacesPrimaryYear7 = PrimaryYear7Places.ToInt(),

                    UnderwrittenPlacesSecondaryYear1 = SecondaryYear1Places.ToInt(),
                    UnderwrittenPlacesSecondaryYear2 = SecondaryYear2Places.ToInt(),
                    UnderwrittenPlacesSecondaryYear3 = SecondaryYear3Places.ToInt(),
                    UnderwrittenPlacesSecondaryYear4 = SecondaryYear4Places.ToInt(),
                    UnderwrittenPlacesSecondaryYear5 = SecondaryYear5Places.ToInt(),

                    UnderwrittenPlacesSixteenToNineteenYear1 = SixteenToNineteenYear1Places.ToInt(),
                    UnderwrittenPlacesSixteenToNineteenYear2 = SixteenToNineteenYear2Places.ToInt(),
                    UnderwrittenPlacesSixteenToNineteenYear3 = SixteenToNineteenYear3Places.ToInt(),

                    ConfirmationFromLocalAuthoritySavedInWorkplacesFolder = ConfirmationFromLocalAuthoritySavedInWorkplacesFolder,
                    CommentsAboutUnderwrittenPlaces = CommentsAboutUnderwrittenPlaces,
                }
            };

            await _updateProjectTaskService.Execute(ProjectId, updateTaskRequest);

            return Redirect(string.Format(RouteConstants.ViewFinancePlanTask, ProjectId));
        }

        private async Task LoadProject()
        {
            try
            {
                var project = await _getProjectService.Execute(ProjectId, TaskName.FinancePlan);
                var projectId = RouteData.Values["projectId"] as string;
                var projectOverview = await _getProjectOverviewService.Execute(projectId);

                SchoolPhase = projectOverview.SchoolDetails.SchoolPhase;
                SchoolName = projectOverview.ProjectStatus.CurrentFreeSchoolName;

                PrimaryYear1Places = project.FinancePlan.UnderwrittenPlacesPrimaryYear1.ToString();
                PrimaryYear2Places = project.FinancePlan.UnderwrittenPlacesPrimaryYear2.ToString();
                PrimaryYear3Places = project.FinancePlan.UnderwrittenPlacesPrimaryYear3.ToString();
                PrimaryYear4Places = project.FinancePlan.UnderwrittenPlacesPrimaryYear4.ToString();
                PrimaryYear5Places = project.FinancePlan.UnderwrittenPlacesPrimaryYear5.ToString();
                PrimaryYear6Places = project.FinancePlan.UnderwrittenPlacesPrimaryYear6.ToString();
                PrimaryYear7Places = project.FinancePlan.UnderwrittenPlacesPrimaryYear7.ToString();

                SecondaryYear1Places = project.FinancePlan.UnderwrittenPlacesSecondaryYear1.ToString();
                SecondaryYear2Places = project.FinancePlan.UnderwrittenPlacesSecondaryYear2.ToString();
                SecondaryYear3Places = project.FinancePlan.UnderwrittenPlacesSecondaryYear3.ToString();
                SecondaryYear4Places = project.FinancePlan.UnderwrittenPlacesSecondaryYear4.ToString();
                SecondaryYear5Places = project.FinancePlan.UnderwrittenPlacesSecondaryYear5.ToString();

                SixteenToNineteenYear1Places = project.FinancePlan.UnderwrittenPlacesSixteenToNineteenYear1.ToString();
                SixteenToNineteenYear2Places = project.FinancePlan.UnderwrittenPlacesSixteenToNineteenYear2.ToString();
                SixteenToNineteenYear3Places = project.FinancePlan.UnderwrittenPlacesSixteenToNineteenYear3.ToString();

                ConfirmationFromLocalAuthoritySavedInWorkplacesFolder = project.FinancePlan.ConfirmationFromLocalAuthoritySavedInWorkplacesFolder;
                CommentsAboutUnderwrittenPlaces = project.FinancePlan.CommentsAboutUnderwrittenPlaces;
            }
            catch (Exception ex)
            {
                _logger.LogErrorMsg(ex);
            }
        }

        public static YesNo? ConvertYesNo(string value)
        {
            return Enum.TryParse<YesNo>(value, true, out var result) ? result : null;
        }
    }
}
