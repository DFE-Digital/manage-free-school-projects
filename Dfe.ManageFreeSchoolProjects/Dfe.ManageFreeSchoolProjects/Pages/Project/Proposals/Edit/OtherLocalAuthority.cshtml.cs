using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Proposals;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using Dfe.ManageFreeSchoolProjects.Services.Proposal;
using Microsoft.Extensions.Logging;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Edit
{
    public class OtherLocalAuthorityModel(
        IGetProposalService getProposalService,
        IGetLocalAuthoritiesService getLocalAuthoritiesService,
        IUpdateProposalService updateProposalService,
        ILogger<OtherLocalAuthorityModel> logger,
        ErrorService errorService)
        : UpdateProposalLocalAuthorityModel(
            getProposalService, getLocalAuthoritiesService, updateProposalService, logger, errorService)
    {
        protected override string BackLinkRoute => RouteConstants.Proposals_Edit_Other_Local_Authority_Region;

        protected override void PopulateForm()
        {
            LocalAuthority = Proposal.OtherLocalAuthority;
        }

        protected override void ApplyChanges(UpdateProposalRequest request)
        {
            request.OtherLocalAuthorityRegion = Region;
            request.OtherLocalAuthority = LocalAuthority;
        }
    }
}
