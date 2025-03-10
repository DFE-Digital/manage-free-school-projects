﻿namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;

public class GetProjectSitesPresumptionResponse
{
    public ProjectSite PermanentSite { get; set; } = new();
    public ProjectSite TemporarySite { get; set; } = new();
    public string SchoolName { get; set; }
    public string ProjectType { get; set; }
}