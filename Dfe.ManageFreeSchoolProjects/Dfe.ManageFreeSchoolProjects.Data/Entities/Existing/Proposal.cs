using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.Data
{
    public partial class MfspContext : DbContext
    {
        public virtual DbSet<Entities.Existing.Proposal> Proposals { get; set; }
    }
}

namespace Dfe.ManageFreeSchoolProjects.Data.Entities.Existing
{
    public class Proposal : IAuditable
    {
        public string Rid { get; set; }

        public string ProjectId { get; set; }

        public string Proposer { get; set; }

        // Academy Trust route - ProposalProposer.AcademyTrust
        public string TrustReferenceNumber { get; set; }
        public string TrustName { get; set; }
        public string TrustType { get; set; }

        // Diocese - ProposalProposer.Diocese
        public string NameOfDiocese { get; set; }
        public string FaithOfDiocese { get; set; }

        // Another religious organisation - ProposalProposer.AnotherReligiousOrganisation
        public string NameOfOtherReligiousOrganisation { get; set; }
        public string FaithTypeOfOtherReligiousOrganisation { get; set; }
        public string OtherFaithTypeOfOtherReligiousOrganisation { get; set; }

        // Another local authority - ProposalProposer.AnotherLocalAuthority
        public string OtherLocalAuthority { get; set; }
        public string OtherLocalAuthorityRegion { get; set; }

        // Joint proposal between the local authority that published the specification and another local authority - ProposalProposer.JointProposal
        public string JointProposalLocalAuthority { get; set; }
        public string JointProposalLocalAuthorityRegion { get; set; }

        //Proposed Faith (Common)
        public string ProposedFaithStatus { get; set; }
        public string ProposedFaithType { get; set; }
        public string OtherFaithType { get; set; }
    }
}
