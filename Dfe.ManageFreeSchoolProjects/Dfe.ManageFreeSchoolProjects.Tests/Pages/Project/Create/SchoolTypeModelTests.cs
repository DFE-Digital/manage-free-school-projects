using Dfe.ManageFreeSchoolProjects.API.Contracts.Project;
using Dfe.ManageFreeSchoolProjects.Pages.Project.Create.Individual;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using FluentAssertions;
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

        private static SchoolTypeModel BuildModel(ProjectType projectType)
        {
            var cache = Substitute.For<ICreateProjectCache>();
            cache.Get().Returns(new CreateProjectCacheItem { ProjectType = projectType });

            return new SchoolTypeModel(new ErrorService(), cache)
            {
                PageContext = CreatePageTestContext.Build()
            };
        }
    }
}
