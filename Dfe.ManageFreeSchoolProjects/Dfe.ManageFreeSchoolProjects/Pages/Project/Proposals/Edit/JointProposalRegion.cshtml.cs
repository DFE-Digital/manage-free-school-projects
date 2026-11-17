using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.Extensions.Logging;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class JointProposalRegionModel(
        IGetProposalService getProposalService,
        ILogger<JointProposalRegionModel> logger,
        ErrorService errorService)
        : UpdateProposalRegionModel(getProposalService, logger, errorService)
    {
        protected override string GetSelectedRegion(ProposalResponse proposal) =>
            proposal.JointProposalLocalAuthorityRegion;

        protected override string LocalAuthorityRoute => RouteConstants.Proposals_Edit_Joint_Proposal_Local_Authority;
    }
}
