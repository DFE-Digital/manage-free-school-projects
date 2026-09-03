using System.Collections.Generic;

namespace Dfe.ManageFreeSchoolProjects.ViewModels
{
    public enum LocalAuthoritySelectionMode
    {
        Create,
        Edit
    }

    public class ProposalLocalAuthoritySelectionViewModel
    {
        public LocalAuthoritySelectionMode Mode { get; set; } = LocalAuthoritySelectionMode.Create;

        public string Title { get; set; }

        public string LocalAuthority { get; set; }

        public Dictionary<string, string> LocalAuthorities { get; set; } = new();

        public string ButtonText { get; set; } = "Continue";
    }
}
