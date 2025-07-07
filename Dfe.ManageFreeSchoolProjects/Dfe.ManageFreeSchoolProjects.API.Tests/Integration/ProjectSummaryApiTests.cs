using Dfe.ManageFreeSchoolProjects.API.Contracts.Dashboard;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Tests.Fixtures;
using Dfe.ManageFreeSchoolProjects.API.Tests.Helpers;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Summary;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Users;
using Dfe.ManageFreeSchoolProjects.API.Extensions;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Integration
{

    [Collection(ApiTestCollection.ApiTestCollectionName)]
    public class ProjectSummaryApiTests : ApiTestsBase
    {
        public ProjectSummaryApiTests(ApiTestFixture apiTestFixture) : base(apiTestFixture)
        {
        }

        [Fact]
        public async Task When_Get_Returns_DashboardFields_200()
        {
            var user = await CreateUser();

            using var context = _testFixture.GetContext();
            var project = DatabaseModelBuilder.BuildProject();

            var presumptionRoute = "FS - Presumption";
            project.ProjectStatusFreeSchoolApplicationWave = presumptionRoute;
            project.Wave = presumptionRoute;

            project.SchoolDetailsSchoolTypeMainstreamApEtc = "FS - AP";
            project.KeyContactsFsgLeadContactEmail = user.Email;

            var dbUser = context.Users.First(u => u.Email == user.Email);
            dbUser.Projects.Add(project);

            await context.SaveChangesAsync();

            var userDashboardResponse = await _client.GetAsync($"/api/v1/summary/project?projectManagedByEmail={user.Email}");
            userDashboardResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await userDashboardResponse.Content.ReadFromJsonAsync<ApiListWrapper<GetProjectSummaryResponse>>();

            result.Data.Should().HaveCount(1);

            var summary = result.Data.First();

            summary.ProjectId.Should().Be(project.ProjectStatusProjectId);
            summary.ProjectTitle.Should().Be(project.ProjectStatusCurrentFreeSchoolName);
            summary.TrustName.Should().Be(project.TrustName);
            summary.LocalAuthority.Should().Be(project.LocalAuthority);
            summary.Region.Should().Be(project.SchoolDetailsGeographicalRegion);
            summary.ProjectStatus.Should().Be(ProjectMapper.ToProjectStatusType(project.ProjectStatusProjectStatus).ToDescription());
            summary.ProjectType.Should().Be("Presumption");
            summary.RealisticOpeningYear.Should().Be(project.ProjectStatusRealisticYearOfOpening);
            summary.ProjectManagedBy.Should().Be(project.KeyContactsFsgLeadContact);
            summary.ProjectManagedByEmail.Should().Be(project.KeyContactsFsgLeadContactEmail);
            summary.SchoolType.Should().Be(SchoolType.AlternativeProvision.ToDescription());
            
        }

        private async Task<CreateUserRequest> CreateUser()
        {
            var result = _autoFixture.Create<CreateUserRequest>();
            await _client.PostAsync($"/api/v1/client/users", result.ConvertToJson());

            return result;
        }
    }
}