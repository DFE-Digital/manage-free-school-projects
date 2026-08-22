using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals.Enums;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Proposals.Create
{
    public interface ICreateProposalCache : ICookieCacheService<CreateProposalCacheItem>;

    public class CreateProposalCache(IHttpContextAccessor httpContextAccessor, IDataProtectionProvider dataProtectionProvider) : CookieCacheService<CreateProposalCacheItem>(httpContextAccessor, dataProtectionProvider, "CREATE_PROPOSAL"), ICreateProposalCache
    {
    }

    public record CreateProposalCacheItem
    {
        public ProposalProposer? Proposer { get; set; }

        public TrustTask Trust { get; set; }

        public bool TrustConfirmed { get; set; }
    }
}
