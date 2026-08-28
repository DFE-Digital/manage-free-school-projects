using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using Microsoft.Extensions.Logging;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public class LocalAuthorityModel(
        ICreateProposalCache createProposalCache,
        IGetLocalAuthoritiesService getLocalAuthoritiesService,
        ILogger<LocalAuthorityModel> logger,
        ErrorService errorService
    ) : ProposalLocalAuthorityModel<LocalAuthorityModel>(
        createProposalCache, getLocalAuthoritiesService, logger, errorService)
    {
        protected override string BackLinkRoute =>
            RouteConstants.Proposals_Create_Other_Local_Authority_Region;

        protected override ProjectRegion? GetRegion(CreateProposalCacheItem cache) =>
            cache.OtherLocalAuthorityRegion;

        protected override string GetSelectedLocalAuthority(CreateProposalCacheItem cache) =>
            cache.OtherLocalAuthority;

        protected override void StoreLocalAuthority(CreateProposalCacheItem cache, string name, string code)
        {
            cache.OtherLocalAuthority = name;
            cache.OtherLocalAuthorityCode = code;
        }
    }
}
