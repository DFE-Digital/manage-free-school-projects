using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.Extensions.Logging;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class OtherLocalAuthorityRegionModel(
        IGetProposalService getProposalService,
        ILogger<OtherLocalAuthorityRegionModel> logger,
        ErrorService errorService)
        : UpdateProposalRegionModel(getProposalService, logger, errorService)
    {
        protected override string GetSelectedRegion(ProposalResponse proposal) => proposal.OtherLocalAuthorityRegion;

        protected override string LocalAuthorityRoute => RouteConstants.Proposals_Edit_Other_Local_Authority;
    }
}
