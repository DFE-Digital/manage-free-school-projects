using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Dfe.ManageFreeSchoolProjects.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class SearchTrustByTrnModel(
        IGetTrustByRefService getTrustByRefService,
        IGetProposalService getProposalService,
        ILogger<SearchTrustByTrnModel> logger,
        ErrorService errorService) : UpdateProposalBaseModel(getProposalService, logger)
    {
        [BindProperty(Name = "trn")]
        [Display(Name = "TRN (trust reference number)")]
        [StringLength(7, ErrorMessage = ValidationConstants.TextValidationMessage)]
        [Required(ErrorMessage = "Enter the TRN")]
        public string TRN { get; set; }

        public async Task<IActionResult> OnGet()
        {
            LogPageEntered();

            SetBackLink();

            if (await LoadProposal() == null)
            {
                return NotFound();
            }

            TRN = Proposal.TrustReferenceNumber;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            LogPageEntered();

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

            await getTrustByRefService.Execute(TRN);

            return Redirect(string.Format(RouteConstants.Proposals_Edit_Confirm_Trust, ProjectId, Rid, TRN));
        }
    }
}
