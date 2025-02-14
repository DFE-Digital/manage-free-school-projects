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
using System.Linq;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;
using Dfe.ManageFreeSchoolProjects.ViewModels;
using Microsoft.AspNetCore.Http;


namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.FinancePlan
{
    public class EditFinancePlanTaskModel : PageModel
    {
        private readonly IGetProjectByTaskService _getProjectService;
        private readonly IUpdateProjectByTaskService _updateProjectTaskService;
        private readonly IUpdateFinancePlanCache _updateFinancePlanCache;
        private readonly ILogger<EditFinancePlanTaskModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ErrorService _errorService;

        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }


        [BindProperty(Name = "finance-plan-agreed")]
        public bool? FinancePlanAgreed { get; set; }

        [BindProperty(Name = "date-agreed", BinderType = typeof(DateInputModelBinder))]
        [Display(Name = "Date agreed")]

        public DateTime? DateAgreed { get; set; }

        [BindProperty(Name = "plan-saved-in-workplaces-folder")]
        public bool? PlanSavedInWorkplacesFolder { get; set; }

        [BindProperty(Name = "local-authority-agreed-to-pupil-numbers")]
        public string LocalAuthorityAgreedToPupilNumbers { get; set; }

        [BindProperty(Name = "comments")]
        [Display(Name = "Comments")]
        [ValidText(999)]
        public string Comments { get; set; }

        [BindProperty(Name = "trust-opt-into-rpa")]
        public string TrustOptIntoRpa { get; set; }

        [BindProperty(Name = "rpa-start-date", BinderType = typeof(DateInputModelBinder))]
        [DisplayName("RPA start date")]
        public DateTime? RpaStartDate { get; set; }

        [BindProperty(Name = "rpa-cover-type")]
        [DisplayName("Type of RPA cover")]
        [ValidText(100)]
        public string RpaCoverType { get; set; }

        [BindProperty]
        public string SchoolName { get; set; }

        public EditFinancePlanTaskModel(
            IGetProjectByTaskService getProjectService,
            IUpdateProjectByTaskService updateProjectTaskService,
            IUpdateFinancePlanCache updateFinancePlanCache,
            ILogger<EditFinancePlanTaskModel> logger,
            IHttpContextAccessor httpContextAccessor,
            ErrorService errorService)
        {
            _getProjectService = getProjectService;
            _updateProjectTaskService = updateProjectTaskService;
            _updateFinancePlanCache = updateFinancePlanCache;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
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
            var trustOptIntoRpa = ConvertYesNo(TrustOptIntoRpa);

            if (trustOptIntoRpa != YesNo.Yes)
            {
                // Ignore any errors for the RPA fields if the trust is not opting into RPA
                var errorKeys = ModelState.Keys.Where(k => k.StartsWith("rpa-start-date") || k == "rpa-cover-type").ToList();

                errorKeys.ForEach(k => ModelState.Remove(k));
            }

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            var project = await _getProjectService.Execute(ProjectId, TaskName.FinancePlan);

            var updateTaskRequest = new UpdateProjectByTaskRequest()
            {
                FinancePlan = new()
                {
                    FinancePlanAgreed = FinancePlanAgreed == true ? YesNo.Yes : YesNo.No,
                    DateAgreed = DateAgreed,
                    PlanSavedInWorkplacesFolder = PlanSavedInWorkplacesFolder == true ? YesNo.Yes : YesNo.No,
                    LocalAuthorityAgreedPupilNumbers = ConvertYesNoNotApplicable(LocalAuthorityAgreedToPupilNumbers),
                    TrustWillOptIntoRpa = ConvertYesNo(TrustOptIntoRpa),
                    RpaStartDate =  null,
                    RpaCoverType = null,

                    UnderwrittenPlacesPrimaryYear1 = project.FinancePlan.UnderwrittenPlacesPrimaryYear1,
                    UnderwrittenPlacesPrimaryYear2 = project.FinancePlan.UnderwrittenPlacesPrimaryYear2,
                    UnderwrittenPlacesPrimaryYear3 = project.FinancePlan.UnderwrittenPlacesPrimaryYear3,
                    UnderwrittenPlacesPrimaryYear4 = project.FinancePlan.UnderwrittenPlacesPrimaryYear4,
                    UnderwrittenPlacesPrimaryYear5 = project.FinancePlan.UnderwrittenPlacesPrimaryYear5,
                    UnderwrittenPlacesPrimaryYear6 = project.FinancePlan.UnderwrittenPlacesPrimaryYear6,
                    UnderwrittenPlacesPrimaryYear7 = project.FinancePlan.UnderwrittenPlacesPrimaryYear7,
                    UnderwrittenPlacesSecondaryYear1 = project.FinancePlan.UnderwrittenPlacesSecondaryYear1,
                    UnderwrittenPlacesSecondaryYear2 = project.FinancePlan.UnderwrittenPlacesSecondaryYear2,
                    UnderwrittenPlacesSecondaryYear3 = project.FinancePlan.UnderwrittenPlacesSecondaryYear3,
                    UnderwrittenPlacesSecondaryYear4 = project.FinancePlan.UnderwrittenPlacesSecondaryYear4,
                    UnderwrittenPlacesSecondaryYear5 = project.FinancePlan.UnderwrittenPlacesSecondaryYear5,
                    UnderwrittenPlacesSixteenToNineteenYear1 = project.FinancePlan.UnderwrittenPlacesSixteenToNineteenYear1,
                    UnderwrittenPlacesSixteenToNineteenYear2 = project.FinancePlan.UnderwrittenPlacesSixteenToNineteenYear2,
                    UnderwrittenPlacesSixteenToNineteenYear3 = project.FinancePlan.UnderwrittenPlacesSixteenToNineteenYear3,
                    ConfirmationFromLocalAuthoritySavedInWorkplacesFolder = project.FinancePlan.ConfirmationFromLocalAuthoritySavedInWorkplacesFolder,
                    CommentsAboutUnderwrittenPlaces = project.FinancePlan.CommentsAboutUnderwrittenPlaces,
                }
            };

            if (trustOptIntoRpa == YesNo.Yes)
            {
                updateTaskRequest.FinancePlan.RpaStartDate = RpaStartDate;
                updateTaskRequest.FinancePlan.RpaCoverType = RpaCoverType;
            }

            if (ConvertYesNoNotApplicable(LocalAuthorityAgreedToPupilNumbers) == YesNoNotApplicable.Yes)
            {
                var existingCacheItem = _updateFinancePlanCache.Get();

                existingCacheItem.FinancePlan = updateTaskRequest.FinancePlan;

                _updateFinancePlanCache.Update(existingCacheItem);

                return Redirect(string.Format(RouteConstants.EditUnderwrittenPlaces, ProjectId));
            }

        await _updateProjectTaskService.Execute(ProjectId, updateTaskRequest);

            return Redirect(string.Format(RouteConstants.ViewFinancePlanTask, ProjectId));
        }

        private async Task LoadProject()
        {
            var project = await _getProjectService.Execute(ProjectId, TaskName.FinancePlan);

            var referrerQuery = _httpContextAccessor.HttpContext.Request.Query["referrer"];

            var isReferred = Enum.TryParse(referrerQuery, out Referrer referrer);


            if (isReferred && referrer == Referrer.EditUnderwrittenPlaces)
            {
                var existingCacheItem = _updateFinancePlanCache.Get();

                FinancePlanAgreed = existingCacheItem.FinancePlan.FinancePlanAgreed == YesNo.Yes;
                DateAgreed = existingCacheItem.FinancePlan.DateAgreed;
                PlanSavedInWorkplacesFolder = existingCacheItem.FinancePlan.PlanSavedInWorkplacesFolder == YesNo.Yes;
                LocalAuthorityAgreedToPupilNumbers = existingCacheItem.FinancePlan.LocalAuthorityAgreedPupilNumbers?.ToString();
                TrustOptIntoRpa = existingCacheItem.FinancePlan.TrustWillOptIntoRpa?.ToString();
                RpaStartDate = existingCacheItem.FinancePlan.RpaStartDate;
                RpaCoverType = existingCacheItem.FinancePlan.RpaCoverType;

            }

            else
            {
                FinancePlanAgreed = project.FinancePlan.FinancePlanAgreed == YesNo.Yes;
                DateAgreed = project.FinancePlan.DateAgreed;
                PlanSavedInWorkplacesFolder = project.FinancePlan.PlanSavedInWorkplacesFolder == YesNo.Yes;
                LocalAuthorityAgreedToPupilNumbers = project.FinancePlan.LocalAuthorityAgreedPupilNumbers?.ToString();
                TrustOptIntoRpa = project.FinancePlan.TrustWillOptIntoRpa?.ToString();
                RpaStartDate = project.FinancePlan.RpaStartDate;
                RpaCoverType = project.FinancePlan.RpaCoverType;
            }

            SchoolName = project.SchoolName;


        }

        private static YesNoNotApplicable? ConvertYesNoNotApplicable(string value)
        {
            return Enum.TryParse<YesNoNotApplicable>(value, true, out var result) ? result : null;
        }

        public static YesNo? ConvertYesNo(string value)
        {
            return Enum.TryParse<YesNo>(value, true, out var result) ? result : null;
        }
    }
}
