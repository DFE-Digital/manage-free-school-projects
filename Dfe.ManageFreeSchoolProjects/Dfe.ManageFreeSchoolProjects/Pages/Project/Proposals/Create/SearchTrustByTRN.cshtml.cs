using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class SearchTrustByTRNModel(
        ICreateProposalCache createProposalCache,
        IGetTrustByRefService getTrustByRefService,
        ILogger<SearchTrustByTRNModel> logger,
        ErrorService errorService
    ) : CreateProposalBaseModel(createProposalCache)
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(Name = "trn")]
        [Display(Name = "TRN (trust reference number)")]
        [StringLength(7, ErrorMessage = ValidationConstants.TextValidationMessage)]
        [Required(ErrorMessage = "Enter the TRN")]
        public string TRN { get; set; }

        public GetTrustByRefResponse Trust { get; set; }

        public IActionResult OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            TRN = CreateProposalCache.Get().Trust?.TRN;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            logger.LogMethodEntered();

            SetBackLink();

            if (!ModelState.IsValid)
            {
                errorService.AddErrors(ModelState.Keys, ModelState);
                return Page();
            }

            if (!Regex.IsMatch(TRN, "TR\\d\\d\\d\\d\\d", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(5)))
            {
                ModelState.AddModelError("trn", "The TRN must start with the letters TR, followed by at least 5 numbers");
                errorService.AddErrors(ModelState.Keys, ModelState);

                return Page();
            }

            try
            {
                //Attempt to validate trust, will throw an exception when 404 is returned
                var trust = await getTrustByRefService.Execute(TRN);

                // update cache
                var cache = CreateProposalCache.Get();

                cache.Trust = trust.Trust;
                CreateProposalCache.Update(cache);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError("trn", "Trust ID not found. Enter a different ID");
                    errorService.AddErrors(ModelState.Keys, ModelState);

                    return Page();
                }

                throw;
            }

            return Redirect(string.Format(RouteConstants.Proposals_Create_Confirm_Trust, ProjectId));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Create_Proposer, ProjectId);
        }
    }
}
