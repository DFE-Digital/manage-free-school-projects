using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Tests.Fixtures;
using Dfe.ManageFreeSchoolProjects.API.Tests.Helpers;
using Dfe.ManageFreeSchoolProjects.API.Tests.Utils;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Task;
using DocumentFormat.OpenXml.Spreadsheet;
using Kpi = Dfe.ManageFreeSchoolProjects.Data.Entities.Existing.Kpi;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Integration
{
    [Collection(ApiTestCollection.ApiTestCollectionName)]
    public class GetProjectTaskSummaryApiTests : ApiTestsBase
    {
        public GetProjectTaskSummaryApiTests(ApiTestFixture apiTestFixture) : base(apiTestFixture)
        {
        }

        [Fact]
        public async Task GetProjectTaskList_Returns_200()
        {
            using var context = _testFixture.GetContext();
            
            var project = DatabaseModelBuilder.BuildProject();
            context.Kpi.Add(project);
            
            var tasks = TasksStub.BuildListOfTasks(project.Rid);
            context.Tasks.AddRange(tasks);
            
            await context.SaveChangesAsync();

            var taskListResponse = await _client.GetAsync($"/api/v1/client/projects/{project.ProjectStatusProjectId}/tasks/summary");
            taskListResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await taskListResponse.Content.ReadFromJsonAsync<ApiSingleResponseV2<ProjectByTaskSummaryResponse>>();

            var result = content.Data;

            result.School.Name.Should().Be("School");
            result.School.Status.Should().Be(ProjectTaskStatus.NotStarted);
            result.Dates.Name.Should().Be("Dates");
            result.Dates.Status.Should().Be(ProjectTaskStatus.NotStarted);
            result.Trust.Name.Should().Be("Trust");
            result.Trust.Status.Should().Be(ProjectTaskStatus.NotStarted);
            result.Constituency.Name.Should().Be("Constituency");
            result.Constituency.Status.Should().Be(ProjectTaskStatus.NotStarted);
            result.KickOffMeeting.Name.Should().Be("KickOffMeeting");
            result.KickOffMeeting.Status.Should().Be(ProjectTaskStatus.NotStarted);
            result.FundingAgreement.Name.Should().Be("FundingAgreement");
            result.FundingAgreement.Status.Should().Be(ProjectTaskStatus.NotStarted);
            result.StatutoryConsultation.Name.Should().Be("StatutoryConsultation");
            result.StatutoryConsultation.Status.Should().Be(ProjectTaskStatus.NotStarted);
            result.ArticlesOfAssociation.Name.Should().Be("ArticlesOfAssociation");
            result.ArticlesOfAssociation.Status.Should().Be(ProjectTaskStatus.NotStarted);
            result.FinancePlan.Name.Should().Be("FinancePlan");
            result.FinancePlan.Status.Should().Be(ProjectTaskStatus.NotStarted);

            result.GovernancePlan.Name.Should().Be("GovernancePlan");
            result.GovernancePlan.Status.Should().Be(ProjectTaskStatus.NotStarted);

            result.AdmissionsArrangements.Name.Should().Be(TaskName.AdmissionsArrangements.ToString());
            result.AdmissionsArrangements.Status.Should().Be(ProjectTaskStatus.NotStarted);

            result.EqualitiesAssessment.Name.Should().Be("EqualitiesAssessment");
            result.EqualitiesAssessment.Status.Should().Be(ProjectTaskStatus.NotStarted);

            result.PDG.Name.Should().Be(TaskName.PDG.ToString());
            result.PDG.Status.Should().Be(ProjectTaskStatus.NotStarted);

            result.FinalFinancePlan.Name.Should().Be(TaskName.FinalFinancePlan.ToString());
            result.FinalFinancePlan.Status.Should().Be(ProjectTaskStatus.NotStarted);

            result.ApplicationsEvidence.Name.Should().Be(TaskName.ApplicationsEvidence.ToString());
            result.ApplicationsEvidence.Status.Should().Be(ProjectTaskStatus.NotStarted);
            result.ApplicationsEvidence.IsHidden.Should().BeFalse();
        }

        [Fact]
        public async Task GetProjectTaskList_ApplicationsEvidence_HiddenWithSchoolType_Returns_200()
        {
            using var context = _testFixture.GetContext();
            
            var project = DatabaseModelBuilder.BuildProject();
            project.SchoolDetailsSchoolTypeMainstreamApEtc = "FS - AP";

            context.Kpi.Add(project);

            await context.SaveChangesAsync();

            var taskListResponse = await _client.GetAsync($"/api/v1/client/projects/{project.ProjectStatusProjectId}/tasks/summary");
            taskListResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await taskListResponse.Content.ReadFromJsonAsync<ApiSingleResponseV2<ProjectByTaskSummaryResponse>>();

            var result = content.Data;

            result.ApplicationsEvidence.Name.Should().Be(TaskName.ApplicationsEvidence.ToString());
            result.ApplicationsEvidence.Status.Should().Be(ProjectTaskStatus.NotStarted);
            result.ApplicationsEvidence.IsHidden.Should().BeTrue();
        }
        
        /// <summary>
        /// A New School project stores the school type without the historic "FS - " prefix,
        /// so the task list has to resolve the unprefixed value to hide the task.
        /// </summary>
        [Theory]
        [InlineData("AP", true)]
        [InlineData("Special", true)]
        [InlineData("Mainstream", false)]
        public async Task GetProjectTaskList_NewSchoolWave_ApplicationsEvidence_IsHiddenForSchoolType_Returns_200(
            string storedSchoolType, bool expectedIsHidden)
        {
            using var context = _testFixture.GetContext();

            var project = DatabaseModelBuilder.BuildProject();
            project.ProjectStatusFreeSchoolApplicationWave =
                DatabaseModelBuilder.CreateProjectWave(ProjectType.NewSchool);
            project.SchoolDetailsSchoolTypeMainstreamApEtc = storedSchoolType;

            context.Kpi.Add(project);

            await context.SaveChangesAsync();

            var taskListResponse = await _client.GetAsync($"/api/v1/client/projects/{project.ProjectStatusProjectId}/tasks/summary");
            taskListResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await taskListResponse.Content.ReadFromJsonAsync<ApiSingleResponseV2<ProjectByTaskSummaryResponse>>();

            content.Data.ApplicationsEvidence.IsHidden.Should().Be(expectedIsHidden);
        }

        [Fact]
        public async Task GetProjectTaskList_HiddenApplicationsEvidence_ReturnsLowerTaskCount()
        {
            var project = await setUpProject(false);

            var taskListResponse = await _client.GetAsync($"/api/v1/client/projects/{project.ProjectStatusProjectId}/tasks/summary");
            taskListResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await taskListResponse.Content.ReadFromJsonAsync<ApiSingleResponseV2<ProjectByTaskSummaryResponse>>();

            var result = content.Data;
            
            var taskCount = result.TaskCount;
            
            var projectWithAp = await setUpProject(true);

            var taskListResponseWithAp = await _client.GetAsync($"/api/v1/client/projects/{projectWithAp.ProjectStatusProjectId}/tasks/summary");
            taskListResponseWithAp.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var contentWithAp = await taskListResponseWithAp.Content.ReadFromJsonAsync<ApiSingleResponseV2<ProjectByTaskSummaryResponse>>();
            
            var resultWithAp = contentWithAp.Data;

            resultWithAp.TaskCount.Should().Be(taskCount - 1);
        }

        [InlineData("School", ProjectTaskStatus.Completed)]
        [Theory]
        public async Task GetProjectTaskList_CompletedTaskCountIsCorrect(string expectedTaskName, ProjectTaskStatus expectedProjectTaskStatus)
        {
            var project =  await setUpProject(false);
            
            var updateTaskStatusRequest = new UpdateTaskStatusRequest
            {
                ProjectTaskStatus = expectedProjectTaskStatus, TaskName = expectedTaskName
            };
        
            var taskUpdateResponse =
                await _client.PatchAsync($"/api/v1/{project.ProjectStatusProjectId}/task/status", updateTaskStatusRequest.ConvertToJson());
            taskUpdateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var taskStatusResponse =
                await _client.GetAsync($"/api/v1/{project.ProjectStatusProjectId}/task/status?taskName={expectedTaskName}");
            taskStatusResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var taskListResponse = await _client.GetAsync($"/api/v1/client/projects/{project.ProjectStatusProjectId}/tasks/summary");
            
            var content = await taskListResponse.Content.ReadFromJsonAsync<ApiSingleResponseV2<ProjectByTaskSummaryResponse>>();
            
            var result = content.Data;
            result.CompletedTasks.Should().Be(1);
        }


        [Fact]
        public async Task Get_ProjectTaskList_ProjectDoesNotExist_Returns_404()
        {
            var taskListResponse = await _client.GetAsync($"/api/v1/client/projects/NotExist/tasks/summary");
            taskListResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        public async Task<Kpi> setUpProject(bool schoolIsAp)
        {
            using var context = _testFixture.GetContext();
            
            var project = DatabaseModelBuilder.BuildProject();
            
            if (schoolIsAp)
            {
                project.SchoolDetailsSchoolTypeMainstreamApEtc = "FS - AP";
            }
            
            context.Kpi.Add(project);
            
            await context.SaveChangesAsync();

            return project;
        }
    }
}
