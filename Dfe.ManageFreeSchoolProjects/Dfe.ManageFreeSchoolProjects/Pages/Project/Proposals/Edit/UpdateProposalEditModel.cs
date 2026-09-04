using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public abstract class UpdateProposalEditModel(
        IGetProposalService getProposalService,
        IUpdateProposalService updateProposalService,
        ILogger logger,
        ErrorService errorService) : UpdateProposalBaseModel(getProposalService, logger)
    {
        protected abstract void PopulateForm();

        protected abstract void ApplyChanges(UpdateProposalRequest request);

        protected virtual Task PrepareView() => Task.CompletedTask;

        public async Task<IActionResult> OnGet()
        {
            LogPageEntered();

            SetBackLink();

            if (await LoadProposal() == null)
            {
                return NotFound();
            }

            await PrepareView();

            PopulateForm();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            LogPageEntered();

            SetBackLink();

            if (!ModelState.IsValid)
            {
                await PrepareView();

                errorService.AddErrors(ModelState.Keys, ModelState);

                return Page();
            }

            await LoadProposal();

            var updateRequest = new UpdateProposalRequest
            {
                Rid = Rid,
                Proposer = Proposal.Proposer
            };

            ApplyChanges(updateRequest);

            await updateProposalService.Execute(updateRequest);

            return Redirect(ProposalDetailsUrl);
        }
    }
}
