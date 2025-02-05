using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.Tests.Fixtures;
using Dfe.ManageFreeSchoolProjects.API.Tests.Helpers;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.Integration
{
    [Collection(ApiTestCollection.ApiTestCollectionName)]
    public class ProjectSiteApiTests : ApiTestsBase
    {
        public ProjectSiteApiTests(ApiTestFixture apiTestFixture) : base(apiTestFixture)
        {
        }

        [Fact]
        public async Task When_SitesDoNotExistCentral_Returns_200()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            await context.SaveChangesAsync();

            var getSiteInformationResponse =
                await _client.GetAsync($"/api/v1.0/client/projects/{projectId}/sites/central");
            getSiteInformationResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var siteInformationData = await getSiteInformationResponse.Content
                .ReadResponseFromWrapper<GetProjectSitesCentralResponse>();

            siteInformationData.TemporarySiteAddress.Should().BeNull();
            siteInformationData.TemporarySitePostcode.Should().BeNull();
            siteInformationData.TemporaryRagRating.Should().BeNull();
            siteInformationData.TemporarySitePlanningDecision.Should().BeNull();

            siteInformationData.HoTsAgreedForTemporarySiteForecast.Should().BeNull();
            siteInformationData.DateOfHoTSecuredOnTemporaryAccommodationSiteIfRequired.Should().BeNull();

            siteInformationData.ContractorForTemporarySiteAppointedForecast.Should().BeNull();
            siteInformationData.ContractorForTemporarySiteAppointedActual.Should().BeNull();

            siteInformationData.DateOfPlanningDecisionForTemporarySiteMainPlanningRecordForecast.Should().BeNull();
            siteInformationData.DateOfPlanningDecisionForTemporarySiteMainPlanningRecordActual.Should().BeNull();

            siteInformationData.TemporaryAccommodationFirstReadyForOccupationForecast.Should().BeNull();
            siteInformationData.TemporaryAccommodationFirstReadyForOccupationActual.Should().BeNull();


            siteInformationData.MainSiteAddress.Should().BeNull();
            siteInformationData.PostcodeOfSite.Should().BeNull();
            siteInformationData.PlanningRisk.Should().BeNull();
            siteInformationData.PlanningDecision.Should().BeNull();

            siteInformationData.HoTsAgreedForSiteForMainSchoolBuildingForecast.Should().BeNull();
            siteInformationData.HoTAgreedForSiteForMainSchoolBuildingActual.Should().BeNull();

            siteInformationData.ContractorForSiteForMainSchoolBuildingAppointedForecast.Should().BeNull();
            siteInformationData.ContractorForSiteForMainSchoolBuildingAppointedActual.Should().BeNull();

            siteInformationData.DateOfPlanningDecisionForMainSiteMainPlanningRecordForecast.Should().BeNull();
            siteInformationData.DateOfPlanningDecisionForMainSiteMainPlanningRecordActual.Should().BeNull();

            siteInformationData.MainSchoolBuildingFirstReadyForOccupationForecast.Should().BeNull();
            siteInformationData.MainSchoolBuildingFirstReadyForOccupationActual.Should().BeNull();

            siteInformationData.CapitalProjectRag.Should().BeNull();
            siteInformationData.CapitalProjectRagRatingCommentary.Should().BeNull();
        }

        [Fact]
        public async Task When_SitesExistCentral_Returns_200()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);

            var siteInformation = DatabaseModelBuilder.BuildSiteInformation(project.ProjectStatusProjectId);
            context.ConstructData.Add(siteInformation);

            await context.SaveChangesAsync();

            var getSiteInformationResponse =
                await _client.GetAsync($"/api/v1.0/client/projects/{projectId}/sites/central");
            getSiteInformationResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var siteInformationData = await getSiteInformationResponse.Content
                .ReadResponseFromWrapper<GetProjectSitesCentralResponse>();

            siteInformationData.TemporarySiteAddress.Should().Be(siteInformation.TemporarySiteAddress);
            siteInformationData.TemporarySitePostcode.Should().Be(siteInformation.TemporarySitePostcode);
            siteInformationData.TemporaryRagRating.Should().Be(siteInformation.TemporaryRagRating);
            siteInformationData.TemporarySitePlanningDecision.Should().Be(siteInformation.TemporarySitePlanningDecision);

            siteInformationData.HoTsAgreedForTemporarySiteForecast.Should().Be(siteInformation.HoTsAgreedForTemporarySiteForecast);
            siteInformationData.DateOfHoTSecuredOnTemporaryAccommodationSiteIfRequired.Should().Be(siteInformation.DateOfHoTSecuredOnTemporaryAccommodationSiteIfRequired);

            siteInformationData.ContractorForTemporarySiteAppointedForecast.Should().Be(siteInformation.ContractorForTemporarySiteAppointedForecast);
            siteInformationData.ContractorForTemporarySiteAppointedActual.Should().Be(siteInformation.ContractorForTemporarySiteAppointedActual);

            siteInformationData.DateOfPlanningDecisionForTemporarySiteMainPlanningRecordForecast.Should().Be(siteInformation.DateOfPlanningDecisionForTemporarySiteMainPlanningRecordForecast);
            siteInformationData.DateOfPlanningDecisionForTemporarySiteMainPlanningRecordActual.Should().Be(siteInformation.DateOfPlanningDecisionForTemporarySiteMainPlanningRecordActual);

            siteInformationData.TemporaryAccommodationFirstReadyForOccupationForecast.Should().Be(siteInformation.TemporaryAccommodationFirstReadyForOccupationForecast);
            siteInformationData.TemporaryAccommodationFirstReadyForOccupationActual.Should().Be(siteInformation.TemporaryAccommodationFirstReadyForOccupationActual);


            siteInformationData.MainSiteAddress.Should().Be(siteInformation.MainSiteAddress);
            siteInformationData.PostcodeOfSite.Should().Be(siteInformation.PostcodeOfSite);
            siteInformationData.PlanningRisk.Should().Be(siteInformation.PlanningRisk);
            siteInformationData.PlanningDecision.Should().Be(siteInformation.PlanningDecision);

            siteInformationData.HoTsAgreedForSiteForMainSchoolBuildingForecast.Should().Be(siteInformation.HoTsAgreedForSiteForMainSchoolBuildingForecast);
            siteInformationData.HoTAgreedForSiteForMainSchoolBuildingActual.Should().Be(siteInformation.HoTAgreedForSiteForMainSchoolBuildingActual);

            siteInformationData.ContractorForSiteForMainSchoolBuildingAppointedForecast.Should().Be(siteInformation.ContractorForSiteForMainSchoolBuildingAppointedForecast);
            siteInformationData.ContractorForSiteForMainSchoolBuildingAppointedActual.Should().Be(siteInformation.ContractorForSiteForMainSchoolBuildingAppointedActual);

            siteInformationData.DateOfPlanningDecisionForMainSiteMainPlanningRecordForecast.Should().Be(siteInformation.DateOfPlanningDecisionForMainSiteMainPlanningRecordForecast);
            siteInformationData.DateOfPlanningDecisionForMainSiteMainPlanningRecordActual.Should().Be(siteInformation.DateOfPlanningDecisionForMainSiteMainPlanningRecordActual);

            siteInformationData.MainSchoolBuildingFirstReadyForOccupationForecast.Should().Be(siteInformation.MainSchoolBuildingFirstReadyForOccupationForecast);
            siteInformationData.MainSchoolBuildingFirstReadyForOccupationActual.Should().Be(siteInformation.MainSchoolBuildingFirstReadyForOccupationActual);

            siteInformationData.CapitalProjectRag.Should().Be(siteInformation.CapitalProjectRag);
            siteInformationData.CapitalProjectRagRatingCommentary.Should().Be(siteInformation.CapitalProjectRagRatingCommentary);
        }

        [Fact]
        public async Task When_Get_ProjectDoesNotExistCentral_Returns_404()
        {
            var projectId = Guid.NewGuid().ToString();

            var getProjectSitesResponse = await _client.GetAsync($"/api/v1/client/projects/{projectId}/sites/central");
            getProjectSitesResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_SitesDoNotExistPresumption_Returns_200()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            project.ProjectStatusFreeSchoolApplicationWave = DatabaseModelBuilder.CreateProjectWave(ProjectType.PresumptionRoute);

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);
            await context.SaveChangesAsync();

            var updatePermanentSiteRequest = _autoFixture.Create<UpdateProjectSitePresumptionRequest>();
            var updatePermanentSiteResponse = await _client.PatchAsync($"/api/v1/client/projects/{projectId}/sites/presumption/permanent", updatePermanentSiteRequest.ConvertToJson());
            updatePermanentSiteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updateTemporarySiteRequest = _autoFixture.Create<UpdateProjectSitePresumptionRequest>();
            var updateTemporarySiteResponse = await _client.PatchAsync($"/api/v1/client/projects/{projectId}/sites/presumption/temporary", updateTemporarySiteRequest.ConvertToJson());
            updateTemporarySiteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getProjectSitesResponse = await _client.GetAsync($"/api/v1/client/projects/{projectId}/sites/presumption");
            getProjectSitesResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await getProjectSitesResponse.Content.ReadFromJsonAsync<ApiSingleResponseV2<GetProjectSitesPresumptionResponse>>();

            var actualPermanentSite = content.Data.PermanentSite;
            var actualTemporarySite = content.Data.TemporarySite;

            AssertionHelper.AssertProjectSite(actualPermanentSite, updatePermanentSiteRequest);
            AssertionHelper.AssertProjectSite(actualTemporarySite, updateTemporarySiteRequest);

            content.Data.SchoolName.Should().Be(project.ProjectStatusCurrentFreeSchoolName);
            content.Data.ProjectType.Should().Be("Presumption");
        }

        [Fact]
        public async Task When_SitesExistPresumption_Returns_200()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            project.ProjectStatusFreeSchoolApplicationWave = DatabaseModelBuilder.CreateProjectWave(ProjectType.PresumptionRoute);

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);
            await context.SaveChangesAsync();

            await SetSites(projectId);

            var updatePermanentSiteRequest = _autoFixture.Create<UpdateProjectSitePresumptionRequest>();
            var updatePermanentSiteResponse = await _client.PatchAsync($"/api/v1/client/projects/{projectId}/sites/presumption/permanent", updatePermanentSiteRequest.ConvertToJson());
            updatePermanentSiteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updateTemporarySiteRequest = _autoFixture.Create<UpdateProjectSitePresumptionRequest>();
            var updateTemporarySiteResponse = await _client.PatchAsync($"/api/v1/client/projects/{projectId}/sites/presumption/temporary", updateTemporarySiteRequest.ConvertToJson());
            updateTemporarySiteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getProjectSitesResponse = await _client.GetAsync($"/api/v1/client/projects/{projectId}/sites/presumption");
            getProjectSitesResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await getProjectSitesResponse.Content.ReadFromJsonAsync<ApiSingleResponseV2<GetProjectSitesPresumptionResponse>>();

            var actualPermanentSite = content.Data.PermanentSite;
            var actualTemporarySite = content.Data.TemporarySite;

            AssertionHelper.AssertProjectSite(actualPermanentSite, updatePermanentSiteRequest);
            AssertionHelper.AssertProjectSite(actualTemporarySite, updateTemporarySiteRequest);

            content.Data.ProjectType.Should().Be("Presumption");
        }

        [Fact]
        public async Task When_SiteNotConfiguredPresumption_Returns_200()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;

            project.ProjectStatusFreeSchoolApplicationWave = DatabaseModelBuilder.CreateProjectWave(ProjectType.PresumptionRoute);

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);
            await context.SaveChangesAsync();

            var getProjectSitesResponse = await _client.GetAsync($"/api/v1/client/projects/{projectId}/sites/presumption");
            getProjectSitesResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await getProjectSitesResponse.Content.ReadFromJsonAsync<ApiSingleResponseV2<GetProjectSitesPresumptionResponse>>();

            AssertBlankSite(content.Data.PermanentSite);
            AssertBlankSite(content.Data.TemporarySite);

            content.Data.ProjectType.Should().Be("Presumption");
        }

        [Fact]
        public async Task When_Get_ProjectDoesNotExistPresumption_Returns_404()
        {
            var projectId = Guid.NewGuid().ToString();

            var getProjectSitesResponse = await _client.GetAsync($"/api/v1/client/projects/{projectId}/sites/presumption");
            getProjectSitesResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_Patch_ProjectDoesNotExistPresumption_Returns_404()
        {
            var projectId = Guid.NewGuid().ToString();
            var updateSiteRequest = _autoFixture.Create<UpdateProjectSitePresumptionRequest>();

            var updateSiteResponse = await _client.PatchAsync($"/api/v1/client/projects/{projectId}/sites/presumption/permanent", updateSiteRequest.ConvertToJson());
            updateSiteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task When_Patch_SiteTypeInvalidPresumption_Returns_400()
        {
            var project = DatabaseModelBuilder.BuildProject();
            var projectId = project.ProjectStatusProjectId;
            var updateSiteRequest = _autoFixture.Create<UpdateProjectSitePresumptionRequest>();

            using var context = _testFixture.GetContext();
            context.Kpi.Add(project);
            await context.SaveChangesAsync();

            var updateSiteResponse = await _client.PatchAsync($"/api/v1/client/projects/{projectId}/sites/presumption/invalid", updateSiteRequest.ConvertToJson());
            updateSiteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private async Task SetSites(string projectId)
        {
            var setPermanentSiteRequest = _autoFixture.Create<UpdateProjectSitePresumptionRequest>();
            var setPermanentSiteResponse = await _client.PatchAsync($"/api/v1/client/projects/{projectId}/sites/presumption/permanent", setPermanentSiteRequest.ConvertToJson());
            setPermanentSiteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var setTemporarySiteRequest = _autoFixture.Create<UpdateProjectSitePresumptionRequest>();
            var setTemporarySiteResponse = await _client.PatchAsync($"/api/v1/client/projects/{projectId}/sites/presumption/temporary", setTemporarySiteRequest.ConvertToJson());
            setTemporarySiteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private static void AssertBlankSite(ProjectSite actual)
        {
            actual.Address.AddressLine1.Should().BeNull();
            actual.Address.AddressLine2.Should().BeNull();
            actual.Address.Postcode.Should().BeNull();
            actual.Address.TownOrCity.Should().BeNull();
            actual.StartDateOfSiteOccupation.Should().BeNull();
        }
    }
}
