using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;

namespace Dfe.ManageFreeSchoolProjects.ViewModels;

public class ProjectStatusViewModel
{
    public string ProjectId { get; set; }
    
    public ProjectStatus ProjectStatus { get; set; }
    
    public Referrer Referrer { get; set; }
    
}

public enum Referrer
{
    ProjectOverview = 0,
    TaskList = 1,
    ContactsOverview = 2,
    EditUnderwrittenPlaces = 3,
}