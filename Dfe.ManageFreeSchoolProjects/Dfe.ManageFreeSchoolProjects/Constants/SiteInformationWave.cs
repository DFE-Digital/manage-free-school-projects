namespace Dfe.ManageFreeSchoolProjects.Constants
{
    /// <summary>
    /// Maps a project type (as returned by the API, e.g. "New School") to the
    /// site information route segment (e.g. "newschool") and the matching API segment.
    /// </summary>
    public static class SiteInformationWave
    {
        public const string Central = "central";
        public const string Presumption = "presumption";
        public const string NewSchool = "newschool";

        public static string FromProjectType(string projectType)
        {
            return projectType switch
            {
                "Presumption" => Presumption,
                "New School" => NewSchool,
                _ => Central
            };
        }

        public static string ToApiSegment(string wave)
        {
            return string.Equals(wave, NewSchool, System.StringComparison.OrdinalIgnoreCase)
                ? "new-school"
                : Presumption;
        }
    }
}
