namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;

public class KickOffMeetingTask
{
    public bool? FundingArrangementAgreed { get; set; }
    
    public string FundingArrangementDetailsAgreed { get; set; }
    public string RealisticYearOfOpening { get; set; }
    
    public bool? SavedDocumentsInWorkplacesFolder { get; set; }
}