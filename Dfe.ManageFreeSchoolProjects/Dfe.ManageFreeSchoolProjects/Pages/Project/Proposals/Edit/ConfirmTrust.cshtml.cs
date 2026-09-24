using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Enums;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Dfe.ManageFreeSchoolProjects.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class ConfirmTrustModel(
        IGetTrustByRefService getTrustByRefService,
        IGetProposalService getProposalService,
        IUpdateProposalService updateProposalService,
        ILogger<ConfirmTrustModel> logger,
        ErrorService errorService) : UpdateProposalBaseModel(getProposalService, logger)
    {
        [BindProperty(SupportsGet = true, Name = "trn")]
        public string Trn { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select an option to confirm the trust")]
        public YesNoOption? ConfirmOption { get; set; }

        public TrustTask Trust { get; set; }

        protected override string BackLinkRoute => RouteConstants.Proposals_Edit_SearchTrustByTRN;

        public async Task<IActionResult> OnGet()
        {
            LogPageEntered();

            SetBackLink();

            if (await LoadProposal() == null)
            {
                return NotFound();
            }

            var trustResponse = await getTrustByRefService.Execute(Trn);
            Trust = trustResponse.Trust;

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

            if (ConfirmOption == YesNoOption.No)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Edit_SearchTrustByTRN, ProjectId, Rid));
            }

            try
            {
                await LoadProposal();

                var trustResponse = await getTrustByRefService.Execute(Trn);

                var updateRequest = new UpdateProposalRequest
                {
                    Rid = Rid,
                    Proposer = Proposal.Proposer,
                    TrustReferenceNumber = Trn,
                    TrustName = trustResponse.Trust.TrustName,
                    TrustType = trustResponse.Trust.TrustType,
                };

                await updateProposalService.Execute(updateRequest);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelState.AddModelError("trn", "Trust ID not found. Enter a different ID");
                errorService.AddErrors(ModelState.Keys, ModelState);

                return Page();
            }

            return Redirect(ProposalDetailsUrl);
        }
    }
}
