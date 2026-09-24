namespace Dfe.ManageFreeSchoolProjects.ViewModels
{
    public class ProposalRegionSelectionViewModel
    {
        public ProposalJourneyMode Mode { get; set; } = ProposalJourneyMode.Create;

        public string Title { get; set; }

        public string Region { get; set; }

        public string ButtonText { get; set; } = "Continue";
    }
}
