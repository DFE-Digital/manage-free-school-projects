using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class FaithOfOtherReligiousOrganisationModel(
        ICreateProposalCache createProposalCache,
        ILogger<FaithOfOtherReligiousOrganisationModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "faith-of-other-religious-organisation")]
        [Display(Name = "Faith of the other religious organisation")]
        [Required(ErrorMessage = "Select the faith of the other religious organisation")]
        public FaithType? FaithTypeOfOtherReligiousOrganisation { get; set; }

        [BindProperty(Name = "other-faith-of-other-religious-organisation")]
        [Display(Name = "Other faith of the other religious organisation")]
        public string OtherFaithType { get; set; }

        public IActionResult OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            FaithTypeOfOtherReligiousOrganisation = CreateProposalCache.Get().FaithTypeOfOtherReligiousOrganisation;

            return Page();
        }

        public IActionResult OnPost()
        {
            logger.LogMethodEntered();

            SetBackLink();

            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            // update cache
            var cache = CreateProposalCache.Get();

            cache.FaithTypeOfOtherReligiousOrganisation = FaithTypeOfOtherReligiousOrganisation;
            
            if (FaithTypeOfOtherReligiousOrganisation == FaithType.Other && !string.IsNullOrWhiteSpace(OtherFaithType))
            {
                cache.OtherFaithTypeOfOtherReligiousOrganisation = OtherFaithType;
            }

            CreateProposalCache.Update(cache);

            return Redirect(string.Format(RouteConstants.Proposals_Create_Proposed_Faith_Status, ProjectId));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Create_Name_Of_Other_Religious_Organisation, ProjectId);
        }
    }
}
