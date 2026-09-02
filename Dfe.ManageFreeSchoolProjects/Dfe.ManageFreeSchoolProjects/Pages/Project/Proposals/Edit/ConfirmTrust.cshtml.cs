using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Enums;
using Dfe.ManageFreeSchoolProjects.Logging;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Dfe.ManageFreeSchoolProjects.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class ConfirmTrustModel(
        IGetTrustByRefService getTrustByRefService,
        IGetProposalService getProposalService,
        IUpdateProposalService updateProposalService,
        ILogger<ConfirmTrustModel> logger,
        ErrorService errorService
    ) : UpdateProposalBaseModel
    {
        [BindProperty(SupportsGet = true, Name = "projectId")]
        public string ProjectId { get; set; }

        [BindProperty(SupportsGet = true, Name = "rid")]
        public string Rid { get; set; }

        [BindProperty(SupportsGet = true, Name = "trn")]
        public string Trn { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select an option to confirm the trust")]
        public YesNoOption? ConfirmOption { get; set; }

        public TrustTask Trust { get; set; }

        public ProposalResponse Proposal { get; set; }


        public async Task<IActionResult> OnGet()
        {
            logger.LogMethodEntered();

            SetBackLink();

            var proposal = await getProposalService.ExecuteSingle(Rid);

            if (proposal == null || proposal.Data == null)
            {
                return NotFound();
            }

            Proposal = proposal.Data;

            var trustResponse = await getTrustByRefService.Execute(Trn);
            Trust = trustResponse.Trust;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            logger.LogMethodEntered();

            SetBackLink();

            if (ConfirmOption == YesNoOption.No)
            {
                return Redirect(string.Format(RouteConstants.Proposals_Edit_SearchTrustByTRN, ProjectId, Rid));
            }

            try
            {
                var proposal = await getProposalService.ExecuteSingle(Rid);
                Proposal = proposal.Data;

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

            return Redirect(string.Format(RouteConstants.Proposals_Details, ProjectId, Rid));
        }

        private void SetBackLink()
        {
            BackLink = string.Format(RouteConstants.Proposals_Edit_SearchTrustByTRN, ProjectId, Rid);
        }
    }
}
