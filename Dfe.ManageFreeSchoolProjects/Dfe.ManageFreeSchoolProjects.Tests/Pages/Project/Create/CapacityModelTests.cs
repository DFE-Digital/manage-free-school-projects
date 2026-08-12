using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
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
        [InlineData(ProjectType.LocalAuthority, SchoolType.Special)]
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
            var cacheItem = BuildCacheItem(ProjectType.LocalAuthority, SchoolType.Mainstream);
            var model = BuildModel(cacheItem, apResourcesProvision: null, senResourcedProvision: null);

            var result = model.OnPost();

            result.Should().BeOfType<PageResult>();
            model.ModelState["ap-resources-provision"].Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Enter the AP resources provision");
            model.ModelState["sen-resourced-provision-sen-unit"].Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Enter the SEN resourced provision / SEN unit");
        }

        [Fact]
        public void OnPost_WhenNewSchoolMainstream_StoresProvisionCapacities()
        {
            var cacheItem = BuildCacheItem(ProjectType.LocalAuthority, SchoolType.Mainstream);
            var model = BuildModel(cacheItem, apResourcesProvision: "12", senResourcedProvision: "34");

            var result = model.OnPost();

            result.Should().BeOfType<RedirectResult>();
            cacheItem.APResourcesProvision.Should().Be(12);
            cacheItem.SENResourcedProvisionSENUnit.Should().Be(34);
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
            CreateProjectCacheItem cacheItem, string? apResourcesProvision, string? senResourcedProvision)
        {
            var cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(cacheItem);

            return new CapacityModel(new ErrorService(), cache)
            {
                PageContext = CreatePageTestContext.Build(),
                YRY6Capacity = "10",
                Y7Y11Capacity = "20",
                Y12Y14Capacity = "30",
                APResourcesProvision = apResourcesProvision,
                SENResourcedProvisionSENUnit = senResourcedProvision
            };
        }
    }
}
