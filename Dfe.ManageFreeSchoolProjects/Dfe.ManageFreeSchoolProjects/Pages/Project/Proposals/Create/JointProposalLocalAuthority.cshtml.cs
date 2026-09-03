using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using Microsoft.Extensions.Logging;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class JointProposalLocalAuthorityModel(
        ICreateProposalCache createProposalCache,
        IGetLocalAuthoritiesService getLocalAuthoritiesService,
        ILogger<JointProposalLocalAuthorityModel> logger,
        ErrorService errorService
    ) : ProposalLocalAuthorityModel(
        createProposalCache, getLocalAuthoritiesService, logger, errorService)
    {
        protected override string BackLinkRoute =>
            RouteConstants.Proposals_Create_Joint_Proposal_Region;

        protected override ProjectRegion? GetRegion(CreateProposalCacheItem cache) =>
            cache.JointProposalLocalAuthorityRegion;

        protected override string GetSelectedLocalAuthority(CreateProposalCacheItem cache) =>
            cache.JointProposalLocalAuthority;

        protected override void StoreLocalAuthority(CreateProposalCacheItem cache, string name, string code)
        {
            cache.JointProposalLocalAuthority = name;
            cache.JointProposalLocalAuthorityCode = code;
        }
    }
}
