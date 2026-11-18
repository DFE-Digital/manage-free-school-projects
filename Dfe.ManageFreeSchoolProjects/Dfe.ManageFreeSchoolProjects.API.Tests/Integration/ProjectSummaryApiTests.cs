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

            result.Paging.Should().NotBeNull();
            result.Paging.RecordCount.Should().Be(1);
            result.Paging.Page.Should().Be(1);
        }

        [Fact]
        public async Task When_Get_WithPagination_Returns_RequestedPage_200()
        {
            var user = await CreateUser();

            using var context = _testFixture.GetContext();
            var projectOne = DatabaseModelBuilder.BuildProject();
            var projectTwo = DatabaseModelBuilder.BuildProject();

            projectOne.KeyContactsFsgLeadContactEmail = user.Email;
            projectTwo.KeyContactsFsgLeadContactEmail = user.Email;

            var dbUser = context.Users.First(u => u.Email == user.Email);
            dbUser.Projects.AddRange(projectOne, projectTwo);

            await context.SaveChangesAsync();

            var orderedProjects = new[] { projectOne, projectTwo }
                .OrderByDescending(p => p.ProjectStatusProvisionalOpeningDateAgreedWithTrust)
                .ThenBy(p => p.ProjectStatusCurrentFreeSchoolName)
                .ToList();

            var pageOneResponse = await _client.GetAsync($"/api/v1/summary/project?projectManagedByEmail={user.Email}&page=1&count=1");
            pageOneResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var pageOneResult = await pageOneResponse.Content.ReadFromJsonAsync<ApiListWrapper<GetProjectSummaryResponse>>();

            pageOneResult.Data.Should().HaveCount(1);
            pageOneResult.Data.First().ProjectId.Should().Be(orderedProjects[0].ProjectStatusProjectId);
            pageOneResult.Paging.RecordCount.Should().Be(2);
            pageOneResult.Paging.Page.Should().Be(1);
            pageOneResult.Paging.HasNext.Should().BeTrue();
            pageOneResult.Paging.TotalPages.Should().Be(2);

            var pageTwoResponse = await _client.GetAsync($"/api/v1/summary/project?projectManagedByEmail={user.Email}&page=2&count=1");
            pageTwoResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var pageTwoResult = await pageTwoResponse.Content.ReadFromJsonAsync<ApiListWrapper<GetProjectSummaryResponse>>();

            pageTwoResult.Data.Should().HaveCount(1);
            pageTwoResult.Data.First().ProjectId.Should().Be(orderedProjects[1].ProjectStatusProjectId);
            pageTwoResult.Paging.Page.Should().Be(2);
            pageTwoResult.Paging.HasNext.Should().BeFalse();
            pageTwoResult.Paging.HasPrevious.Should().BeTrue();
        }

        [Fact]
        public async Task When_Get_WithoutProjectManagedByEmail_Returns_400()
        {
            var response = await _client.GetAsync("/api/v1/summary/project");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task When_Get_WithEmptyProjectManagedByEmail_Returns_400()
        {
            var response = await _client.GetAsync("/api/v1/summary/project?projectManagedByEmail=");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task When_Get_WithUnmatchedEmail_Returns_EmptyPage_200()
        {
            var user = await CreateUser();

            using var context = _testFixture.GetContext();
            var project = DatabaseModelBuilder.BuildProject();
            project.KeyContactsFsgLeadContactEmail = "other.user@education.gov.uk";

            var dbUser = context.Users.First(u => u.Email == user.Email);
            dbUser.Projects.Add(project);

            await context.SaveChangesAsync();

            var response = await _client.GetAsync($"/api/v1/summary/project?projectManagedByEmail={user.Email}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ApiListWrapper<GetProjectSummaryResponse>>();

            result.Data.Should().BeEmpty();
            result.Paging.RecordCount.Should().Be(0);
        }

        [Fact]
        public async Task When_Get_WithCentralRoute_Returns_CentralRouteProjectType_200()
        {
            var user = await CreateUser();

            using var context = _testFixture.GetContext();
            var project = DatabaseModelBuilder.BuildProject();
            project.ProjectStatusFreeSchoolApplicationWave = "FS - Wave 14";
            project.Wave = "FS - Wave 14";
            project.KeyContactsFsgLeadContactEmail = user.Email;

            var dbUser = context.Users.First(u => u.Email == user.Email);
            dbUser.Projects.Add(project);

            await context.SaveChangesAsync();

            var response = await _client.GetAsync($"/api/v1/summary/project?projectManagedByEmail={user.Email}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ApiListWrapper<GetProjectSummaryResponse>>();

            result.Data.Should().HaveCount(1);
            result.Data.First().ProjectType.Should().Be("Central Route");
        }

        private async Task<CreateUserRequest> CreateUser()
        {
            var result = _autoFixture.Create<CreateUserRequest>();
            await _client.PostAsync($"/api/v1/client/users", result.ConvertToJson());

            return result;
        }
    }
}