using System.Collections.Generic;

namespace Dfe.ManageFreeSchoolProjects.ViewModels
{
    public class ProposalLocalAuthoritySelectionViewModel
    {
        public ProposalJourneyMode Mode { get; set; } = ProposalJourneyMode.Create;

        public string Title { get; set; }

        public string LocalAuthority { get; set; }

        public Dictionary<string, string> LocalAuthorities { get; set; } = new();

        public string ButtonText { get; set; } = "Continue";
    }
}
