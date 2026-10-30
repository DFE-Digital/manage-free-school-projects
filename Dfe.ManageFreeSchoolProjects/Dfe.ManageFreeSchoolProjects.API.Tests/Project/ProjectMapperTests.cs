using System;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Project
{
    public class ProjectMapperTests
    {
        private const string NewSchoolWave = "New School";

        /// <summary>
        /// Every wave other than "New School" stores the school type with the historic
        /// "FS - " prefix, so the mapper has to keep reading and writing it for them.
        /// </summary>
        private static readonly string[] WavesThatKeepThePrefix =
        [
            "FS - Presumption", "Central Route", "Non-Presumption", "Wave 15", string.Empty, null
        ];

        public static TheoryData<string, string, SchoolType> PrefixedStringToEnumData()
        {
            var data = new TheoryData<string, string, SchoolType>();

            foreach (var wave in WavesThatKeepThePrefix)
            {
                data.Add(wave, "FS - AP", SchoolType.AlternativeProvision);
                data.Add(wave, "FS - AP/PRU", SchoolType.AlternativeProvisionPRU);
                data.Add(wave, "FS - Special", SchoolType.Special);
                data.Add(wave, "FS - Mainstream", SchoolType.Mainstream);
                data.Add(wave, "SS", SchoolType.StudioSchool);
                data.Add(wave, "UTC", SchoolType.UniversityTechnicalCollege);
                data.Add(wave, "FE", SchoolType.FurtherEducation);
                data.Add(wave, "VA", SchoolType.VoluntaryAided);
                data.Add(wave, "InvalidType", SchoolType.NotSet);
                data.Add(wave, null, SchoolType.NotSet);

                // The unprefixed names are only school types on a New School project.
                data.Add(wave, "AP", SchoolType.NotSet);
                data.Add(wave, "AP/PRU", SchoolType.NotSet);
                data.Add(wave, "Special", SchoolType.NotSet);
                data.Add(wave, "Mainstream", SchoolType.NotSet);
            }

            return data;
        }

        [Theory]
        [MemberData(nameof(PrefixedStringToEnumData))]
        public void ToSchoolType_WhenTheWaveKeepsThePrefix_ReturnsExpectedEnum(
            string applicationWave, string input, SchoolType expectedResult)
        {
            var result = ProjectMapper.ToSchoolType(input, applicationWave);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("AP", SchoolType.AlternativeProvision)]
        [InlineData("AP/PRU", SchoolType.AlternativeProvisionPRU)]
        [InlineData("Special", SchoolType.Special)]
        [InlineData("Mainstream", SchoolType.Mainstream)]
        [InlineData("SS", SchoolType.StudioSchool)]
        [InlineData("UTC", SchoolType.UniversityTechnicalCollege)]
        [InlineData("FE", SchoolType.FurtherEducation)]
        [InlineData("VA", SchoolType.VoluntaryAided)]
        [InlineData("InvalidType", SchoolType.NotSet)]
        [InlineData(null, SchoolType.NotSet)]
        public void ToSchoolType_WhenTheWaveIsNewSchool_ReturnsExpectedEnum(
            string input, SchoolType expectedResult)
        {
            var result = ProjectMapper.ToSchoolType(input, NewSchoolWave);

            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// A New School project never stores the prefix, so a prefixed value is not a
        /// school type it can recognise.
        /// </summary>
        [Theory]
        [InlineData("FS - AP")]
        [InlineData("FS - AP/PRU")]
        [InlineData("FS - Special")]
        [InlineData("FS - Mainstream")]
        public void ToSchoolType_WhenTheWaveIsNewSchool_DoesNotRecogniseThePrefixedString(string input)
        {
            var result = ProjectMapper.ToSchoolType(input, NewSchoolWave);

            Assert.Equal(SchoolType.NotSet, result);
        }

        public static TheoryData<string, SchoolType?, string> EnumToPrefixedStringData()
        {
            var data = new TheoryData<string, SchoolType?, string>();

            foreach (var wave in WavesThatKeepThePrefix)
            {
                data.Add(wave, SchoolType.AlternativeProvision, "FS - AP");
                data.Add(wave, SchoolType.AlternativeProvisionPRU, "FS - AP/PRU");
                data.Add(wave, SchoolType.Special, "FS - Special");
                data.Add(wave, SchoolType.Mainstream, "FS - Mainstream");
                data.Add(wave, SchoolType.StudioSchool, "SS");
                data.Add(wave, SchoolType.UniversityTechnicalCollege, "UTC");
                data.Add(wave, SchoolType.FurtherEducation, "FE");
                data.Add(wave, SchoolType.VoluntaryAided, "VA");
                data.Add(wave, SchoolType.NotSet, "NotSet");
                data.Add(wave, null, "NotSet");
            }

            return data;
        }

        [Theory]
        [MemberData(nameof(EnumToPrefixedStringData))]
        public void ToSchoolType_WhenTheWaveKeepsThePrefix_ReturnsExpectedString(
            string applicationWave, SchoolType? input, string expectedResult)
        {
            var result = ProjectMapper.ToSchoolType(input, applicationWave);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(SchoolType.AlternativeProvision, "AP")]
        [InlineData(SchoolType.AlternativeProvisionPRU, "AP/PRU")]
        [InlineData(SchoolType.Special, "Special")]
        [InlineData(SchoolType.Mainstream, "Mainstream")]
        [InlineData(SchoolType.StudioSchool, "SS")]
        [InlineData(SchoolType.UniversityTechnicalCollege, "UTC")]
        [InlineData(SchoolType.FurtherEducation, "FE")]
        [InlineData(SchoolType.VoluntaryAided, "VA")]
        [InlineData(SchoolType.NotSet, "NotSet")]
        [InlineData(null, "NotSet")]
        public void ToSchoolType_WhenTheWaveIsNewSchool_ReturnsExpectedString(
            SchoolType? input, string expectedResult)
        {
            var result = ProjectMapper.ToSchoolType(input, NewSchoolWave);

            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// The school type is round tripped through the database as a string, so whatever
        /// the mapper writes for a wave has to map back to the same enum member.
        /// </summary>
        [Theory]
        [InlineData(SchoolType.AlternativeProvision)]
        [InlineData(SchoolType.AlternativeProvisionPRU)]
        [InlineData(SchoolType.Special)]
        [InlineData(SchoolType.Mainstream)]
        [InlineData(SchoolType.StudioSchool)]
        [InlineData(SchoolType.UniversityTechnicalCollege)]
        [InlineData(SchoolType.FurtherEducation)]
        [InlineData(SchoolType.VoluntaryAided)]
        [InlineData(SchoolType.NotSet)]
        public void ToSchoolType_WhenTheWaveIsNewSchool_RoundTrips(SchoolType schoolType)
        {
            var result = ProjectMapper.ToSchoolType(
                ProjectMapper.ToSchoolType(schoolType, NewSchoolWave), NewSchoolWave);

            Assert.Equal(schoolType, result);
        }

        [Theory]
        [MemberData(nameof(RoundTripsForWavesThatKeepThePrefixData))]
        public void ToSchoolType_WhenTheWaveKeepsThePrefix_RoundTrips(
            string applicationWave, SchoolType schoolType)
        {
            var result = ProjectMapper.ToSchoolType(
                ProjectMapper.ToSchoolType(schoolType, applicationWave), applicationWave);

            Assert.Equal(schoolType, result);
        }

        public static TheoryData<string, SchoolType> RoundTripsForWavesThatKeepThePrefixData()
        {
            var data = new TheoryData<string, SchoolType>();

            foreach (var wave in WavesThatKeepThePrefix)
            {
                foreach (var schoolType in Enum.GetValues<SchoolType>())
                {
                    data.Add(wave, schoolType);
                }
            }

            return data;
        }

        /// <summary>
        /// The gate is an exact match on the wave, so a differently cased value is treated
        /// as any other wave and keeps the prefix.
        /// </summary>
        [Theory]
        [InlineData("new school")]
        [InlineData("NEW SCHOOL")]
        [InlineData("New School ")]
        public void ToSchoolType_WhenTheWaveIsNotExactlyNewSchool_KeepsThePrefix(string applicationWave)
        {
            var result = ProjectMapper.ToSchoolType(SchoolType.Mainstream, applicationWave);

            Assert.Equal("FS - Mainstream", result);
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
        public void ToProjectCancelledReasonType_ReturnsExpectedEnum(
            string input, ProjectCancelledReason expectedResult)
        {
            var result = ProjectMapper.ToProjectCancelledReasonType(input);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ToProjectCancelledReasonType_WhenThereIsNoReason_ReturnsNotSet()
        {
            Assert.Equal(ProjectCancelledReason.NotSet, ProjectMapper.ToProjectCancelledReasonType(null));
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
        public void ToProjectWithdrawnReasonType_ReturnsExpectedEnum(
            string input, ProjectWithdrawnReason expectedResult)
        {
            var result = ProjectMapper.ToProjectWithdrawnReasonType(input);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ToProjectWithdrawnReasonType_WhenThereIsNoReason_ReturnsNotSet()
        {
            Assert.Equal(ProjectWithdrawnReason.NotSet, ProjectMapper.ToProjectWithdrawnReasonType(null));
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

        [Theory]
        [InlineData("Active", ProposalStatus.Active)]
        [InlineData("Successful", ProposalStatus.Successful)]
        [InlineData("Unsuccessful", ProposalStatus.Unsuccessful)]
        [InlineData(null, ProposalStatus.Active)]
        [InlineData("", ProposalStatus.Active)]
        [InlineData("Not a status", ProposalStatus.Active)]
        public void ToProposalStatus_ReturnsExpectedEnum(string input, ProposalStatus expectedResult)
        {
            var result = ProjectMapper.ToProposalStatus(input);
            Assert.Equal(expectedResult, result);
        }
    }
}
