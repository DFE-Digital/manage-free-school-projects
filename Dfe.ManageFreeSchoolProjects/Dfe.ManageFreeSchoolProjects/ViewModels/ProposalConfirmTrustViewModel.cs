using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.Enums;

namespace Dfe.ManageFreeSchoolProjects.ViewModels
{
    public class ProposalConfirmTrustViewModel
    {
        public ProposalJourneyMode Mode { get; set; } = ProposalJourneyMode.Create;

        public string Title { get; set; }

        public TrustTask Trust { get; set; }

        public YesNoOption? ConfirmOption { get; set; }

        public string ButtonText { get; set; } = "Continue";
    }
}
