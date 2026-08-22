

using System.ComponentModel;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Proposals.Enums
{
    public enum ProposalProposer
    {
        [Description("Academy trust (including Diocese academy trust)")]
        AcademyTrust = 1,
        [Description("Diocese")]
        Diocese = 2,
        [Description("Another religious organisation")]
        AnotherReligiousOrganisation = 3,
        [Description("Local authority that published the specification")]
        LocalAuthority = 4,
        [Description("Another local authority")]
        AnotherLocalAuthority = 5,
        [Description("Joint proposal between the local authority that published the specification and another local authority")]
        JointProposal = 6
    }
}
