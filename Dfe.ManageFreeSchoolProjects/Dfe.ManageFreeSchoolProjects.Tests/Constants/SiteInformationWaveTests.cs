using Dfe.ManageFreeSchoolProjects.Constants;
using FluentAssertions;

namespace Dfe.ManageFreeSchoolProjects.Tests.Constants
{
    public class SiteInformationWaveTests
    {
        [Theory]
        [InlineData("Presumption", SiteInformationWave.Presumption)]
        [InlineData("New School", SiteInformationWave.NewSchool)]
        [InlineData("Central Route", SiteInformationWave.Central)]
        [InlineData(null, SiteInformationWave.Central)]
        public void FromProjectType_ReturnsTheRouteSegment(string? projectType, string expected)
        {
            SiteInformationWave.FromProjectType(projectType!).Should().Be(expected);
        }

        [Theory]
        [InlineData(SiteInformationWave.NewSchool, "new-school")]
        [InlineData("NewSchool", "new-school")]
        [InlineData(SiteInformationWave.Presumption, "presumption")]
        [InlineData(null, "presumption")]
        public void ToApiSegment_ReturnsTheApiSegment(string? wave, string expected)
        {
            SiteInformationWave.ToApiSegment(wave!).Should().Be(expected);
        }
    }
}
