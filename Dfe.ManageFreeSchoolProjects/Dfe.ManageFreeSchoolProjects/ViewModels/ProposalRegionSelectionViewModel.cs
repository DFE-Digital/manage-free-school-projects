namespace Dfe.ManageFreeSchoolProjects.ViewModels
{
    public enum RegionSelectionMode
    {
        Create,
        Edit
    }

    public class ProposalRegionSelectionViewModel
    {
        public RegionSelectionMode Mode { get; set; } = RegionSelectionMode.Create;

        public string Title { get; set; }

        public string Region { get; set; }

        public string ButtonText { get; set; } = "Continue";
    }
}
