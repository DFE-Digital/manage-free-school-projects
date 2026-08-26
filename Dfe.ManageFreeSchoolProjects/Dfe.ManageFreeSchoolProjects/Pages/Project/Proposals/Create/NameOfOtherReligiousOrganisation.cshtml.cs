using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class NameOfOtherReligiousOrganisationModel(
        ICreateProposalCache createProposalCache,
        ILogger<NameOfOtherReligiousOrganisationModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "name-of-other-religious-organisation")]
        [Display(Name = "Name of the other religious organisation")]
        [Required(ErrorMessage = "Enter the name of the other religious organisation")]
        public string NameOfOtherReligiousOrganisation { get; set; }

        public IActionResult OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            NameOfOtherReligiousOrganisation = CreateProposalCache.Get().NameOfOtherReligiousOrganisation;

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

            cache.NameOfOtherReligiousOrganisation = NameOfOtherReligiousOrganisation;
            CreateProposalCache.Update(cache);

            return Redirect(string.Format(RouteConstants.Proposals_Create_Faith_Of_Other_Religious_Organisation, ProjectId));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Create_Proposer, ProjectId);
        }
    }
}
