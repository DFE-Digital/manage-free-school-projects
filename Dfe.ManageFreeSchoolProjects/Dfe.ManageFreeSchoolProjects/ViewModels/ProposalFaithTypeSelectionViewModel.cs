namespace Dfe.ManageFreeSchoolProjects.ViewModels
{
    public class ProposalFaithTypeSelectionViewModel
    {
        public ProposalJourneyMode Mode { get; set; } = ProposalJourneyMode.Create;

        public string Title { get; set; }

        public string FaithType { get; set; }

        public string OtherFaithType { get; set; }

        public string ButtonText { get; set; } = "Continue";
    }
}
