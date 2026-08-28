using System;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Project
{
    public class ProjectMapperTests
    {
        [Theory]
        [InlineData("FS - AP", SchoolType.AlternativeProvision)]
        [InlineData("FS - AP/PRU", SchoolType.AlternativeProvisionPRU)]
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
        [InlineData(SchoolType.AlternativeProvisionPRU, "FS - AP/PRU")]
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
        [InlineData("Cancelled trust competition",ProjectStatus.CancelledTrustCompetition)]
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
        [InlineData(ProjectStatus.CancelledTrustCompetition, "Cancelled trust competition")]
        [InlineData(ProjectStatus.Closed, "Closed")]
        [InlineData(ProjectStatus.WithdrawnInPreOpening, "Withdrawn during pre-opening")]
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

        [Theory]
        [InlineData(FaithType.ChurchOfEngland, "Church of England")]
        [InlineData(FaithType.Christian, "Christian")]
        [InlineData(FaithType.GreekOrthodox, "Greek Orthodox")]
        [InlineData(FaithType.Hindu, "Hindu")]
        [InlineData(FaithType.Jewish, "Jewish")]
        [InlineData(FaithType.Methodist, "Methodist")]
        [InlineData(FaithType.Muslim, "Muslim")]
        [InlineData(FaithType.RomanCatholic, "Roman Catholic")]
        [InlineData(FaithType.Sikh, "Sikh")]
        [InlineData(FaithType.Other, "Other")]
        [InlineData(FaithType.None, "None")]
        [InlineData(FaithType.NotSet, null)]
        public void ToFaithType_ReturnsExpectedString(FaithType input, string expectedResult)
        {
            var result = ProjectMapper.ToFaithType(input);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("Church of England", FaithType.ChurchOfEngland)]
        [InlineData("Christian", FaithType.Christian)]
        [InlineData("Greek Orthodox", FaithType.GreekOrthodox)]
        [InlineData("Hindu", FaithType.Hindu)]
        [InlineData("Jewish", FaithType.Jewish)]
        [InlineData("Methodist", FaithType.Methodist)]
        [InlineData("Muslim", FaithType.Muslim)]
        [InlineData("Roman Catholic", FaithType.RomanCatholic)]
        [InlineData("Sikh", FaithType.Sikh)]
        [InlineData("Other", FaithType.Other)]
        [InlineData("None", FaithType.None)]
        public void ToFaithType_ReturnsExpectedEnum(string input, FaithType expectedResult)
        {
            var result = ProjectMapper.ToFaithType(input);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("ChurchOfEngland", FaithType.ChurchOfEngland)]
        [InlineData("RomanCatholic", FaithType.RomanCatholic)]
        [InlineData("Not a faith", FaithType.NotSet)]
        [InlineData("", FaithType.NotSet)]
        public void ToFaithType_WhenNotADescription_FallsBackToTheEnumParser(
            string input, FaithType expectedResult)
        {
            var result = ProjectMapper.ToFaithType(input);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(TrustType.SingleAcademyTrust, "Standalone")]
        [InlineData(TrustType.MultiAcademyTrust, "MAT")]
        public void ToTrustType_ReturnsExpectedString(TrustType input, string expectedResult)
        {
            var result = ProjectMapper.ToTrustType(input);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ToTrustType_WhenNotSet_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ProjectMapper.ToTrustType(TrustType.NotSet));
        }

        [Theory]
        [InlineData("education quality", ProjectCancelledReason.EducationQuality)]
        [InlineData("Education Quality", ProjectCancelledReason.EducationQuality)]
        [InlineData("governance", ProjectCancelledReason.Governance)]
        [InlineData("site and planning issues", ProjectCancelledReason.SiteAndPlanningIssues)]
        [InlineData("pupil numbers", ProjectCancelledReason.PupilNumbers)]
        [InlineData("something else", ProjectCancelledReason.NotSet)]
        [InlineData(null, ProjectCancelledReason.NotSet)]
        public void ToProjectCancelledReasonType_ReturnsExpectedEnum(
            string input, ProjectCancelledReason expectedResult)
        {
            var result = ProjectMapper.ToProjectCancelledReasonType(input);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(ProjectCancelledReason.EducationQuality, "education quality")]
        [InlineData(ProjectCancelledReason.Governance, "governance")]
        [InlineData(ProjectCancelledReason.SiteAndPlanningIssues, "site and planning issues")]
        [InlineData(ProjectCancelledReason.PupilNumbers, "pupil numbers")]
        [InlineData(ProjectCancelledReason.NotSet, "")]
        public void FromProjectCancelledReasonType_ReturnsExpectedString(
            ProjectCancelledReason input, string expectedResult)
        {
            var result = ProjectMapper.FromProjectCancelledReasonType(input);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("education quality", ProjectWithdrawnReason.EducationQuality)]
        [InlineData("Education Quality", ProjectWithdrawnReason.EducationQuality)]
        [InlineData("governance", ProjectWithdrawnReason.Governance)]
        [InlineData("site and planning issues", ProjectWithdrawnReason.SiteAndPlanningIssues)]
        [InlineData("pupil numbers", ProjectWithdrawnReason.PupilNumbers)]
        [InlineData("something else", ProjectWithdrawnReason.NotSet)]
        [InlineData(null, ProjectWithdrawnReason.NotSet)]
        public void ToProjectWithdrawnReasonType_ReturnsExpectedEnum(
            string input, ProjectWithdrawnReason expectedResult)
        {
            var result = ProjectMapper.ToProjectWithdrawnReasonType(input);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(ProjectWithdrawnReason.EducationQuality, "education quality")]
        [InlineData(ProjectWithdrawnReason.Governance, "governance")]
        [InlineData(ProjectWithdrawnReason.SiteAndPlanningIssues, "site and planning issues")]
        [InlineData(ProjectWithdrawnReason.PupilNumbers, "pupil numbers")]
        [InlineData(ProjectWithdrawnReason.NotSet, "NotSet")]
        public void FromProjectWithdrawnReasonType_ReturnsExpectedString(
            ProjectWithdrawnReason input, string expectedResult)
        {
            var result = ProjectMapper.FromProjectWithdrawnReasonType(input);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(ProposalProposer.AcademyTrust, "Academy trust (including Diocese academy trust)")]
        [InlineData(ProposalProposer.Diocese, "Diocese")]
        [InlineData(ProposalProposer.AnotherReligiousOrganisation, "Another religious organisation")]
        [InlineData(ProposalProposer.LocalAuthorityThatPushedSpecification, "Local authority that published the specification")]
        [InlineData(ProposalProposer.AnotherLocalAuthority, "Another local authority")]
        [InlineData(ProposalProposer.JointProposal, "Joint proposal between the local authority that published the specification and another local authority")]
        public void ToProposer_ReturnsExpectedString(ProposalProposer input, string expectedResult)
        {
            var result = ProjectMapper.ToProposer(input);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ToProposer_WhenTheProposerIsNotRecognised_ReturnsEmpty()
        {
            var result = ProjectMapper.ToProposer((ProposalProposer)0);
            Assert.Equal(string.Empty, result);
        }

        /// <summary>
        /// The proposer is round tripped through the database as its description, so every value
        /// written by ToProposer has to map back to the same enum member.
        /// </summary>
        [Theory]
        [InlineData(ProposalProposer.AcademyTrust)]
        [InlineData(ProposalProposer.Diocese)]
        [InlineData(ProposalProposer.AnotherReligiousOrganisation)]
        [InlineData(ProposalProposer.LocalAuthorityThatPushedSpecification)]
        [InlineData(ProposalProposer.AnotherLocalAuthority)]
        [InlineData(ProposalProposer.JointProposal)]
        public void ToProposer_RoundTrips(ProposalProposer proposer)
        {
            var result = ProjectMapper.ToProposer(ProjectMapper.ToProposer(proposer));
            Assert.Equal(proposer, result);
        }

        [Fact]
        public void ToProposer_WhenTheDescriptionIsNotRecognised_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ProjectMapper.ToProposer("Not a proposer"));
        }

        [Theory]
        [InlineData(FaithOfDiocese.ChurchOfEngland, "Church of England")]
        [InlineData(FaithOfDiocese.RomanCatholic, "Roman Catholic")]
        public void ToDioceseFaithType_ReturnsExpectedString(FaithOfDiocese input, string expectedResult)
        {
            var result = ProjectMapper.ToDioceseFaithType(input);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ToDioceseFaithType_WhenTheFaithIsNotRecognised_ReturnsEmpty()
        {
            var result = ProjectMapper.ToDioceseFaithType((FaithOfDiocese)0);
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData("Church of England", FaithOfDiocese.ChurchOfEngland)]
        [InlineData("Roman Catholic", FaithOfDiocese.RomanCatholic)]
        public void ToDioceseFaithType_ReturnsExpectedEnum(string input, FaithOfDiocese expectedResult)
        {
            var result = ProjectMapper.ToDioceseFaithType(input);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ToDioceseFaithType_WhenTheDescriptionIsNotRecognised_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ProjectMapper.ToDioceseFaithType("Not a faith"));
        }

        [Theory]
        [InlineData(FaithStatus.Designation, "Designation")]
        [InlineData(FaithStatus.Ethos, "Ethos")]
        [InlineData(FaithStatus.None, "None")]
        [InlineData(FaithStatus.NotSet, "NotSet")]
        public void ToFaithStatus_ReturnsExpectedString(FaithStatus input, string expectedResult)
        {
            var result = ProjectMapper.ToFaithStatus(input);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ToFaithStatus_WhenTheStatusIsNotRecognised_ReturnsEmpty()
        {
            var result = ProjectMapper.ToFaithStatus((FaithStatus)99);
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData("Designation", FaithStatus.Designation)]
        [InlineData("Ethos", FaithStatus.Ethos)]
        [InlineData("None", FaithStatus.None)]
        [InlineData("NotSet", FaithStatus.NotSet)]
        public void ToFaithStatus_ReturnsExpectedEnum(string input, FaithStatus expectedResult)
        {
            var result = ProjectMapper.ToFaithStatus(input);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ToFaithStatus_WhenTheStringIsNotRecognised_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ProjectMapper.ToFaithStatus("Not a status"));
        }
    }
}
