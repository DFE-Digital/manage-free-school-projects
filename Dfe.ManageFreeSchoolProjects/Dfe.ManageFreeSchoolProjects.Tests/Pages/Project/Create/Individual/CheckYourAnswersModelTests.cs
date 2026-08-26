using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Create.Individual
{
    public class CheckYourAnswersModelTests
    {
        [Fact]
        public void OnGet_WhenUserIsNotProjectRecordCreator_ReturnsUnauthorized()
        {
            var model = BuildModel(BuildCacheItem(ProjectType.NewSchool), authorised: false);

            var result = model.OnGet();

            result.Should().BeOfType<UnauthorizedResult>();
        }

        /// <summary>
        /// Reaching this page is what unlocks the "change answer" journeys, so the flag has to be
        /// written back to the cache rather than only held on the model.
        /// </summary>
        [Fact]
        public void OnGet_MarksTheProjectAsHavingReachedCheckYourAnswers()
        {
            var cacheItem = BuildCacheItem(ProjectType.NewSchool);
            cacheItem.SchoolType = SchoolType.Mainstream;
            var model = BuildModel(cacheItem, out var cache, out _, out _);

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Project.Should().BeSameAs(cacheItem);
            cacheItem.ReachedCheckYourAnswers.Should().BeTrue();
            cache.Received(1).Update(cacheItem);
            model.IsLocalAuthority.Should().BeTrue();
            model.IsMainStream.Should().BeTrue();
        }

        [Theory]
        [InlineData(ProjectType.CentralRoute, SchoolType.Special, false, false)]
        [InlineData(ProjectType.NewSchool, SchoolType.Special, true, false)]
        [InlineData(ProjectType.PresumptionRoute, SchoolType.Mainstream, false, true)]
        public void OnGet_SetsFlagsForProjectAndSchoolType(
            ProjectType projectType, SchoolType schoolType, bool expectedLocalAuthority, bool expectedMainstream)
        {
            var cacheItem = BuildCacheItem(projectType);
            cacheItem.SchoolType = schoolType;
            var model = BuildModel(cacheItem);

            model.OnGet();

            model.IsLocalAuthority.Should().Be(expectedLocalAuthority);
            model.IsMainStream.Should().Be(expectedMainstream);
        }

        /// <summary>
        /// The application wave is derived from the project type for the routes that have a fixed
        /// wave, but the central route collects it from the user and must keep what they entered.
        /// </summary>
        [Theory]
        [InlineData(ProjectType.NewSchool, "Wave 15", "LocalAuthority")]
        [InlineData(ProjectType.PresumptionRoute, "Wave 15", "FS - Presumption")]
        [InlineData(ProjectType.CentralRoute, "Wave 15", "Wave 15")]
        public async Task OnPostAsync_SetsApplicationWaveForProjectType(
            ProjectType projectType, string cachedApplicationWave, string expectedApplicationWave)
        {
            var cacheItem = BuildCacheItem(projectType);
            cacheItem.ApplicationWave = cachedApplicationWave;

            var model = BuildModel(cacheItem, out _, out _, out var capturedRequest);

            await model.OnPostAsync();

            capturedRequest.Value!.Projects.Should().ContainSingle()
                .Which.ApplicationWave.Should().Be(expectedApplicationWave);
        }

        [Fact]
        public async Task OnPostAsync_SendsTheCachedAnswersToTheApi()
        {
            var cacheItem = BuildCacheItem(ProjectType.NewSchool);
            cacheItem.SchoolType = SchoolType.Mainstream;
            cacheItem.APResourcesProvision = 12;
            cacheItem.SENResourcedProvisionSENUnit = 34;
            cacheItem.ApplicationNumber = "APP123";

            var model = BuildModel(cacheItem, out _, out _, out var capturedRequest);

            await model.OnPostAsync();

            var project = capturedRequest.Value!.Projects.Should().ContainSingle().Subject;
            project.ProjectId.Should().Be("NEW-SCHOOL-1");
            project.SchoolName.Should().Be("Test School");
            project.SchoolType.Should().Be(SchoolType.Mainstream);
            project.Region.Should().Be("London");
            project.NurseryCapacity.Should().Be(0);
            project.YRY6Capacity.Should().Be(10);
            project.Y7Y11Capacity.Should().Be(20);
            project.Y12Y14Capacity.Should().Be(30);
            project.APResourcesProvision.Should().Be(12);
            project.SENResourcedProvisionSENUnit.Should().Be(34);
            project.ApplicationNumber.Should().Be("APP123");
        }

        /// <summary>
        /// The provision capacities are only collected for a new school mainstream project, so every
        /// other route has to send a value the API will accept rather than null.
        /// </summary>
        [Fact]
        public async Task OnPostAsync_WhenProvisionCapacitiesNotCollected_SendsZero()
        {
            var cacheItem = BuildCacheItem(ProjectType.PresumptionRoute);
            cacheItem.APResourcesProvision = null;
            cacheItem.SENResourcedProvisionSENUnit = null;
            cacheItem.ApplicationNumber = null;

            var model = BuildModel(cacheItem, out _, out _, out var capturedRequest);

            await model.OnPostAsync();

            var project = capturedRequest.Value!.Projects.Should().ContainSingle().Subject;
            project.APResourcesProvision.Should().Be(0);
            project.SENResourcedProvisionSENUnit.Should().Be(0);
            project.ApplicationNumber.Should().BeEmpty();
        }

        [Fact]
        public async Task OnPostAsync_WhenProjectCreated_NotifiesTheAssigneeAndRedirects()
        {
            var cacheItem = BuildCacheItem(ProjectType.NewSchool);
            var model = BuildModel(cacheItem, out _, out var notifyUserService, out _);

            var result = await model.OnPostAsync();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(RouteConstants.CreateProjectConfirmation);
            await notifyUserService.Received(1).Execute(
                "test@education.gov.uk",
                Arg.Is<string>(url => url.Contains("NEW-SCHOOL-1")));
        }

        [Fact]
        public async Task OnPostAsync_WhenProjectIdAlreadyExists_ReturnsPageWithError()
        {
            var cacheItem = BuildCacheItem(ProjectType.NewSchool);
            cacheItem.SchoolType = SchoolType.Mainstream;
            var errorService = new ErrorService();
            var model = BuildModel(cacheItem, errorService,
                new HttpRequestException("Duplicate", null, HttpStatusCode.UnprocessableEntity));

            var result = await model.OnPostAsync();

            result.Should().BeOfType<PageResult>();
            errorService.HasErrors().Should().BeTrue();
            model.Project.Should().BeSameAs(cacheItem);
            model.IsLocalAuthority.Should().BeTrue();
            model.IsMainStream.Should().BeTrue();
        }

        /// <summary>
        /// The API returns a 500 when the project was created but a downstream step failed, so the
        /// user is still sent to the confirmation page rather than shown an error.
        /// </summary>
        [Fact]
        public async Task OnPostAsync_WhenApiReturnsServerError_StillRedirectsToConfirmation()
        {
            var model = BuildModel(BuildCacheItem(ProjectType.NewSchool), new ErrorService(),
                new HttpRequestException("Server error", null, HttpStatusCode.InternalServerError));

            var result = await model.OnPostAsync();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(RouteConstants.CreateProjectConfirmation);
        }

        [Fact]
        public async Task OnPostAsync_WhenApiFailsForAnotherReason_Rethrows()
        {
            var model = BuildModel(BuildCacheItem(ProjectType.NewSchool), new ErrorService(),
                new HttpRequestException("Bad request", null, HttpStatusCode.BadRequest));

            await model.Invoking(m => m.OnPostAsync())
                .Should().ThrowAsync<HttpRequestException>()
                .WithMessage("Bad request");
        }

        private static CreateProjectCacheItem BuildCacheItem(ProjectType projectType)
        {
            return new CreateProjectCacheItem
            {
                ProjectType = projectType,
                ProjectId = "NEW-SCHOOL-1",
                SchoolName = "Test School",
                Region = ProjectRegion.London,
                NurseryCapacity = 0,
                YRY6Capacity = 10,
                Y7Y11Capacity = 20,
                Y12Y14Capacity = 30,
                ProjectAssignedToEmail = "test@education.gov.uk"
            };
        }

        private static CheckYourAnswersModel BuildModel(CreateProjectCacheItem cacheItem, bool authorised = true)
        {
            return BuildModel(cacheItem, out _, out _, out _, authorised);
        }

        private static CheckYourAnswersModel BuildModel(
            CreateProjectCacheItem cacheItem, ErrorService errorService, Exception apiFailure)
        {
            var createProjectService = Substitute.For<ICreateProjectService>();
            createProjectService.Execute(Arg.Any<CreateProjectRequest>()).ThrowsAsync(apiFailure);

            return BuildModel(cacheItem, errorService, createProjectService, Substitute.For<INotifyUserService>());
        }

        private static CheckYourAnswersModel BuildModel(
            CreateProjectCacheItem cacheItem,
            out ICreateProjectCache cache,
            out INotifyUserService notifyUserService,
            out CapturedRequest capturedRequest,
            bool authorised = true)
        {
            var captured = new CapturedRequest();
            capturedRequest = captured;

            var createProjectService = Substitute.For<ICreateProjectService>();
            createProjectService
                .Execute(Arg.Do<CreateProjectRequest>(request => captured.Value = request))
                .Returns(new CreateProjectResponse());

            notifyUserService = Substitute.For<INotifyUserService>();
            cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            return new CheckYourAnswersModel(new ErrorService(), cache, createProjectService, notifyUserService)
            {
                PageContext = CreatePageTestContext.Build(authorised)
            };
        }

        private static CheckYourAnswersModel BuildModel(
            CreateProjectCacheItem cacheItem,
            ErrorService errorService,
            ICreateProjectService createProjectService,
            INotifyUserService notifyUserService)
        {
            var cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            return new CheckYourAnswersModel(errorService, cache, createProjectService, notifyUserService)
            {
                PageContext = CreatePageTestContext.Build()
            };
        }

        private sealed class CapturedRequest
        {
            public CreateProjectRequest? Value { get; set; }
        }
    }
}
