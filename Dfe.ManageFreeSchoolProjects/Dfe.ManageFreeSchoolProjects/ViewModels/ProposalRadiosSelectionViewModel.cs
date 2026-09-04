namespace Dfe.ManageFreeSchoolProjects.ViewModels
{
    public class ProposalRadiosSelectionViewModel
    {
        public ProposalJourneyMode Mode { get; set; } = ProposalJourneyMode.Create;

        public string Title { get; set; }

        public bool AddMargin { get; set; }

        /// <summary>
        /// The posted field name. Also used as the element id and test id.
        /// </summary>
        public string Name { get; set; }

        public string Value { get; set; }

        public string[] Labels { get; set; } = [];

        public string[] Values { get; set; } = [];

        /// <summary>
        /// Optional hint text per option. Null when the options need no hints.
        /// </summary>
        public string[] Hints { get; set; }

        public string ButtonText { get; set; } = "Continue";
    }
}
