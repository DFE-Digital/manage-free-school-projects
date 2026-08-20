using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSubstitute;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Create
{
    public class SchoolTypeModelTests
    {
        [Fact]
        public void OnGet_WhenNewSchool_OffersAlternativeProvisionPruOptions()
        {
            var model = BuildModel(ProjectType.LocalAuthority);

            model.OnGet();

            model.SchoolTypeOptions.Should().Equal(
                SchoolType.AlternativeProvisionPRU,
                SchoolType.Mainstream,
                SchoolType.Special);
        }

        [Theory]
        [InlineData(ProjectType.PresumptionRoute)]
        [InlineData(ProjectType.CentralRoute)]
        public void OnGet_WhenNotNewSchool_OffersFullOptions(ProjectType projectType)
        {
            var model = BuildModel(projectType);

            model.OnGet();

            model.SchoolTypeOptions.Should().Equal(
                SchoolType.AlternativeProvision,
                SchoolType.Mainstream,
                SchoolType.Special,
                SchoolType.StudioSchool,
                SchoolType.UniversityTechnicalCollege,
                SchoolType.VoluntaryAided);
        }

        [Theory]
        [InlineData(ProjectType.LocalAuthority)]
        [InlineData(ProjectType.PresumptionRoute)]
        public void OnGet_NeverOffersNotSetOrFurtherEducation(ProjectType projectType)
        {
            var model = BuildModel(projectType);

            model.OnGet();

            model.SchoolTypeOptions.Should().NotContain(SchoolType.NotSet);
            model.SchoolTypeOptions.Should().NotContain(SchoolType.FurtherEducation);
        }

        [Fact]
        public void OnPost_WhenInvalid_StillPopulatesOptionsForRedisplay()
        {
            var model = BuildModel(ProjectType.LocalAuthority);
            model.ModelState.AddModelError("school-type", "Select the school type");

            model.OnPost();

            model.SchoolTypeOptions.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void OnGet_WhenUserIsNotProjectRecordCreator_ReturnsUnauthorized()
        {
            var model = BuildModel(new CreateProjectCacheItem(), out _, authorised: false);

            var result = model.OnGet();

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void OnGet_PreSelectsTheSchoolTypeAlreadyInTheCache()
        {
            var cacheItem = new CreateProjectCacheItem
            {
                ProjectType = ProjectType.LocalAuthority,
                SchoolType = SchoolType.Special
            };
            var model = BuildModel(cacheItem, out _);

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.SchoolType.Should().Be(((int)SchoolType.Special).ToString());
        }

        [Fact]
        public void OnPost_WhenValid_StoresSchoolTypeAndRedirectsToClassType()
        {
            var cacheItem = new CreateProjectCacheItem { ProjectType = ProjectType.LocalAuthority };
            var model = BuildModel(cacheItem, out var cache);
            model.SchoolType = ((int)SchoolType.Mainstream).ToString();

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(RouteConstants.CreateClassType);
            cacheItem.SchoolType.Should().Be(SchoolType.Mainstream);
            cacheItem.PreviousSchoolType.Should().Be(SchoolType.NotSet);
            cache.Received(1).Update(cacheItem);
        }

        /// <summary>
        /// Changing the school type after check your answers is staged as the previous school type so
        /// the downstream pages can tell what the user is moving away from.
        /// </summary>
        [Fact]
        public void OnPost_WhenAnsweredAfterCheckYourAnswers_StoresPreviousSchoolType()
        {
            var cacheItem = new CreateProjectCacheItem
            {
                ProjectType = ProjectType.LocalAuthority,
                SchoolType = SchoolType.Mainstream,
                ReachedCheckYourAnswers = true
            };
            var model = BuildModel(cacheItem, out _);
            model.SchoolType = ((int)SchoolType.Special).ToString();

            model.OnPost();

            cacheItem.PreviousSchoolType.Should().Be(SchoolType.Special);
            cacheItem.SchoolType.Should().Be(SchoolType.Mainstream);
        }

        private static SchoolTypeModel BuildModel(ProjectType projectType)
        {
            return BuildModel(new CreateProjectCacheItem { ProjectType = projectType }, out _);
        }

        private static SchoolTypeModel BuildModel(
            CreateProjectCacheItem cacheItem, out ICreateProjectCache cache, bool authorised = true)
        {
            cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            return new SchoolTypeModel(new ErrorService(), cache)
            {
                PageContext = CreatePageTestContext.Build(authorised)
            };
        }
    }
}
