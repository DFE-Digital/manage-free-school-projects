using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Logging;
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
        IUpdateProposalService updateProposalService,
        ILogger<SearchTrustByTrnModel> logger,
        ErrorService errorService
    ) : UpdateProposalBaseModel
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(SupportsGet = true, Name = "rid")]
        public string Rid { get; set; }

        [BindProperty(Name = "trn")]
        [Display(Name = "TRN (trust reference number)")]
        [StringLength(7, ErrorMessage = ValidationConstants.TextValidationMessage)]
        [Required(ErrorMessage = "Enter the TRN")]
        public string TRN { get; set; }

        public ProposalResponse Proposal { get; set; }

        public async Task<IActionResult> OnGet()
        {
            logger.LogMethodEntered();

            var proposal = await getProposalService.ExecuteSingle(Rid);

            if (proposal == null || proposal.Data == null)
            {
                return NotFound();
            }

            Proposal = proposal.Data;

            TRN = Proposal.TrustReferenceNumber;

            SetBackLink();

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

            await getTrustByRefService.Execute(TRN);

            return Redirect(string.Format(RouteConstants.Proposals_Edit_Confirm_Trust, ProjectId, Rid, TRN));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Details, ProjectId, Rid);
        }
    }
}
