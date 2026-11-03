using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Extensions;
using Dfe.ManageFreeSchoolProjects.API.Tests.Fixtures;
using Dfe.ManageFreeSchoolProjects.API.Tests.Helpers;
using Dfe.ManageFreeSchoolProjects.API.Tests.Utils;
using System.Net;
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
                    EstablishmentType = EstablishmentType.AcademySponsorLed,
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
            projectResponse.School.EstablishmentType.Should().Be(EstablishmentType.AcademySponsorLed);
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
            updatedProject.SchoolDetailsEstablishmentType.Should().Be("Academy sponsor led");
        }

        /// <summary>
        /// The establishment type is stored in the KPI table as the text the radios display, so every
        /// member has to survive the trip out to the database and back.
        /// </summary>
        [Theory]
        [InlineData(EstablishmentType.AcademyAlternativeProvisionSponsorLed)]
        [InlineData(EstablishmentType.AcademySpecialSponsorLed)]
        [InlineData(EstablishmentType.AcademySponsorLed)]
        [InlineData(EstablishmentType.CommunitySchool)]
        [InlineData(EstablishmentType.CommunitySpecialSchool)]
        [InlineData(EstablishmentType.FoundationSchool)]
        [InlineData(EstablishmentType.FoundationSpecialSchool)]
        [InlineData(EstablishmentType.PupilReferralUnit)]
        [InlineData(EstablishmentType.VoluntaryAidedSchool)]
        [InlineData(EstablishmentType.VoluntaryControlledSchool)]
        public async Task Patch_SchoolTask_EstablishmentType_Is_Returned_By_Get(EstablishmentType establishmentType)
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

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
                    EstablishmentType = establishmentType
                }
            };

            var projectResponse = await _client.UpdateProjectTask(projectId, request, TaskName.School.ToString());

            projectResponse.School.EstablishmentType.Should().Be(establishmentType);

            using var updatedContext = _testFixture.GetContext();

            var updatedProject = updatedContext.Kpi.First(p => p.ProjectStatusProjectId == projectId);

            updatedProject.SchoolDetailsEstablishmentType.Should().Be(establishmentType.ToDescription());
        }

        /// <summary>
        /// Projects created before the establishment type was captured have nothing stored against
        /// them, which has to read back as unset rather than failing the get.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Some legacy value")]
        public async Task Get_SchoolTask_WhenTheStoredEstablishmentTypeIsNotRecognised_ReturnsNotSet(string storedEstablishmentType)
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;
            project.SchoolDetailsEstablishmentType = storedEstablishmentType;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var tasks = TasksStub.BuildListOfTasks(project.Rid);
            context.Tasks.AddRange(tasks);

            await context.SaveChangesAsync();

            var getProjectByTaskResponse = await _client.GetAsync(
                $"/api/v1/client/projects/{projectId}/tasks/{TaskName.School}");

            getProjectByTaskResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var schoolTask = await getProjectByTaskResponse.Content.ReadResponseFromWrapper<GetProjectByTaskResponse>();

            schoolTask.School.EstablishmentType.Should().Be(EstablishmentType.NotSet);
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
