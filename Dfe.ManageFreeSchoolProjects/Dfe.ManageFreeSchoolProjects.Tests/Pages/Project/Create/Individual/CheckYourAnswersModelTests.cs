using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.API.Contracts.RequestModels.Projects;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Create.Individual
{
    public class CheckYourAnswersModelTests
    {
        /// <summary>
        /// The application wave is derived from the project type for the routes that have a fixed
        /// wave, but the central route collects it from the user and must keep what they entered.
        /// </summary>
        [Theory]
        [InlineData(ProjectType.LocalAuthority, "Wave 15", "LocalAuthority")]
        [InlineData(ProjectType.PresumptionRoute, "Wave 15", "FS - Presumption")]
        [InlineData(ProjectType.CentralRoute, "Wave 15", "Wave 15")]
        public async Task OnPostAsync_SetsApplicationWaveForProjectType(
            ProjectType projectType, string cachedApplicationWave, string expectedApplicationWave)
        {
            var cacheItem = BuildCacheItem(projectType);
            cacheItem.ApplicationWave = cachedApplicationWave;

            var createProjectService = Substitute.For<ICreateProjectService>();
            CreateProjectRequest capturedRequest = null;
            createProjectService
                .Execute(Arg.Do<CreateProjectRequest>(request => capturedRequest = request))
                .Returns(new CreateProjectResponse());

            var model = BuildModel(cacheItem, createProjectService);

            await model.OnPostAsync();

            capturedRequest.Projects.Should().ContainSingle()
                .Which.ApplicationWave.Should().Be(expectedApplicationWave);
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

        private static CheckYourAnswersModel BuildModel(
            CreateProjectCacheItem cacheItem, ICreateProjectService createProjectService)
        {
            var cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            return new CheckYourAnswersModel(
                new ErrorService(),
                cache,
                createProjectService,
                Substitute.For<INotifyUserService>())
            {
                PageContext = CreatePageTestContext.Build()
            };
        }
    }
}
