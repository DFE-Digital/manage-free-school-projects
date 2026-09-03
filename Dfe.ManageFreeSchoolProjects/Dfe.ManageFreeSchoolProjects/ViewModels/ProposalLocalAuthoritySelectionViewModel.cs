using System.Collections.Generic;

namespace Dfe.ManageFreeSchoolProjects.ViewModels
{
    public class ProposalLocalAuthoritySelectionViewModel
    {
        public string Title { get; set; }

        public string LocalAuthority { get; set; }

        public Dictionary<string, string> LocalAuthorities { get; set; } = new();
    }
}
