using System.ComponentModel;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project
{
    public enum ProjectType
    {
        NotSet = 0,
        [Description("Presumption")]
        PresumptionRoute = 1,
        [Description("Central")]
        CentralRoute = 2,
        [Description("Local authority")]
        LocalAuthority = 3
    }
}
