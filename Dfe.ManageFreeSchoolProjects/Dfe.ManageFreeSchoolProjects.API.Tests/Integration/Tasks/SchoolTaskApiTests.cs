using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Tests.Fixtures;
using Dfe.ManageFreeSchoolProjects.API.Tests.Helpers;
using Dfe.ManageFreeSchoolProjects.API.Tests.Utils;
using System.Threading.Tasks;
using System.Linq;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Integration.Tasks
{
    [Collection(ApiTestCollection.ApiTestCollectionName)]
    public class SchoolTaskApiTests : ApiTestsBase
    {
        public SchoolTaskApiTests(ApiTestFixture apiTestFixture) : base(apiTestFixture)
        {
        }

        [Fact]
        public async Task Patch_SchoolTask_Returns_201()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;
            var previousFreeSchoolName = project.ProjectStatusCurrentFreeSchoolName;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var tasks = TasksStub.BuildListOfTasks(project.Rid);
            context.Tasks.AddRange(tasks);

            await context.SaveChangesAsync();

            var request = new UpdateProjectByTaskRequest()
            {
                School = new SchoolTask()
                {
                    CurrentFreeSchoolName = "Test High School",
                    SchoolType = SchoolType.Mainstream,
                    AgeRange = "11-18",
                    SchoolPhase = SchoolPhase.Primary,
                    Nursery = ClassType.Nursery.No,
                    SixthForm = ClassType.SixthForm.No,
                    ResidentialOrBoarding = ClassType.ResidentialOrBoarding.Yes,
                    AlternativeProvision = ClassType.AlternativeProvision.No,
                    SpecialEducationNeeds = ClassType.SpecialEducationNeeds.Yes,
                    FaithStatus = FaithStatus.NotSet,
                    FaithType = FaithType.Other,
                    Gender = Gender.Mixed,
                    FormsOfEntry = "3"
                }
            };

            var projectResponse = await _client.UpdateProjectTask(projectId, request, TaskName.School.ToString());

            projectResponse.School.CurrentFreeSchoolName.Should().Be("Test High School");
            projectResponse.School.SchoolType.Should().Be(SchoolType.Mainstream);
            projectResponse.School.SchoolPhase.Should().Be(SchoolPhase.Primary);
            projectResponse.School.AgeRange.Should().Be("11-18");
            projectResponse.School.Nursery.Should().Be(ClassType.Nursery.No);
            projectResponse.School.SixthForm.Should().Be(ClassType.SixthForm.No);
            projectResponse.School.AlternativeProvision.Should().Be(ClassType.AlternativeProvision.No);
            projectResponse.School.SpecialEducationNeeds.Should().Be(ClassType.SpecialEducationNeeds.Yes);
            projectResponse.SchoolName.Should().Be("Test High School");
            projectResponse.School.FormsOfEntry.Should().Be("3");
            projectResponse.School.ResidentialOrBoarding.Should().Be(ClassType.ResidentialOrBoarding.Yes);

            using var updatedContext = _testFixture.GetContext();

            var updatedProject = updatedContext.Kpi.First(p => p.ProjectStatusProjectId == projectId);

            updatedProject.ProjectStatusHasTheFreeSchoolChangedItsName.Should().Be("Yes");
            updatedProject.ProjectStatusPreviousFreeSchoolName.Should().Be(previousFreeSchoolName);
        }

        /// <summary>
        /// A New School project stores the school type without the historic "FS - " prefix,
        /// and has to read its own format back.
        /// </summary>
        [Theory]
        [InlineData(SchoolType.Mainstream, "Mainstream")]
        [InlineData(SchoolType.AlternativeProvision, "AP")]
        [InlineData(SchoolType.AlternativeProvisionPRU, "AP/PRU")]
        [InlineData(SchoolType.Special, "Special")]
        [InlineData(SchoolType.StudioSchool, "SS")]
        [InlineData(SchoolType.VoluntaryAided, "VA")]
        public async Task Patch_SchoolTask_WhenTheWaveIsNewSchool_StoresTheSchoolTypeWithoutThePrefix(
            SchoolType schoolType, string expectedStoredSchoolType)
        {
            var projectId = await CreateProjectWithWave(DatabaseModelBuilder.CreateProjectWave(ProjectType.NewSchool));

            var request = new UpdateProjectByTaskRequest()
            {
                School = new SchoolTask() { SchoolType = schoolType }
            };

            // UpdateProjectTask patches and then reads the task back, so this also proves
            // the stored value maps back to the same enum member.
            var projectResponse = await _client.UpdateProjectTask(projectId, request, TaskName.School.ToString());

            projectResponse.School.SchoolType.Should().Be(schoolType);

            using var updatedContext = _testFixture.GetContext();
            var updatedProject = updatedContext.Kpi.First(p => p.ProjectStatusProjectId == projectId);

            updatedProject.SchoolDetailsSchoolTypeMainstreamApEtc.Should().Be(expectedStoredSchoolType);
        }

        [Theory]
        [InlineData(SchoolType.Mainstream, "FS - Mainstream")]
        [InlineData(SchoolType.AlternativeProvision, "FS - AP")]
        [InlineData(SchoolType.AlternativeProvisionPRU, "FS - AP/PRU")]
        [InlineData(SchoolType.Special, "FS - Special")]
        [InlineData(SchoolType.StudioSchool, "SS")]
        [InlineData(SchoolType.VoluntaryAided, "VA")]
        public async Task Patch_SchoolTask_WhenTheWaveIsNotNewSchool_KeepsThePrefixOnTheSchoolType(
            SchoolType schoolType, string expectedStoredSchoolType)
        {
            var projectId = await CreateProjectWithWave(DatabaseModelBuilder.CreateProjectWave(ProjectType.PresumptionRoute));

            var request = new UpdateProjectByTaskRequest()
            {
                School = new SchoolTask() { SchoolType = schoolType }
            };

            var projectResponse = await _client.UpdateProjectTask(projectId, request, TaskName.School.ToString());

            projectResponse.School.SchoolType.Should().Be(schoolType);

            using var updatedContext = _testFixture.GetContext();
            var updatedProject = updatedContext.Kpi.First(p => p.ProjectStatusProjectId == projectId);

            updatedProject.SchoolDetailsSchoolTypeMainstreamApEtc.Should().Be(expectedStoredSchoolType);
        }

        private async Task<string> CreateProjectWithWave(string applicationWave)
        {
            var project = DatabaseModelBuilder.BuildProject();
            project.ProjectStatusFreeSchoolApplicationWave = applicationWave;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);
            context.Tasks.AddRange(TasksStub.BuildListOfTasks(project.Rid));

            await context.SaveChangesAsync();

            return project.ProjectStatusProjectId;
        }

        [Theory]
        [InlineData(FaithStatus.Designation)]
        [InlineData(FaithStatus.Ethos)]
        [InlineData(FaithStatus.None)]
        public async Task Patch_SchoolTask_FaithStatus_Is_Returned_By_Get(FaithStatus faithStatus)
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var tasks = TasksStub.BuildListOfTasks(project.Rid);
            context.Tasks.AddRange(tasks);

            await context.SaveChangesAsync();

            var faithType = faithStatus == FaithStatus.None ? FaithType.None : FaithType.ChurchOfEngland;

            var request = new UpdateProjectByTaskRequest()
            {
                School = new SchoolTask()
                {
                    CurrentFreeSchoolName = "Test High School",
                    FaithStatus = faithStatus,
                    FaithType = faithType
                }
            };

            var projectResponse = await _client.UpdateProjectTask(projectId, request, TaskName.School.ToString());

            projectResponse.School.FaithStatus.Should().Be(faithStatus);
            projectResponse.School.FaithType.Should().Be(faithType);

            using var updatedContext = _testFixture.GetContext();

            var updatedProject = updatedContext.Kpi.First(p => p.ProjectStatusProjectId == projectId);

            updatedProject.SchoolDetailsFaithStatus.Should().Be(faithStatus.ToString());
        }
    }
}
