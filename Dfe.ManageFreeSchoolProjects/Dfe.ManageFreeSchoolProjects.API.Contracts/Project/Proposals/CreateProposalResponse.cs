using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals
{
    public record CreateProposalResponse
    {
        public string Rid { get; set; }

        public string ProjectId { get; set; }

        public ProposalProposer Proposer { get; set; }

        // Academy Trust route - ProposalProposer.AcademyTrust
        public string TrustReferenceNumber { get; set; }
        public string TrustName { get; set; }
        public TrustType? TrustType { get; set; }

        // Diocese - ProposalProposer.Diocese
        public string NameOfDiocese { get; set; }
        public FaithOfDiocese? FaithOfDiocese { get; set; }

        // Another religious organisation - ProposalProposer.AnotherReligiousOrganisation
        public string NameOfOtherReligiousOrganisation { get; set; }
        public FaithType? FaithTypeOfOtherReligiousOrganisation { get; set; }
        public string OtherFaithTypeOfOtherReligiousOrganisation { get; set; }

        // Another local authority - ProposalProposer.AnotherLocalAuthority
        public string OtherLocalAuthority { get; set; }

        // Joint proposal between the local authority that published the specification and another local authority - ProposalProposer.JointProposal
        public string JointProposalLocalAuthority { get; set; }

        //Proposed Faith (Common)
        public FaithStatus ProposedFaithStatus { get; set; }
        public FaithType ProposedFaithType { get; set; }
        public string OtherFaithType { get; set; }
    }
}
