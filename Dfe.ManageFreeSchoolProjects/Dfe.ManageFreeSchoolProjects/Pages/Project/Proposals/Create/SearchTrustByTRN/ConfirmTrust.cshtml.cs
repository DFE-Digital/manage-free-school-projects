using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals.Enums;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Risk;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.LocalAuthority.Conditions;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Trust;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.Enums;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create.SearchTrustByTRN
{
    public class ConfirmTrustModel(
        ICreateProposalCache createProposalCache,
        ILogger<SearchTrustByTRNModel> logger,
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

            Trust = CreateProposalCache.Get().Trust;

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
                return Redirect(string.Format(RouteConstants.Proposals_SearchTrustByTRN, ProjectId));
            }

            // update cache
            var cache = CreateProposalCache.Get();

            cache.TrustConfirmed = true;
            CreateProposalCache.Update(cache);

            return Page();
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_SearchTrustByTRN, ProjectId);
        }
    }
}
