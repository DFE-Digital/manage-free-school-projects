namespace Dfe.ManageFreeSchoolProjects.ViewModels
{
    public class ProposalRadiosSelectionViewModel
    {
        public ProposalJourneyMode Mode { get; set; } = ProposalJourneyMode.Create;

        public string Title { get; set; }

        public bool AddMargin { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string[] Labels { get; set; } = [];

        public string[] Values { get; set; } = [];

        public string[] Hints { get; set; }

        public string ButtonText { get; set; } = "Continue";
    }
}
