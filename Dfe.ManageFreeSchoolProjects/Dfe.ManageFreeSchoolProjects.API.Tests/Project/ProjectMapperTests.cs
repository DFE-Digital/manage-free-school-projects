using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Project
{
    public class ProjectMapperTests
    {
        [Theory]
        [InlineData("FS - AP", SchoolType.AlternativeProvision)]
        [InlineData("FS - Special", SchoolType.Special)]
        [InlineData("SS", SchoolType.StudioSchool)]
        [InlineData("UTC", SchoolType.UniversityTechnicalCollege)]
        [InlineData("VA", SchoolType.VoluntaryAided)]
        [InlineData("InvalidType", SchoolType.NotSet)]
        public void ToSchoolType_Returns_ExpectedString(string input, SchoolType? expectedResult)
        {
            var result = ProjectMapper.ToSchoolType(input);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(SchoolType.AlternativeProvision, "FS - AP")]
        [InlineData(SchoolType.Special, "FS - Special")]
        [InlineData(SchoolType.StudioSchool, "SS")]
        [InlineData(SchoolType.UniversityTechnicalCollege, "UTC")]
        [InlineData(SchoolType.VoluntaryAided, "VA")]
        [InlineData(null, "NotSet")]
        public void ToSchoolType_ReturnsExpectedEnum(SchoolType? input, string expectedResult)
        {
            var result = ProjectMapper.ToSchoolType(input);

            Assert.Equal(expectedResult, result);
        }
        
        [Theory]
        [InlineData("Standalone",TrustType.SingleAcademyTrust)]
        [InlineData( "MAT",TrustType.MultiAcademyTrust)]
        [InlineData("NotSet",null)]
        public void ToTrustType_ReturnsExpectedEnum(string input, TrustType expectedResult)
        {
            var result = ProjectMapper.ToTrustType(input);
            Assert.Equal(expectedResult, result);
        }
        
        [Theory]
        [InlineData("Open",ProjectStatus.Open)]
        [InlineData( "Pre-opening",ProjectStatus.Preopening)]
        [InlineData( null,ProjectStatus.Preopening)]
        [InlineData( "AnyNotRecognised",ProjectStatus.Preopening)]
        [InlineData( "",ProjectStatus.Preopening)]
        [InlineData("Cancelled",ProjectStatus.Cancelled)]
        [InlineData("Cancelled during pre-opening",ProjectStatus.Cancelled)]
        [InlineData("Closed",ProjectStatus.Closed)]
        [InlineData("Withdrawn during pre-opening",ProjectStatus.WithdrawnInPreOpening)]
        [InlineData("Withdrawn in pre-opening",ProjectStatus.WithdrawnInPreOpening)]
        [InlineData("Application Competition stage",ProjectStatus.ApplicationCompetitionStage)]
        [InlineData("Application stage",ProjectStatus.ApplicationStage)]
        [InlineData("Open free school - Not included in figures", ProjectStatus.OpenNotIncludedInFigures)]
        [InlineData("Pre-opening - Not included in the figures", ProjectStatus.PreopeningNotIncludedInFigures)]
        [InlineData("Rejected at application stage", ProjectStatus.Rejected)]       
        public void ToProjectStatusType_ReturnsExpectedEnum(string input, ProjectStatus expectedResult)
        {
            var result = ProjectMapper.ToProjectStatusType(input);
            Assert.Equal(expectedResult, result);
        }

        //FromProjectStatusType

        [Theory]
        [InlineData(ProjectStatus.Open, "Open")]
        [InlineData(ProjectStatus.Preopening, "Pre-opening")]
        //[InlineData("AnyNotRecognised", ProjectStatus.Preopening)]
        [InlineData(ProjectStatus.Cancelled, "Cancelled during pre-opening")]
        [InlineData(ProjectStatus.Closed, "Closed")]
        [InlineData(ProjectStatus.WithdrawnInPreOpening, "Withdrawn in pre-opening")]
        [InlineData(ProjectStatus.ApplicationCompetitionStage, "Application Competition stage")]
        [InlineData(ProjectStatus.ApplicationStage, "Application stage")]
        [InlineData(ProjectStatus.OpenNotIncludedInFigures, "Open free school - Not included in figures")]
        [InlineData(ProjectStatus.PreopeningNotIncludedInFigures, "Pre-opening - Not included in the figures")]
        [InlineData(ProjectStatus.Rejected, "Rejected at application stage")]
        public void FromProjectStatusType_ReturnsExpectedEnum(ProjectStatus input, string expectedResult)
        {
            var result = ProjectMapper.FromProjectStatusType(input);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(SchoolPhase.NotSet, "NotSet")]
        [InlineData(SchoolPhase.Primary, "Primary")]
        [InlineData(SchoolPhase.Secondary, "Secondary")]
        [InlineData(SchoolPhase.SixteenToNineteen, "16-19")]
        [InlineData(SchoolPhase.AllThrough, "All-Through")]
        public void ToSchoolPhaseEnum_ReturnsExpectedEnum(SchoolPhase input, string expectedResult)
        {
            var result = ProjectMapper.ToSchoolPhase(input);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("Primary", SchoolPhase.Primary)]
        [InlineData("Secondary", SchoolPhase.Secondary)]
        [InlineData("16-19", SchoolPhase.SixteenToNineteen)]
        [InlineData("16 to 19", SchoolPhase.SixteenToNineteen)]
        [InlineData("All-Through", SchoolPhase.AllThrough)]
        [InlineData("All-through", SchoolPhase.AllThrough)]
        [InlineData("", SchoolPhase.NotSet)]
        [InlineData("Not a valid school phase", SchoolPhase.NotSet)]
        public void ToSchoolPhaseString_ReturnsExpectedEnum(string input, SchoolPhase expectedResult)
        {
            var result = ProjectMapper.ToSchoolPhase(input);
            Assert.Equal(expectedResult, result);
        }
    }
}
