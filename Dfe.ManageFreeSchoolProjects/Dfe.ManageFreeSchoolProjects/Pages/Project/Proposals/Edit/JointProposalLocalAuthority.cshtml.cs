using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.Extensions.Logging;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class JointProposalLocalAuthorityModel(
        IGetProposalService getProposalService,
        IGetLocalAuthoritiesService getLocalAuthoritiesService,
        IUpdateProposalService updateProposalService,
        ILogger<JointProposalLocalAuthorityModel> logger,
        ErrorService errorService)
        : UpdateProposalLocalAuthorityModel(
            getProposalService, getLocalAuthoritiesService, updateProposalService, logger, errorService)
    {
        protected override string BackLinkRoute => RouteConstants.Proposals_Edit_Joint_Proposal_Region;

        protected override void PopulateForm()
        {
            LocalAuthority = Proposal.JointProposalLocalAuthority;
        }

        protected override void ApplyChanges(UpdateProposalRequest request)
        {
            request.JointProposalLocalAuthorityRegion = Region;
            request.JointProposalLocalAuthority = LocalAuthority;
        }
    }
}
