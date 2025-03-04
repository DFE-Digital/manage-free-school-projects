using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using DocumentFormat.OpenXml.EMMA;
using System.Collections.Generic;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project
{
    public static class ProjectConstants
    {
        public static readonly List<SchoolType> SchoolTypesWithSpecialistProvisions = [SchoolType.Mainstream, SchoolType.StudioSchool, SchoolType.UniversityTechnicalCollege, SchoolType.VoluntaryAided];

        public static readonly string[] ContactTypes = ["Team Lead", "Grade 6", "Project Director", "Trust Chair", "School Chair"];
    }
}
