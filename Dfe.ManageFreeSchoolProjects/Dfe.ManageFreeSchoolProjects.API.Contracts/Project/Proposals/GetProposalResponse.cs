using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals
{
    public class GetProposalResponse
    {
        public string Rid { get; set; }
        
        public string ProjectId { get; set; }

        public ProposalProposer Proposer { get; set; }

        public string Name { get; set; }

        public FaithStatus ProposedFaithStatus { get; set; }

        public FaithType? ProposedFaithType { get; set; }

        public ProposalStatus? Status { get; set; }
    }
}
