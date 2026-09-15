using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageFreeSchoolProjects.Enums;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class ConfirmTrustModel(
        ICreateProposalCache createProposalCache,
        ILogger<ConfirmTrustModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        public TrustTask Trust { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select an option to confirm the trust")]
        public YesNoOption? ConfirmOption { get; set; }

        public IActionResult OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            var cache = CreateProposalCache.Get();

            Trust = cache.Trust;

            if (cache.TrustConfirmed != null)
            {
                ConfirmOption = cache.TrustConfirmed.Value ? YesNoOption.Yes : YesNoOption.No;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            logger.LogMethodEntered();

            SetBackLink();

            Trust = CreateProposalCache.Get().Trust;

            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            if (ConfirmOption == YesNoOption.No)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Create_SearchTrustByTRN, ProjectId));
            }

            // update cache
            var cache = CreateProposalCache.Get();

            cache.TrustConfirmed = true;
            CreateProposalCache.Update(cache);

            return Redirect(string.Format(RouteConstants.Proposals_Create_Proposed_Faith_Status, ProjectId));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Create_SearchTrustByTRN, ProjectId);
        }
    }
}
