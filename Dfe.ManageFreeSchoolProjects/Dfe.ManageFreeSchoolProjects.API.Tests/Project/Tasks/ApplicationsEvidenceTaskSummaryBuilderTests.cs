using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.ApplicationsEvidence;
using FluentAssertions;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Project.Tasks
{
    public class ApplicationsEvidenceTaskSummaryBuilderTests
    {
        // AlternativeProvisionPRU is deliberately absent from both theories: whether it should
        // hide this task is an open question, so no test pins the behaviour either way yet.
        [Theory]
        [InlineData(SchoolType.AlternativeProvision)]
        [InlineData(SchoolType.Special)]
        public void Build_ForProvisionSchoolTypes_HidesTask(SchoolType schoolType)
        {
            var result = Build(schoolType);

            result.IsHidden.Should().BeTrue();
        }

        [Theory]
        [InlineData(SchoolType.Mainstream)]
        [InlineData(SchoolType.StudioSchool)]
        [InlineData(SchoolType.UniversityTechnicalCollege)]
        [InlineData(SchoolType.VoluntaryAided)]
        [InlineData(SchoolType.FurtherEducation)]
        [InlineData(SchoolType.NotSet)]
        public void Build_ForOtherSchoolTypes_LeavesTaskVisible(SchoolType schoolType)
        {
            var result = Build(schoolType);

            result.IsHidden.Should().BeFalse();
        }

        private static TaskSummaryResponse Build(SchoolType schoolType)
        {
            return new ApplicationsEvidenceTaskSummaryBuilder().Build(
                new ApplicationsEvidenceTaskSummaryBuilderParameters
                {
                    SchoolType = schoolType,
                    TaskSummary = new TaskSummaryResponse()
                });
        }
    }
}
