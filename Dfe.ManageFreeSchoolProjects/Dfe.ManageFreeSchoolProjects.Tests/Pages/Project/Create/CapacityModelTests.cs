using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Create;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NSubstitute;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageFreeSchoolProjects.Tests.Pages.Project.Create
{
    public class CapacityModelTests
    {
        /// <summary>
        /// The AP / SEN provision inputs are only rendered for a new school mainstream project.
        /// A [Required] attribute on them would be evaluated during model binding for every other
        /// project type, blocking a page the user has no way to complete.
        /// </summary>
        [Fact]
        public void ProvisionCapacities_AreNotUnconditionallyRequired()
        {
            var model = new CapacityModel(new ErrorService(), Substitute.For<ICreateProjectCache>())
            {
                YRY6Capacity = "10",
                Y7Y11Capacity = "20",
                Y12Y14Capacity = "30",
                APResourcesProvision = null,
                SENResourcedProvisionSENUnit = null
            };

            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);

            isValid.Should().BeTrue(because: string.Join(" | ", results.Select(r => r.ErrorMessage)));
        }

        [Theory]
        [InlineData(ProjectType.PresumptionRoute, SchoolType.Mainstream)]
        [InlineData(ProjectType.CentralRoute, SchoolType.Mainstream)]
        [InlineData(ProjectType.NewSchool, SchoolType.Special)]
        public void OnPost_WhenProvisionCapacitiesHidden_SubmitsAndDefaultsThemToZero(
            ProjectType projectType, SchoolType schoolType)
        {
            var cacheItem = BuildCacheItem(projectType, schoolType);
            var model = BuildModel(cacheItem, apResourcesProvision: null, senResourcedProvision: null);

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>();
            model.ModelState.IsValid.Should().BeTrue();
            cacheItem.APResourcesProvision.Should().Be(0);
            cacheItem.SENResourcedProvisionSENUnit.Should().Be(0);
        }

        [Fact]
        public void OnPost_WhenNewSchoolMainstreamAndProvisionCapacitiesMissing_AddsErrors()
        {
            var cacheItem = BuildCacheItem(ProjectType.NewSchool, SchoolType.Mainstream);
            var model = BuildModel(cacheItem, apResourcesProvision: null, senResourcedProvision: null);

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.ModelState["ap-resources-provision"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Enter the AP resources provision");
            model.ModelState["sen-resourced-provision-sen-unit"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Enter the SEN resourced provision / SEN unit");
        }

        [Fact]
        public void OnPost_WhenNewSchoolMainstream_StoresProvisionCapacities()
        {
            var cacheItem = BuildCacheItem(ProjectType.NewSchool, SchoolType.Mainstream);
            var model = BuildModel(cacheItem, apResourcesProvision: "12", senResourcedProvision: "34");

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>();
            cacheItem.APResourcesProvision.Should().Be(12);
            cacheItem.SENResourcedProvisionSENUnit.Should().Be(34);
        }

        [Fact]
        public void OnGet_WhenUserIsNotProjectRecordCreator_ReturnsUnauthorized()
        {
            var model = BuildModel(BuildCacheItem(ProjectType.NewSchool, SchoolType.Mainstream),
                apResourcesProvision: null, senResourcedProvision: null, authorised: false);

            var result = model.OnGet();

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void OnGet_PopulatesCapacitiesFromTheCache()
        {
            var cacheItem = BuildCacheItem(ProjectType.NewSchool, SchoolType.Mainstream);
            cacheItem.Nursery = ClassType.Nursery.Yes;
            cacheItem.NurseryCapacity = 5;
            cacheItem.YRY6Capacity = 10;
            cacheItem.Y7Y11Capacity = 20;
            cacheItem.Y12Y14Capacity = 30;
            cacheItem.APResourcesProvision = 12;
            cacheItem.SENResourcedProvisionSENUnit = 34;

            var model = BuildModel(cacheItem, apResourcesProvision: null, senResourcedProvision: null);

            var result = model.OnGet();

            result.Should().BeOfType<PageResult>();
            model.NurseryCapacity.Should().Be("5");
            model.YRY6Capacity.Should().Be("10");
            model.Y7Y11Capacity.Should().Be("20");
            model.Y12Y14Capacity.Should().Be("30");
            model.APResourcesProvision.Should().Be("12");
            model.SENResourcedProvisionSENUnit.Should().Be("34");
            model.HasNursery.Should().Be(ClassType.Nursery.Yes);
            model.IsLocalAuthority.Should().BeTrue();
            model.IsMainStream.Should().BeTrue();
        }

        [Theory]
        [InlineData(ProjectType.CentralRoute, SchoolType.Mainstream, false, true)]
        [InlineData(ProjectType.NewSchool, SchoolType.Special, true, false)]
        public void OnGet_SetsProvisionFlagsForProjectAndSchoolType(
            ProjectType projectType, SchoolType schoolType, bool expectedLocalAuthority, bool expectedMainstream)
        {
            var model = BuildModel(BuildCacheItem(projectType, schoolType),
                apResourcesProvision: null, senResourcedProvision: null);

            model.OnGet();

            model.IsLocalAuthority.Should().Be(expectedLocalAuthority);
            model.IsMainStream.Should().Be(expectedMainstream);
        }

        [Fact]
        public void OnPost_WhenProjectHasNurseryAndCapacityMissing_AddsError()
        {
            var cacheItem = BuildCacheItem(ProjectType.PresumptionRoute, SchoolType.Mainstream);
            cacheItem.Nursery = ClassType.Nursery.Yes;
            var model = BuildModel(cacheItem, apResourcesProvision: null, senResourcedProvision: null);
            model.NurseryCapacity = null;

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.ModelState["nursery-capacity"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Enter the nursery capacity");
        }

        [Fact]
        public void OnPost_WhenProjectHasNursery_StoresNurseryCapacity()
        {
            var cacheItem = BuildCacheItem(ProjectType.PresumptionRoute, SchoolType.Mainstream);
            cacheItem.Nursery = ClassType.Nursery.Yes;
            var model = BuildModel(cacheItem, apResourcesProvision: null, senResourcedProvision: null);
            model.NurseryCapacity = "5";

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>();
            cacheItem.NurseryCapacity.Should().Be(5);
        }

        [Fact]
        public void OnPost_WhenProjectHasNoNursery_DefaultsNurseryCapacityToZero()
        {
            var cacheItem = BuildCacheItem(ProjectType.PresumptionRoute, SchoolType.Mainstream);
            var model = BuildModel(cacheItem, apResourcesProvision: null, senResourcedProvision: null);
            model.NurseryCapacity = null;

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>();
            cacheItem.NurseryCapacity.Should().Be(0);
        }

        [Fact]
        public void OnPost_StoresTheYearGroupCapacities()
        {
            var cacheItem = BuildCacheItem(ProjectType.PresumptionRoute, SchoolType.Mainstream);
            var model = BuildModel(cacheItem, apResourcesProvision: null, senResourcedProvision: null);

            model.OnPost();

            cacheItem.YRY6Capacity.Should().Be(10);
            cacheItem.Y7Y11Capacity.Should().Be(20);
            cacheItem.Y12Y14Capacity.Should().Be(30);
        }

        [Theory]
        [InlineData(false, RouteConstants.CreateFaithStatus)]
        [InlineData(true, RouteConstants.CreateProjectCheckYourAnswers)]
        public void OnPost_RedirectsToNextPage(bool reachedCheckYourAnswers, string expectedRoute)
        {
            var cacheItem = BuildCacheItem(ProjectType.PresumptionRoute, SchoolType.Mainstream);
            cacheItem.ReachedCheckYourAnswers = reachedCheckYourAnswers;
            var model = BuildModel(cacheItem, apResourcesProvision: null, senResourcedProvision: null);

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>()
                .Which.Url.Should().Be(expectedRoute);
        }

        private static CreateProjectCacheItem BuildCacheItem(ProjectType projectType, SchoolType schoolType)
        {
            return new CreateProjectCacheItem
            {
                ProjectType = projectType,
                SchoolType = schoolType,
                Nursery = ClassType.Nursery.No
            };
        }

        private static CapacityModel BuildModel(
            CreateProjectCacheItem cacheItem,
            string? apResourcesProvision,
            string? senResourcedProvision,
            bool authorised = true)
        {
            var cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            return new CapacityModel(new ErrorService(), cache)
            {
                PageContext = CreatePageTestContext.Build(authorised),
                YRY6Capacity = "10",
                Y7Y11Capacity = "20",
                Y12Y14Capacity = "30",
                APResourcesProvision = apResourcesProvision,
                SENResourcedProvisionSENUnit = senResourcedProvision
            };
        }
    }
}
