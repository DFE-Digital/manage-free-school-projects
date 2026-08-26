using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
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
        // Proposed Faith (Common)
        public FaithStatus ProposedFaithStatus { get; set; }
        public FaithType ProposedFaithType { get; set; }
        public string OtherFaithType { get; set; }

        // Academy Trust route - ProposalProposer.AcademyTrust
        public ProposalProposer? Proposer { get; set; }
        public ProposalProposer? PreviousProposer { get; set; }
        public TrustTask Trust { get; set; }
        public bool? TrustConfirmed { get; set; }

        // Diocese - ProposalProposer.Diocese
        public string NameOfDiocese { get; set; }
        public FaithOfDiocese? FaithOfDiocese { get; set; }

        // Another religious organisation - ProposalProposer.AnotherReligiousOrganisation
        public string NameOfOtherReligiousOrganisation { get; set; }
        public FaithType? FaithTypeOfOtherReligiousOrganisation { get; set; }
        public string OtherFaithTypeOfOtherReligiousOrganisation { get; set; }

        // Another local authority - ProposalProposer.AnotherLocalAuthority
        public ProjectRegion? OtherLocalAuthorityRegion { get; set; }
        public string OtherLocalAuthority { get; set; }
        public string OtherLocalAuthorityCode { get; set; }

        // Joint proposal between the local authority that published the specification and another local authority - ProposalProposer.JointProposal
        public ProjectRegion? JointProposalLocalAuthorityRegion { get; set; }
        public string JointProposalLocalAuthority { get; set; }
        public string JointProposalLocalAuthorityCode { get; set; }

        // Check answers
        public bool ReachedCheckYourAnswers { get; set; }
    }
}
