using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Sites;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.SiteInformation;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.SiteInformation
{
    public class EditSiteInformationModelTests
    {
        private const string ProjectId = "FS-1";

        private readonly IGetProjectSitesService _getProjectSitesService = Substitute.For<IGetProjectSitesService>();
        private readonly IUpdateProjectSiteService _updateProjectSiteService = Substitute.For<IUpdateProjectSiteService>();

        private UpdateProjectSiteRequest? _capturedRequest;

        public EditSiteInformationModelTests()
        {
            _updateProjectSiteService
                .When(service => service.Execute(Arg.Any<string>(), Arg.Any<UpdateProjectSiteRequest>(), Arg.Any<ProjectSiteType>()))
                .Do(call => _capturedRequest = call.Arg<UpdateProjectSiteRequest>());
        }

        [Theory]
        [InlineData(SiteInformationWave.NewSchool, "New School")]
        [InlineData(SiteInformationWave.Presumption, "Presumption")]
        public async Task OnGet_PopulatesThePermanentSite(string wave, string projectType)
        {
            var sites = BuildSites(projectType);
            ReturnsSites(wave, sites);
            var model = BuildModel(wave, ProjectSiteType.Permanent);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            AssertModelMatchesSite(model, sites.PermanentSite);
            model.SchoolName.Should().Be(sites.SchoolName);
            model.ProjectType.Should().Be(projectType);
        }

        [Fact]
        public async Task OnGet_PopulatesTheTemporarySite()
        {
            var sites = BuildSites("New School");
            ReturnsSites(SiteInformationWave.NewSchool, sites);
            var model = BuildModel(SiteInformationWave.NewSchool, ProjectSiteType.Temporary);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
            AssertModelMatchesSite(model, sites.TemporarySite);
        }

        [Fact]
        public async Task OnGet_MatchesTheWaveIgnoringCase()
        {
            ReturnsSites("NewSchool", BuildSites("New School"));
            var model = BuildModel("NewSchool", ProjectSiteType.Permanent);

            var result = await model.OnGet();

            result.Should().BeOfType<PageResult>();
        }

        [Theory]
        [InlineData(SiteInformationWave.Presumption, "Central Route")]
        [InlineData(SiteInformationWave.NewSchool, "Presumption")]
        [InlineData(SiteInformationWave.Presumption, "New School")]
        public async Task OnGet_WhenTheProjectIsNotForThisWave_ReturnsNotFound(string wave, string projectType)
        {
            ReturnsSites(wave, BuildSites(projectType));
            var model = BuildModel(wave, ProjectSiteType.Permanent);

            var result = await model.OnGet();

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task OnGet_WithAnInvalidSiteType_Throws()
        {
            ReturnsSites(SiteInformationWave.NewSchool, BuildSites("New School"));
            var model = BuildModel(SiteInformationWave.NewSchool, (ProjectSiteType)99);

            var act = () => model.OnGet();

            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(SiteInformationWave.NewSchool, ProjectSiteType.Permanent)]
        [InlineData(SiteInformationWave.Presumption, ProjectSiteType.Temporary)]
        public async Task OnPost_UpdatesTheSiteAndRedirectsToTheWavesSiteInformation(string wave, ProjectSiteType siteType)
        {
            var model = BuildModel(wave, siteType);
            model.AddressLine1 = "1 Test Street";
            model.AddressLine2 = "Test Area";
            model.TownOrCity = "Test Town";
            model.Postcode = "AB1 2CD";
            model.DatePlanningPermissionObtained = new DateTime(2025, 1, 2);
            model.StartDateOfSiteOccupation = new DateTime(2026, 3, 4);

            var result = await model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(string.Format(RouteConstants.ViewSiteInformation, ProjectId, wave));

            await _updateProjectSiteService.Received(1).Execute(ProjectId, Arg.Any<UpdateProjectSiteRequest>(), siteType);
            _capturedRequest!.Address.AddressLine1.Should().Be("1 Test Street");
            _capturedRequest.Address.AddressLine2.Should().Be("Test Area");
            _capturedRequest.Address.TownOrCity.Should().Be("Test Town");
            _capturedRequest.Address.Postcode.Should().Be("AB1 2CD");
            _capturedRequest.DatePlanningPermissionObtained.Should().Be(new DateTime(2025, 1, 2));
            _capturedRequest.StartDateOfSiteOccupation.Should().Be(new DateTime(2026, 3, 4));
        }

        [Fact]
        public async Task OnPost_WhenValidationFails_DoesNotUpdate()
        {
            var model = BuildModel(SiteInformationWave.NewSchool, ProjectSiteType.Permanent);
            model.ModelState.AddModelError("postcode", "Postcode must be 10 characters or less");

            var result = await model.OnPost();

            result.Should().BeOfType<PageResult>();
            await _updateProjectSiteService.DidNotReceiveWithAnyArgs().Execute(default!, default!, default);
        }

        private static void AssertModelMatchesSite(EditSiteInformationModel model, ProjectSite site)
        {
            model.AddressLine1.Should().Be(site.Address.AddressLine1);
            model.AddressLine2.Should().Be(site.Address.AddressLine2);
            model.TownOrCity.Should().Be(site.Address.TownOrCity);
            model.Postcode.Should().Be(site.Address.Postcode);
            model.DatePlanningPermissionObtained.Should().Be(site.DatePlanningPermissionObtained);
            model.StartDateOfSiteOccupation.Should().Be(site.StartDateOfSiteOccupation);
        }

        private static GetProjectSitesResponse BuildSites(string projectType)
        {
            var fixture = new Fixture();
            var sites = fixture.Create<GetProjectSitesResponse>();
            sites.ProjectType = projectType;
            return sites;
        }

        private void ReturnsSites(string wave, GetProjectSitesResponse sites)
        {
            _getProjectSitesService.Execute(ProjectId, wave).Returns(sites);
        }

        private EditSiteInformationModel BuildModel(string wave, ProjectSiteType siteType)
        {
            return new EditSiteInformationModel(_getProjectSitesService, _updateProjectSiteService, new ErrorService())
            {
                ProjectId = ProjectId,
                Wave = wave,
                SiteType = siteType,
                PageContext = BuildPageContext()
            };
        }

        private static PageContext BuildPageContext()
        {
            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new PageActionDescriptor(),
                new ModelStateDictionary());

            return new PageContext(actionContext);
        }
    }
}
