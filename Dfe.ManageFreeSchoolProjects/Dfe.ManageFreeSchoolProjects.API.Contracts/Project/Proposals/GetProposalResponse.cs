using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals.Enums;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals
{
    public class GetProposalResponse
    {
        public int Id { get; set; }
        public string Proposer { get; set; }

        public string Name { get; set; }

        public string ReligiousCharacterOrEthos { get; set; }

        public string ProposedFaithOfNewSchool { get; set; }

        public ProposalStatus Status { get; set; }
    }
}
