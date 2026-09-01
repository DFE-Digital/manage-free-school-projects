using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Create;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Create
{
    public class MethodModelTests
    {
        [Fact]
        public void OnGet_WhenUserIsNotProjectRecordCreator_ReturnsUnauthorized()
        {
            var model = BuildModel(new CreateProjectCacheItem(), out _, authorised: false);

            var result = model.OnGet();

            result.Should().BeOfType<UnauthorizedResult>();
        }

        /// <summary>
        /// The method page is the entry point to the create journey, so it pre-selects whatever is
        /// already cached and then resets the session ready for the answers about to be given.
        /// </summary>
        [Fact]
        public void OnGet_PreSelectsTheMethodAlreadyInTheCacheAndResetsTheSession()
        {
            var cacheItem = new CreateProjectCacheItem { ProjectType = ProjectType.CentralRoute };
            var model = BuildModel(cacheItem, out var cache);

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.Method.Should().Be(ProjectType.CentralRoute);
            cache.Received().Delete();
        }

        [Fact]
        public void OnPost_WhenMethodNotSelected_ReturnsPageWithErrors()
        {
            var model = BuildModel(new CreateProjectCacheItem(), out var cache);
            model.ModelState.AddModelError("method", "Select what project you want to create");

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            cache.DidNotReceive().Update(Arg.Any<CreateProjectCacheItem>());
        }

        [Theory]
        [InlineData(ProjectType.PresumptionRoute, false, RouteConstants.CreateProjectId)]
        [InlineData(ProjectType.PresumptionRoute, true, RouteConstants.CreateProjectCheckYourAnswers)]
        [InlineData(ProjectType.NewSchool, false, RouteConstants.CreateProjectId)]
        [InlineData(ProjectType.NewSchool, true, RouteConstants.CreateProjectCheckYourAnswers)]
        [InlineData(ProjectType.CentralRoute, false, RouteConstants.CreateApplicationNumber)]
        public void OnPost_RedirectsForChosenMethod(
            ProjectType method, bool reachedCheckYourAnswers, string expectedRoute)
        {
            var cacheItem = new CreateProjectCacheItem { ReachedCheckYourAnswers = reachedCheckYourAnswers };
            var model = BuildModel(cacheItem, out var cache);
            model.Method = method;

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(expectedRoute);
            cacheItem.ProjectType.Should().Be(method);
            cache.Received().Update(cacheItem);
        }

        /// <summary>
        /// The central route collects an application number and wave. Returning to it from check your
        /// answers only skips those pages when the answers are already there to go back to.
        /// </summary>
        [Theory]
        [InlineData("Wave 15", null, RouteConstants.CreateProjectCheckYourAnswers)]
        [InlineData(null, "APP123", RouteConstants.CreateProjectCheckYourAnswers)]
        [InlineData(null, null, RouteConstants.CreateApplicationNumber)]
        [InlineData("", "", RouteConstants.CreateApplicationNumber)]
        public void OnPost_WhenCentralRouteFromCheckYourAnswers_RedirectsOnApplicationDetails(
            string? applicationWave, string? applicationNumber, string expectedRoute)
        {
            var cacheItem = new CreateProjectCacheItem
            {
                ReachedCheckYourAnswers = true,
                ApplicationWave = applicationWave,
                ApplicationNumber = applicationNumber
            };
            var model = BuildModel(cacheItem, out _);
            model.Method = ProjectType.CentralRoute;

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(expectedRoute);
        }

        [Fact]
        public void OnPost_WhenSwitchingToPresumptionRoute_ClearsCentralRouteAnswers()
        {
            var cacheItem = new CreateProjectCacheItem
            {
                ProjectType = ProjectType.CentralRoute,
                ApplicationWave = "Wave 15",
                ApplicationNumber = "APP123"
            };
            var model = BuildModel(cacheItem, out _);
            model.Method = ProjectType.PresumptionRoute;

            model.OnPost();

            cacheItem.ApplicationWave.Should().BeNull();
            cacheItem.ApplicationNumber.Should().BeNull();
            cacheItem.ProjectType.Should().Be(ProjectType.PresumptionRoute);
        }

        [Fact]
        public void OnPost_WhenMethodIsNotSet_Throws()
        {
            var model = BuildModel(new CreateProjectCacheItem(), out _);
            model.Method = ProjectType.NotSet;

            model.Invoking(m => m.OnPost())
                .Should().Throw<InvalidOperationException>()
                .WithMessage("Unrecognized method NotSet");
        }

        private static MethodModel BuildModel(
            CreateProjectCacheItem cacheItem, out ICreateProjectCache cache, bool authorised = true)
        {
            cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            return new MethodModel(new ErrorService(), cache)
            {
                PageContext = CreatePageTestContext.Build(authorised)
            };
        }
    }
}
